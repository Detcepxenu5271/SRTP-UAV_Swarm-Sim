using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsModel : MonoBehaviour
{
    public static BoidsModel inst = null; // 单例模式

    private System.Random rnd;

    // ################ 参数：乘数因子 ################

    public float cohension_factor;
    public float separation_factor;
    public float alignment_factor;
    public float target_factor;
    public float noise_factor; // 噪声因子

    // ################ 参数：目标 target ################

    public bool is_use_target;
    public Vector3 target_position;

    // ################ 函数：Boids 模型的三种力 ################

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

        return dir.normalized; // 返回单位方向向量
        // return dir; // 返回方向向量
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
    
    // ################ 函数：目标 target ################

    /** 目标力 */
    private Vector3 TargetForce(Agent agent, Vector3 target_position) {
        Vector3 dir = Vector3.zero;
        dir = target_position - agent.Position;
        return dir.normalized; // 返回单位方向向量
    }

    // ################ 函数：与 Agent 的接口 ################

    /** 将各种力结合到一起，获取最终指向力 */
    public (Vector3, float) GetDirForce(Agent agent, List<Agent> neighbour_list) {
        Vector3 dir = Vector3.zero;
        
        dir += Cohension(agent, neighbour_list) * cohension_factor;
        dir += Seperation(agent, neighbour_list) * separation_factor;
        dir += Alignment(agent, neighbour_list) * alignment_factor;

        if (is_use_target) {
            dir += TargetForce(agent, target_position) * target_factor;
        }

        // 添加噪声
        float phi = Random.Range(-Mathf.PI/2f, Mathf.PI/2f);
        float theta = Random.Range(0, 2f*Mathf.PI);
        dir = Quaternion.AngleAxis(MyMath.inst.RandomGaussian(0, noise_factor),
                                   MyMath.inst.DirAngleToVector(phi, theta))
              * dir;

        return (dir.normalized, 1 - 1/(dir.magnitude+1));
    }

    // ################ 参数：生命周期 ################

    void Awake() {
        inst = this;
    }
}
