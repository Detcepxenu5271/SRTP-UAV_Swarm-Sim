using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsModel : MonoBehaviour
{
    public static BoidsModel instance = null; // 单例模式

    // 乘数因子
    public float total_factor;
    public float cohension_factor;
    public float seperation_factor;
    public float alignment_factor;
    public float target_factor;

    // 目标点
    public bool is_use_target;
    public Vector3 target_position;

    // 【？】使用相同的邻居，半径、视野等定义在 Agent 中
    // 三种力的生效半径
    // public float cohension_radius;
    // public float seperate_radius;
    // public float alignment_radius;

    // private Agent agent; // 要计算的智能体
    // private List<Agent> neighbour_list; // agent 的邻居（依据附近一定范围内其他 agent 的位置进行决策）

    /** 凝聚力 */
    private Vector3 Cohension(Agent agent, List<Agent> neighbour_list) {
        Vector3 dir = Vector3.zero; // 用于加总各邻居的位置，计算中心点，再计算凝聚方向

        // 没有邻居，直接返回
        if (neighbour_list.Count == 0) {
            return dir;
        }

        // 计算邻居中心点
        foreach (Agent neighbour in neighbour_list) {
            dir += neighbour.Position;
        }
        dir /= neighbour_list.Count;

        // 计算 agent 指向 neighbour 中心的的向量
        dir -= agent.Position;

        return dir.normalized; // 返回单位方向向量
    }
    /** 分离力 */
    private Vector3 Seperation(Agent agent, List<Agent> neighbour_list) {
        Vector3 dir = Vector3.zero; // 用于加总各邻居的分离向量，得出分离方向

        // 没有邻居，直接返回
        if (neighbour_list.Count == 0) {
            return dir;
        }

        // 累加分离向量
        foreach (Agent neighbour in neighbour_list) {
            Vector3 reverse_dir = agent.Position - neighbour.Position;
            dir += reverse_dir.normalized / reverse_dir.magnitude; // 距离越近，分离向量越大
        }

        // return dir.normalized; // 返回单位方向向量
        return dir; // 返回方向向量
    }
    /** 对齐力 */
    private Vector3 Alignment(Agent agent, List<Agent> neighbour_list) {
        Vector3 dir = Vector3.zero; // 用于加总各邻居的速度向量，得出对齐方向

        // 没有邻居，直接返回
        if (neighbour_list.Count == 0) {
            return dir;
        }

        // 累加速度方向
        foreach (Agent neighbour in neighbour_list) {
            dir += neighbour.Velocity;
        }

        return dir.normalized; // 返回单位方向向量
    }
    /** 目标力 */
    private Vector3 TargetForce(Agent agent, Vector3 target_position) {
        Vector3 dir = Vector3.zero;
        dir = target_position - agent.Position;
        return dir.normalized; // 返回单位方向向量
    }
    /** 将各种力结合到一起，计算最终力 */
    public Vector3 CalcForce(Agent agent, List<Agent> neighbour_list) {
        Vector3 dir = Vector3.zero;
        dir += Cohension(agent, neighbour_list) * cohension_factor;
        dir += Seperation(agent, neighbour_list) * seperation_factor;
        dir += Alignment(agent, neighbour_list) * alignment_factor;
        if (is_use_target) {
            dir += TargetForce(agent, target_position) * target_factor;
        }
        return dir;
    }

    void Awake() {
        instance = this;
    }
}
