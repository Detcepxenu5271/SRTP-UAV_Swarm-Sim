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

    private ModelConfigPanel modelConfigPanel;

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

    void Awake() {
        initConfigCanvas = transform.Find("CV-InitConfig").gameObject;
        simCanvas = transform.Find("CV-Sim").gameObject;
        modelConfigPanel = simCanvas.transform.Find("PN-SimConfig").Find("PN-ModelConfig").GetComponent<ModelConfigPanel>();
    }
}
