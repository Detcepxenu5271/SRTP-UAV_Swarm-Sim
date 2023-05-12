using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vicesk
{

public class Model : MonoBehaviour, IModel
{
    private float fixedSpeed = 0.1f;
    public float FixedSpeed {
        get { return fixedSpeed; }
        set { fixedSpeed = value; }
    }
    public void SetFixedSpeed(string value) {
        fixedSpeed = (float)Math.Round(float.Parse(value), 3);
    }

    private float radius = 0.2f;
    public float Radius {
        get { return radius; }
        set { radius = value; }
    }
    public void SetRadius(string value) {
        radius = (float)Math.Round(float.Parse(value), 3);
    }

    private float noise = 0.1f;
    public float Noise {
        get { return noise; }
        set { noise = value; }
    }
    public void SetNoise(string value) {
        noise = (float)Math.Round(float.Parse(value), 3);
    }

    private Vector3 GetNoiseVel() {
        // 添加噪声
        Vector3 noiseVel;
        noiseVel.x = UnityEngine.Random.Range(-noise, noise);
        noiseVel.y = UnityEngine.Random.Range(-noise, noise);
        noiseVel.z = UnityEngine.Random.Range(-noise, noise);
        return noiseVel * fixedSpeed;
    }
    
    public string GetName() {
        return "Vicesk";
    }

    public IModel.ModelMode GetModelMode() {
        return IModel.ModelMode.DirVel;
    }

    public (List<Vector3>, List<float>) GetDirForce(List<Vector3> posList, List<Vector3> velList) {
        return (null, null);
    }

    public List<Vector3> GetDirVel(List<Vector3> posList, List<Vector3> velList) {
        if (posList.Count != velList.Count) {
            Debug.Log("posList and velList's Count are not equal");
            return null;
        }
        int agentCount = posList.Count;

        List<Vector3> resVelList = new List<Vector3>();

        for (int i = 0; i < agentCount; ++i) {
            for (int j = 0; j < agentCount; ++j) {
                Vector3 aveVel = Vector3.zero;
                if (i != j && Vector3.Distance(posList[i], posList[j]) <= radius) {
                    aveVel += velList[j];
                }
                aveVel /= agentCount;

                resVelList.Add((aveVel + GetNoiseVel()).normalized * fixedSpeed);
            }
        }
        
        return resVelList;
    }
}
    
}
