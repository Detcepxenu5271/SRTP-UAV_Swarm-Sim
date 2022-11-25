using System; // Math
using System.Collections;
using System.Collections.Generic; // 该命名空间下有 List<T>
using UnityEngine;
using Random = UnityEngine.Random; // 使用 UnityEngine 命名空间下的 Random

/** agent 管理器
 * 所挂载的游戏物体：AgentManager
 * 描述：用于管理 agent 列表，实现 agent 的初始化和销毁 */
public class AgentManager : MonoBehaviour
{
    public static AgentManager inst = null; // 单例模式

    // ################ 参数：对象所挂载的游戏物体的组件，以及其他相关游戏物体 ################

    public GameObject agent_prefab; // agent 预制件

    // ################ 参数：Agent 列表 ################

    // agent 列表
    private List<Agent> agent_list; // agent 列表，存储 Agent 游戏物体下挂载的脚本定义的 Agent 对象

    private int spawn_cnt; // 已经生成的 agent 数目
    public int SpawnCnt {get => spawn_cnt;}

    // ################ 函数：Agent 外观效果 ################

    private bool show_trail = true;

    /** agent 是否显示拖尾，由 UI 控制 */
    public void ShowTrail(bool st) {
        show_trail = st;
        foreach (Agent agent in agent_list) {
            agent.ShowTrail(st);
        }
    }

    // ################ 函数：Agent 生成和销毁 ################

    /** 在 Box 中随机生成一个 agent，添加到 agent_list 中 */
    public void SpawnSingleAgent() {
        GameObject agent_obj = Instantiate(agent_prefab);
        Agent agent = agent_obj.GetComponent<Agent>();

        // 设置随机随机坐标
        agent.SetPosition(EnvironmentConfig.inst.BoxRandomPosition(AgentConfig.inst.collider_radius));

        // 如果全局设置为不显示拖尾，则取消初始化的拖尾显示
        if (!show_trail) {
            agent.ShowTrail(false);
        }

        agent_list.Add(agent);
        spawn_cnt++;
    }

    /** 在 pos 处生成一个 agent，添加到 agent_list 中 */
    public void SpawnSingleAgent(Vector3 pos) {
        GameObject agent_obj = Instantiate(agent_prefab);
        Agent agent = agent_obj.GetComponent<Agent>();

        // 设置初始坐标为 pos
        agent.SetPosition(pos);

        // 如果全局设置为不显示拖尾，则取消初始化的拖尾显示
        if (!show_trail) {
            agent.ShowTrail(false);
        }

        agent_list.Add(agent);
        spawn_cnt++;
    }

    public void SpawnTenAgent() {
        for (int i = 0; i < 10; ++i) {
            SpawnSingleAgent();
        }
    }

    /** 删除 agent_list 中的最后一个元素 */
    public void DestorySingleAgent() {
        if (agent_list.Count > 0) {
            Destroy(agent_list[agent_list.Count-1].gameObject);
            agent_list.RemoveAt(agent_list.Count-1);

            spawn_cnt--;

            UpdateNeighbours();
        }
    }

    public void DestorySingleAgent(Agent agent) {
        for (int i = 0; i < agent_list.Count; i++) {
            if (agent_list[i] == agent) {
                Destroy(agent_list[i].gameObject);
                agent_list.RemoveAt(i);

                spawn_cnt--;
            
                UpdateNeighbours();

                break;
            }
        }
    }

    public void DestoryAllAgent() {
        foreach (Agent agent in agent_list) {
            Destroy(agent.gameObject);
        }
        agent_list.Clear();

        spawn_cnt = 0;

        UpdateNeighbours();
    }

    // ################ 函数：Agent 信息获取 ################

    /** 获取某个 agent 的邻居 */
    public void GetNeighbours(Agent agent, List<Agent> neighbour_list) {
         foreach (Agent a in agent_list) {
            if (a != agent
             && Vector3.Distance(agent.Position, a.Position) <= AgentConfig.inst.view_distance
             && Vector3.Angle(agent.Velocity, a.Position-agent.Position) <= AgentConfig.inst.view_angle)
            neighbour_list.Add(a);
         }
    }

    /** 更新所有 agent 的邻居 */
    public void UpdateNeighbours() {
        foreach (Agent a in agent_list) {
            a.UpdateNeighbourList();
        }
    }

    // ################ 函数：生命周期 ################

    void Awake() {
        inst = this;

        spawn_cnt = 0;
    }

    void Start() {
        agent_list = new List<Agent>();
    }
}
