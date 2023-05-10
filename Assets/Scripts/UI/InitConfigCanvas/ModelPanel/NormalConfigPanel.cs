using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NormalConfigPanel : MonoBehaviour, IConfigPanel
{
    public AgentManager agentManager; // [手动绑定]
    
    private GameObject simCountValue;
    private GameObject simTimeValue;

    public void SetInitValue() {
        if (simCountValue == null || simTimeValue == null) {
            Debug.Log("NormalConfigPanel: simCountValue or simTimeValue is null");
            Awake();
        }
        simCountValue.GetComponent<TMP_InputField>().text = agentManager.SimCount.ToString();
        simTimeValue.GetComponent<TMP_InputField>().text = agentManager.SimTime.ToString("F2");
    }

    void Awake() {
        if (simCountValue == null) simCountValue = transform.Find("PN-SimCount").Find("IF-Value").gameObject;
        if (simTimeValue == null) simTimeValue = transform.Find("PN-SimTime").Find("IF-Value").gameObject;
    }

    void OnEnable() {
        SetInitValue();
    }

    void Start() {
        // SetInitValue(); // Model Panel 会调用，不用自己初始化
    }
}
