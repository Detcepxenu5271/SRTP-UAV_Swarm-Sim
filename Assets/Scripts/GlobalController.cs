using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public UIManager uiManager; // [手动绑定]
    
    enum State {
        InitConfig,
        Simulating,
        Simulated,
        Analyzing,
        Analyzed,
        Error
    }

    private State state;

    /** 仿真模式
     * -1 表示导入数据仿真
     * 0, 1, 2... 表示使用模型仿真，且数字表示模型编号 */
    private int simMode = 0;
    public int SimMode {
        get { return simMode; }
        set {
            if (value < -1) {
                Debug.Log("GlobalController: SimMode " + value + " out of range");
                return;
            }
            simMode = value;
        }
    }

    public void EnterInitConfig() {
        state = State.InitConfig;

        simMode = 0;
        
        uiManager.SwitchToInitConfig();
    }
    public void EnterSimulaing(bool isSimData) {
        state = State.Simulating;

        if (isSimData) simMode = -1;

        uiManager.SwitchToSim();
    }
    
    public void Quit() {
#if UNITY_EDITOR // 编辑器中退出游戏
        UnityEditor.EditorApplication.isPlaying = false;
#else // 应用程序中退出游戏
        Application.Quit();
#endif
    }

    void Awake() {
        
    }

    void Start() {
        EnterInitConfig();
    }
}
