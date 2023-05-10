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

    #endregion // 模型参数

    public string GetName() {
        return "Boids";
    }
}

}
