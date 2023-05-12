using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    // ################ 参数：对象所挂载的游戏物体的组件，以及其他相关游戏物体 ################

    private GameObject agentPrefab; // agent 预制件
    
    public GameObject agentsHolder; // [手动绑定]
    public Box box; // [手动绑定]
    public SwarmModelManager swarmModelManager; // [手动绑定]
    
    private SimManager simManager;
    
    #region 仿真参数

    public int SimCount {
        get { return simManager.SimCount; }
    }

    public float SimTime {
        get { return simManager.SimTime; }
    }

    private float colliderRadius;

    #endregion // 仿真参数

    // ################ 参数：Agent 列表 ################

    // agent 列表
    private List<Agent> agentList; // agent 列表，存储 Agent 游戏物体下挂载的脚本定义的 Agent 对象

    private int spawnCnt; // 已经生成的 agent 数目
    public int SpawnCnt {get => spawnCnt;}

    public List<Vector3> GetVelList() {
        List<Vector3> velList = new List<Vector3>();
        foreach (Agent agent in agentList) {
            velList.Add(agent.Velocity);
        }
        return velList;
    }

    // ################ 函数：Agent 外观效果 ################

    private bool showTrail = true;

    /** agent 是否显示拖尾，由 UI 控制 */
    public void ShowTrail(bool st) {
        showTrail = st;
        foreach (Agent agent in agentList) {
            agent.ShowTrail(st);
        }
    }

    // ################ 函数：Agent 生成和销毁 ################

    /** 在 Box 中随机生成一个 agent，添加到 agent_list 中 */
    public void SpawnSingleAgent() {
        GameObject agentObj = Instantiate(agentPrefab);
        agentObj.transform.SetParent(agentsHolder.transform);

        Agent agent = agentObj.GetComponent<Agent>();
        agent.simManager = simManager;
        agent.agentManager = this;
        agent.box = box;

        // 设置随机随机坐标
        agent.SetPosition(box.BoxRandomPosition(colliderRadius));

        // 如果全局设置为不显示拖尾，则取消初始化的拖尾显示
        if (!showTrail) {
            agent.ShowTrail(false);
        }

        agentList.Add(agent);
        spawnCnt++;
    }

    /** 在 pos 处生成一个 agent，添加到 agent_list 中 */
    public void SpawnSingleAgent(Vector3 pos) {
        GameObject agentObj = Instantiate(agentPrefab);
        agentObj.transform.SetParent(agentsHolder.transform);

        Agent agent = agentObj.GetComponent<Agent>();
        agent.simManager = simManager;
        agent.agentManager = this;
        agent.box = box;

        // 设置随机随机坐标
        agent.SetPosition(pos);

        // 如果全局设置为不显示拖尾，则取消初始化的拖尾显示
        if (!showTrail) {
            agent.ShowTrail(false);
        }

        agentList.Add(agent);
        spawnCnt++;
    }

    public void SpawnAgents(int agentCnt) {
        for (int i = 0; i < agentCnt; ++i) {
            SpawnSingleAgent();
        }
    }

    public void SpawnAgents(List<Vector3> posList) {
        foreach (var pos in posList) {
            SpawnSingleAgent(pos);
        }
    }

    /** 删除 agent_list 中的最后一个元素 */
    public void DestorySingleAgent() {
        if (agentList.Count > 0) {
            Destroy(agentList[agentList.Count-1].gameObject);
            agentList.RemoveAt(agentList.Count-1);

            spawnCnt--;
        }
    }

    public void DestorySingleAgent(Agent agent) {
        for (int i = 0; i < agentList.Count; i++) {
            if (agentList[i] == agent) {
                Destroy(agentList[i].gameObject);
                agentList.RemoveAt(i);

                spawnCnt--;
            
                break;
            }
        }
    }

    public void DestoryAllAgent() {
        foreach (Agent agent in agentList) {
            Destroy(agent.gameObject);
        }
        agentList.Clear();

        spawnCnt = 0;
    }

    // ################ 函数：生命周期 ################

    void Awake() {
        agentPrefab = Resources.Load<GameObject>("Prefabs/Agent");
        simManager = transform.parent.GetComponent<SimManager>();

        colliderRadius = agentPrefab.GetComponent<SphereCollider>().radius;

        spawnCnt = 0;
    }

    void Start() {
        agentList = new List<Agent>();
    }

    void FixedUpdate() {
        if (!simManager.IsStop) {
            swarmModelManager.CallModelForAgents(agentList);
        }
    }
}
