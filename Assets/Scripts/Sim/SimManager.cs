using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimManager : MonoBehaviour
{
    public GlobalController globalController; // [手动绑定]
    public Metrics metrics; // [手动绑定]
    public TimePanel timePanel; // XXX [手动绑定] 这里把 UI 组件放在非 UI 类里了，混乱
    
    private AgentManager agentManager;
    
    #region 流程控制参数
    
    private bool isStop = true;
    public bool IsStop {
        get { return isStop; }
    }

    public void Play() {
        if (isStop) {
            isStop = false;
        }
    }
    public void Pause() {
        if (!isStop) {
            isStop = true;
        }
    }

    public enum PlaySpeedType {
        X05,
        X1,
        X2
    }

    private PlaySpeedType playSpeed = PlaySpeedType.X1;
    public PlaySpeedType PlaySpeed {
        get { return playSpeed; }
    }
    public void CyclePlaySpeed() {
        switch (playSpeed) {
        case PlaySpeedType.X05:
            playSpeed = PlaySpeedType.X1;
            Time.timeScale = 1.0f;
            break;
        case PlaySpeedType.X1:
            playSpeed = PlaySpeedType.X2;
            Time.timeScale = 2.0f;
            break;
        case PlaySpeedType.X2:
            playSpeed = PlaySpeedType.X05;
            Time.timeScale = 0.5f;
            break;
        }
    }

    #endregion // 流程控制参数

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

    public Tuple<float, List<float>> GetMetricsOrder() {
        return new Tuple<float, List<float>>(metrics.DeltaTime, metrics.OrderList);
    }

    #region 流程控制

    private float timeCounter;

    public void SimStart() {
        isStop = true;
        playSpeed = PlaySpeedType.X1;

        timeCounter = 0.0f;

        metrics.StartCalcMetrics();
    }
    /** （未结束时）终止仿真 */
    public void SimTerminate() {
        // TODO (可能) AgentManager 清空所有 Agent
    }
    
    /** （ShowResult 完成后）结束整个仿真过程 */
    public void ShowResultEnd() {
        // TODO (可能) AgentManager 清空所有 Agent
    }

    #endregion // 流程控制

    #region 生命周期函数

    void Awake() {
        agentManager = transform.Find("AgentManager").GetComponent<AgentManager>();
    }

    void FixedUpdate() {
        if (!isStop) {
            // TODO agentManager.MoveAllAgents();
            metrics.CalcMetrics(Time.fixedDeltaTime);

            timeCounter += Time.fixedDeltaTime;
            timePanel.SetTimeValue(timeCounter);
            if (timeCounter >= simTime) {
                isStop = true;
                globalController.ExitSimulating2ShowResult();
            }
        }
    }
    
    #endregion // 生命周期函数
}
