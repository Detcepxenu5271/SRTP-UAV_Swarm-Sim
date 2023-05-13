using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimManager : MonoBehaviour
{
    public GlobalController globalController; // [手动绑定]
    public Metrics metrics; // [手动绑定]
    public TimePanel timePanel; // XXX [手动绑定] 这里把 UI 组件放在非 UI 类里了，混乱
    public DataManager dataManager; // [手动绑定]
    
    private AgentManager agentManager;

    public int SimMode {
        get { return globalController.SimMode; }
    }
    
    #region 流程控制参数
    
    private bool isStop = true;
    public bool IsStop {
        get { return isStop; }
    }

    private float timeScaleSaved;

    public void Play() {
        if (isStop) {
            isStop = false;
            Time.timeScale = timeScaleSaved;
        }
    }
    public void Pause() {
        if (!isStop) {
            isStop = true;
            timeScaleSaved = Time.timeScale;
            Time.timeScale = 0f;
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

    private float airDragCof = 0.02f; // 空气阻力系数，包括 C（空气阻力系数）、ρ（空气密度）和 S（物体迎风面积）
    public float AirDragCof {
        get { return airDragCof; }
    }

    #endregion // 仿真参数

    public Tuple<float, List<float>> GetMetricsOrder() {
        return new Tuple<float, List<float>>(metrics.DeltaTime, metrics.OrderList);
    }

    #region 流程控制

    private int simStep;
    public int SimStep {
        get { return simStep; }
    }

    private float timeCounter;
    private int stepCounter;
    public int StepCounter {
        get { return stepCounter; }
    }

    public void SimStart() {
        isStop = false; // 为了保证 Pause 里的操作能执行
        Time.timeScale = 1.0f;
        Pause();
        playSpeed = PlaySpeedType.X1;

        timeCounter = 0.0f;
        stepCounter = 0;

        // model 和 data 有区别
        if (SimMode == -1) {
            simCount = dataManager.AgentCount;
            simStep = dataManager.DataCount;
            agentManager.SpawnAgents(dataManager.GetPosList(0));
        } else {
            agentManager.SpawnAgents(simCount);
        }

        metrics.StartCalcMetrics();
    }
    /** （未结束时）终止仿真 */
    public void SimTerminate() {
        Pause();
        agentManager.DestoryAllAgent();
    }
    
    /** （ShowResult 完成后）结束整个仿真过程 */
    public void ShowResultEnd() {
        agentManager.DestoryAllAgent();

        dataManager.WriteData();
    }

    #endregion // 流程控制

    #region 生命周期函数

    void Awake() {
        agentManager = transform.Find("AgentManager").GetComponent<AgentManager>();
    }

    void FixedUpdate() {
        if (!isStop) {
            agentManager.CalledFixedUpdate();
            metrics.CalcMetrics(Time.fixedDeltaTime);

            timeCounter += Time.fixedDeltaTime;
            timePanel.SetTimeValue(timeCounter);
            stepCounter++;
            if (SimMode == -1) {
                if (stepCounter >= simStep) {
                    Pause();
                    globalController.ExitSimulating2ShowResult();
                }
            } else {
                if (timeCounter >= simTime) {
                    Pause();
                    globalController.ExitSimulating2ShowResult();
                }
            }
        }
    }
    
    #endregion // 生命周期函数
}
