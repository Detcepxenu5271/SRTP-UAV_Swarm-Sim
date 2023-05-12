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

    private GameObject resultPanel;

    public void ShowResultPanel() {
        resultPanel.SetActive(true);
    }
    public void HideResultPanel() {
        resultPanel.SetActive(false);
    }

    /** 把 Metrics 结果显示在 ResultPanel 上
     * deltaTime: 计算 Metrics 的时间间隔
     * orderList: 每隔时间间隔所计算出的 Order */
    public void SetMetrics2ResultPanel(float deltaTime, List<float> orderList) {
        resultPanel.GetComponent<ResultPanel>().SetOrderChart(deltaTime, orderList);
    }

    void Awake() {
        modelConfigPanel = transform.Find("PN-SimConfig").Find("PN-ModelConfig").gameObject;
        dataConfigPanel = transform.Find("PN-SimConfig").Find("PN-DataConfig").gameObject;
        resultPanel = transform.Find("PN-Result").gameObject;
    }
}
