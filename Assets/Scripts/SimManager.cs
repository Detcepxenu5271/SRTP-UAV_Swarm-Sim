using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimManager : MonoBehaviour
{
    public GlobalController globalController; // [手动绑定]
    
    private AgentManager agentManager;
    
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

    public void Terminate() {
        isStop = true; // XXX 只需放在 StartSim 时
        // TODO (可能) AgentManager 清空所有 Agent
        globalController.EnterInitConfig();
    }

    void Awake() {
        agentManager = transform.Find("AgentManager").GetComponent<AgentManager>();
    }
}
