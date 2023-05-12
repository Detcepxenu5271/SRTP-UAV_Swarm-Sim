using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    private MetricsPanel metricsPanel;

    /** 设置 OrderChart 的数据
     * deltaTime: 计算 Metrics 的时间间隔
     * orderList: 每隔时间间隔所计算出的 Order */
    public void SetOrderChart(float deltaTime, List<float> orderList) {
        if (metricsPanel == null) {
            Awake();
        }
        metricsPanel.SetOrderChart(deltaTime, orderList);
    }

    void Awake() {
        if (metricsPanel == null) metricsPanel = GetComponentInChildren<MetricsPanel>(); // XXX 改成和其他panel一样的获取方式
    }
}
