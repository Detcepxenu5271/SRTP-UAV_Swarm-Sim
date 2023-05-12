using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metrics : MonoBehaviour
{
    public AgentManager agentManager; // [手动绑定]
    
    // 计算 Metrics 的时间间隔，最好至少数倍于 SimManager 中 FixedUpdate 的时间间隔
    private float deltaTime = 1.0f;
    public float DeltaTime {
        get { return deltaTime; }
        set { deltaTime = value; }
    }

    List<float> orderList; // 每隔时间间隔所计算出的 Order
    public List<float> OrderList {
        get { return orderList; }
    }

    private void CalcOrder() {
        // 从 AgentManager 中获取所有 Agent 的速度，计算 Order
        List<Vector3> velList = agentManager.GetVelList();
        Vector3 aveVel = Vector3.zero;
        foreach (Vector3 vel in velList) {
            aveVel += vel;
        }
        aveVel /= velList.Count;
        orderList.Add(aveVel.magnitude);
    }
    
    private float timeCounter;
    public void CalcMetrics(float timePass) {
        timeCounter += timePass;
        while (timeCounter >= deltaTime) {
            CalcOrder();
            timeCounter -= deltaTime;
        }
    }
    public void StartCalcMetrics() {
        orderList.Clear();
        timeCounter = deltaTime;
    }

    void Awake() {
        orderList = new List<float>();
    }
}
