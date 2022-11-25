using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ModelPanel : MonoBehaviour
{
    // ################ 类型、参数、函数：模型参数（目前是 Boids 模型） ################

    public GameObject param_value;

    public enum Param {
        Cohension, Separation, Alignment, Target, Noise
    }

    Param cur_param; // current coefficient

    public void ChangeParameterGroup(int id) {
        cur_param = (Param)id;

        switch (cur_param) {
        case Param.Cohension:
            param_value.GetComponent<SettableText>().SetNumericText(BoidsModel.inst.cohension_factor);
            break;
        case Param.Separation:
            param_value.GetComponent<SettableText>().SetNumericText(BoidsModel.inst.separation_factor);
            break;
        case Param.Alignment:
            param_value.GetComponent<SettableText>().SetNumericText(BoidsModel.inst.alignment_factor);
            break;
        case Param.Target:
            param_value.GetComponent<SettableText>().SetNumericText(BoidsModel.inst.target_factor);
            break;
        case Param.Noise:
            param_value.GetComponent<SettableText>().SetNumericText(BoidsModel.inst.noise_factor);
            break;
        }
    }

    public void ChangeParameter(string str) {
        float param = Convert.ToSingle(str);

        switch (cur_param) {
        case Param.Cohension:
            BoidsModel.inst.cohension_factor = param;
            param_value.GetComponent<SettableText>().SetNumericText(param);
            break;
        case Param.Separation:
            BoidsModel.inst.separation_factor = param;
            param_value.GetComponent<SettableText>().SetNumericText(param);
            break;
        case Param.Alignment:
            BoidsModel.inst.alignment_factor = param;
            param_value.GetComponent<SettableText>().SetNumericText(param);
            break;
        case Param.Target:
            BoidsModel.inst.target_factor = param;
            param_value.GetComponent<SettableText>().SetNumericText(param);
            break;
        case Param.Noise:
            BoidsModel.inst.noise_factor = param;
            param_value.GetComponent<SettableText>().SetNumericText(param);
            break;
        }
    }

    // ################ 参数、函数：Target ################

    public GameObject target;

    public void UseTarget(bool ut) {
        BoidsModel.inst.is_use_target = ut;

        target.SetActive(ut);
    }

    // 三个函数传入的值为 [-1, 1] 的比例
    public void SetTargetX(float x) {
        float wx = EnvironmentConfig.inst.boxbound_east * x; // world x
        BoidsModel.inst.target_position.x = wx;
        
        target.transform.position = new Vector3(wx, target.transform.position.y, target.transform.position.z);
    }
    public void SetTargetY(float y) {
        float wy = EnvironmentConfig.inst.boxbound_east * y; // world y
        BoidsModel.inst.target_position.y = wy;
        
        target.transform.position = new Vector3(target.transform.position.x, wy, target.transform.position.z);
    }
    public void SetTargetZ(float z) {
        float wz = EnvironmentConfig.inst.boxbound_east * z; // world z
        BoidsModel.inst.target_position.z = wz;
        
        target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, wz);
    }
}
