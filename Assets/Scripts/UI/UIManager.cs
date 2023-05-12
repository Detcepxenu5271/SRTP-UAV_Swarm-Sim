using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GlobalController globalController; // [手动绑定]
    
    private GameObject initConfigCanvas;
    private GameObject simCanvas;

    /** 是否为“导入数据仿真” */
    // public bool IsSimData() {
    //     return initConfigCanvas.GetComponent<InitConfigCanvas>().IsSimData();
    // }

    public void SwitchToInitConfig() {
        simCanvas.SetActive(false);
        initConfigCanvas.SetActive(true);
    }
    public void SwitchToSim() {
        initConfigCanvas.SetActive(false);
        simCanvas.SetActive(true);
        if (globalController.SimMode >= 0) {
            simCanvas.GetComponent<SimCanvas>().ToggleModelConfigPanel();
        } else {
            simCanvas.GetComponent<SimCanvas>().ToggleDataConfigPanel();
        }
    }

    public void ShowResultPanel() {
        if (globalController.State == GlobalController.StateType.ShowResult) {
            simCanvas.GetComponent<SimCanvas>().ShowResultPanel();
        }
    }
    public void HideResultPanel() {
        if (globalController.State == GlobalController.StateType.ShowResult) {
            simCanvas.GetComponent<SimCanvas>().HideResultPanel();
        }
    }

    /** 把 Metrics 结果显示在 ResultPanel 上
     * deltaTime: 计算 Metrics 的时间间隔
     * orderList: 每隔时间间隔所计算出的 Order */
    public void SetMetrics2ResultPanel(Tuple<float, List<float>> tp) {
        if (globalController.State == GlobalController.StateType.ShowResult) {
            simCanvas.GetComponent<SimCanvas>().SetMetrics2ResultPanel(tp.Item1, tp.Item2);
        }
    }

    void Awake() {
        initConfigCanvas = transform.Find("CV-InitConfig").gameObject;
        simCanvas = transform.Find("CV-Sim").gameObject;
    }
}
