using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitConfigCanvas : MonoBehaviour
{
    private GameObject modelPanel;
    private GameObject dataPanel;

    /** 是否为“导入数据仿真” */
    // public bool IsSimData() {
    //     return dataPanel.activeSelf;
    // }

    /** 显示“使用模型仿真”的面板 */
    public void ToggleModelPanel(bool isOn) {
        Debug.Log("ToggleModelPanel " + isOn);
        modelPanel.SetActive(isOn);
    }
    /** 显示“导入数据仿真”的面板 */
    public void ToggleDataPanel(bool isOn) {
        Debug.Log("ToggleDataPanel " + isOn);
        dataPanel.SetActive(isOn);
    }

    void Awake() {
        modelPanel = transform.Find("PN-Right").Find("PN-Model").gameObject;
        dataPanel = transform.Find("PN-Right").Find("PN-Data").gameObject;
    }
}
