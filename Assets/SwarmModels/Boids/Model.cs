using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boids
{

public class Model : MonoBehaviour, IModel
{
    #region 模型参数

    private float cohensionFactor = 1.0f; // 默认初始值 1.0f
    public float CohensionFactor {
        get { return cohensionFactor; }
        set { cohensionFactor = value; }
    }
    public void SetConhensionFactor(string value) {
        cohensionFactor = (float)Math.Round(float.Parse(value), 2); // 保留两位小数
    }

    private float separationFactor = 1.0f; // 默认初始值 1.0f
    public float SeparationFactor {
        get { return separationFactor; }
        set { separationFactor = value; }
    }
    public void SetSeparationFactor(string value) {
        separationFactor = (float)Math.Round(float.Parse(value), 2); // 保留两位小数
    }
    
    private float alignmentFactor = 1.0f; // 默认初始值 1.0f
    public float AlignmentFactor {
        get { return alignmentFactor; }
        set { alignmentFactor = value; }
    }
    public void SetAlignmentFactor(string value) {
        alignmentFactor = (float)Math.Round(float.Parse(value), 2); // 保留两位小数
    }

    private float noiseFactor = 1.0f; // 默认初始值 1.0f
    public float NoiseFactor {
        get { return noiseFactor; }
        set { noiseFactor = value; }
    }
    public void SetNoiseFactor(string value) {
        noiseFactor = (float)Math.Round(float.Parse(value), 2); // 保留两位小数
    }

    private float viewDistance = 0.3f;
    public float ViewDistance {
        get { return viewDistance; }
        set { viewDistance = value; }
    }
    
    private float viewAngle = 135f;
    public float ViewAngle {
        get { return viewAngle; }
        set { viewAngle = value; }
    }

    #endregion // 模型参数

    #region 计算模型参数的方法

    /** 凝聚力 */
    private Vector3 Cohension(Vector3 pos, List<Vector3> neighbourPosList) {
        Vector3 dir = Vector3.zero; // 用于加总各邻居的位置，计算中心点，再计算凝聚方向

        // 没有邻居，直接返回
        if (neighbourPosList.Count == 0) {
            return dir;
        }

        // 计算邻居中心点
        foreach (Vector3 neighbourPos in neighbourPosList) {
            dir += neighbourPos;
        }
        dir /= neighbourPosList.Count;

        // 计算 agent 指向 neighbour 中心的的向量
        dir -= pos;

        return dir.normalized; // 返回单位方向向量
    }
    /** 分离力 */
    private Vector3 Seperation(Vector3 pos, List<Vector3> neighbourPosList) {
        Vector3 dir = Vector3.zero; // 用于加总各邻居的分离向量，得出分离方向

        // 没有邻居，直接返回
        if (neighbourPosList.Count == 0) {
            return dir;
        }

        // 累加分离向量
        foreach (Vector3 neighbourPos in neighbourPosList) {
            Vector3 reverse_dir = pos - neighbourPos;
            dir += reverse_dir.normalized / reverse_dir.magnitude; // 距离越近，分离向量越大
        }

        return dir.normalized; // 返回单位方向向量
        // return dir; // 返回方向向量
    }
    /** 对齐力 */
    private Vector3 Alignment(Vector3 vel, List<Vector3> neighbourVelList) {
        Vector3 dir = Vector3.zero; // 用于加总各邻居的速度向量，得出对齐方向

        // 没有邻居，直接返回
        if (neighbourVelList.Count == 0) {
            return dir;
        }

        // 累加速度方向
        foreach (Vector3 neighbourVel in neighbourVelList) {
            dir += neighbourVel;
        }

        return dir.normalized; // 返回单位方向向量
    }
        
    #endregion // 计算模型参数的方法

    #region IModel 接口函数

    public string GetName() {
        return "Boids";
    }

    public IModel.ModelMode GetModelMode() {
        return IModel.ModelMode.DirForce;
    }

    public (List<Vector3>, List<float>) GetDirForce(List<Vector3> posList, List<Vector3> velList) {
        if (posList.Count != velList.Count) {
            Debug.Log("posList and velList's Count are not equal");
            return (null, null);
        }
        int agentCount = posList.Count;

        List<Vector3> dirList = new List<Vector3>();
        List<float> degList = new List<float>();

        for (int i = 0; i < agentCount; ++i) {
            List<Vector3> neighbourPosList = new List<Vector3>();
            List<Vector3> neighbourVelList = new List<Vector3>();

            // 获取第 i 个 agent 的邻居（坐标列表和速度列表）
            for (int j = 0; j < agentCount; ++j) {
                if (i != j
                   && Vector3.Distance(posList[i], posList[j]) <= viewDistance
                   && Vector3.Angle(velList[i], posList[j]-posList[i]) <= viewAngle)
                {
                    neighbourPosList.Add(posList[j]);
                    neighbourVelList.Add(velList[j]);
                }
            }

            Vector3 dir = Vector3.zero;
        
            // 累加三种力
            dir += Cohension(posList[i], neighbourPosList) * cohensionFactor;
            dir += Seperation(posList[i], neighbourPosList) * separationFactor;
            dir += Alignment(velList[i], neighbourVelList) * alignmentFactor;

            // 添加噪声
            float phi = UnityEngine.Random.Range(-Mathf.PI/2f, Mathf.PI/2f);
            float theta = UnityEngine.Random.Range(0, 2f*Mathf.PI);
            dir = Quaternion.AngleAxis(MyMath.RandomGaussian(0, noiseFactor),
                                       MyMath.DirAngleToVector(phi, theta))
                  * dir;

            dirList.Add(dir.normalized);
            degList.Add(1 - 1/(dir.magnitude+1));
        }

        return (dirList, degList);
    }

    public List<Vector3> GetDirVel(List<Vector3> posList, List<Vector3> velList) {
        return null;
    }

    #endregion // IModel 接口函数
}

}
