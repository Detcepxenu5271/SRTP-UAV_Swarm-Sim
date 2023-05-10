using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimCanvas : MonoBehaviour
{
    private GameObject modelConfigPanel;
    private GameObject dataConfigPanel;

    public void ToggleModelConfigPanel() {
        dataConfigPanel.SetActive(false);
        modelConfigPanel.SetActive(true);
    }
    public void ToggleDataConfigPanel() {
        modelConfigPanel.SetActive(false);
        dataConfigPanel.SetActive(true);
    }

    void Awake() {
        modelConfigPanel = transform.Find("PN-SimConfig").Find("PN-ModelConfig").gameObject;
        dataConfigPanel = transform.Find("PN-SimConfig").Find("PN-DataConfig").gameObject;
    }
}
