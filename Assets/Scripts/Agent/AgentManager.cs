using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    #region 仿真参数

    private int simCount = 10; // 默认初始值 10
    public int SimCount {
        get { return simCount; }
        set { simCount = value; }
    }
    public void SetSimCount(string value) {
        simCount = int.Parse(value);
    }

    private float simTime = 10.0f; // 默认初始值 10.0f
    public float SimTime {
        get { return simTime; }
        set { simTime = value; }
    }
    public void SetSimTime(string value) {
        simTime = (float)Math.Round(float.Parse(value), 2); // 保留两位小数
    }

    #endregion // 仿真参数
}
