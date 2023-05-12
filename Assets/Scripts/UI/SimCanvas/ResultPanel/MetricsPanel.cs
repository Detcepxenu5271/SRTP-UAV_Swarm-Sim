using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class MetricsPanel : MonoBehaviour
{
    private LineChart orderChart;

    /** 设置 OrderChart 的数据
     * deltaTime: 计算 Metrics 的时间间隔
     * orderList: 每隔时间间隔所计算出的 Order */
    public void SetOrderChart(float deltaTime, List<float> orderList) {
        // 设置坐标轴
        var xAxis = orderChart.EnsureChartComponent<XAxis>();
        xAxis.type = Axis.AxisType.Time;
        var yAxis = orderChart.EnsureChartComponent<YAxis>();
        yAxis.type = Axis.AxisType.Value;

        // 清空默认数据，添加 Line 类型的 Serie 用于接收数据
        orderChart.ClearData();
        orderChart.AddSerie<Line>();

        // 添加 orderList 中的数据，X 坐标为时刻，Y 坐标为 Order
        for (int i = 0; i < orderList.Count; i++) {
            orderChart.AddData(0, Math.Round(i * deltaTime, 3), Math.Round(orderList[i], 6));
        }
    }

    void Awake() {
        orderChart = transform.Find("PN-Order").Find("LineChart").GetComponent<LineChart>();
    }
}
