using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    private SimManager simManager;
    
    #region 仿真参数

    public int SimCount {
        get { return simManager.SimCount; }
    }

    public float SimTime {
        get { return simManager.SimTime; }
    }

    #endregion // 仿真参数

    void Awake() {
        simManager = transform.parent.GetComponent<SimManager>();
    }
}
