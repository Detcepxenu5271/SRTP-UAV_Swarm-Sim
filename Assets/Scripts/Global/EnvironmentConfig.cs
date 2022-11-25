using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentConfig : MonoBehaviour
{
    public static EnvironmentConfig inst = null;

    // ################ 参数和函数：Box ################

    // box 边界限制的世界坐标
    public float boxbound_up    =  0.5f;
    public float boxbound_down  = -0.5f;
    public float boxbound_north =  0.5f;
    public float boxbound_south = -0.5f;
    public float boxbound_east  =  0.5f;
    public float boxbound_west  = -0.5f;

    /** 生成一个 Box 内的随机坐标
     * 距离边界 d 的范围内不生成 */
    public Vector3 BoxRandomPosition(float d) {
        float x = Random.Range(EnvironmentConfig.inst.boxbound_west  + d, EnvironmentConfig.inst.boxbound_east  - d);
        float y = Random.Range(EnvironmentConfig.inst.boxbound_down  + d, EnvironmentConfig.inst.boxbound_up    - d);
        float z = Random.Range(EnvironmentConfig.inst.boxbound_south + d, EnvironmentConfig.inst.boxbound_north - d);

        return new Vector3(x, y, z);
    }

    /** 获取远离边界的力 */
    public (Vector3, float) GetBoundForce(Agent agent) {
        Vector3 bound_force = Vector3.zero; // 分别计算 xyz 三个轴，累加边界力
        float d = AgentConfig.inst.bound_avoid_dis; // 远离边界的触发距离

        if (boxbound_east - agent.Position.x < d) {
            // 1/t - 1/d，t 是坐标到边界的距离，值域为 [0, +inf]
            bound_force += (1/(boxbound_east - agent.Position.x) - 1/d) * Vector3.left;
        } else
        if (agent.Position.x - boxbound_west < d) {
            bound_force += (1/(agent.Position.x - boxbound_west) - 1/d) * Vector3.right;
        }

        if (boxbound_up - agent.Position.y < d) {
            bound_force += (1/(boxbound_up - agent.Position.y) - 1/d) * Vector3.down;
        } else
        if (agent.Position.y - boxbound_down < d) {
            bound_force += (1/(agent.Position.y - boxbound_down) - 1/d) * Vector3.up;
        }

        if (boxbound_north - agent.Position.z < d) {
            bound_force += (1/(boxbound_north - agent.Position.z) - 1/d) * Vector3.back;
        } else
        if (agent.Position.z - boxbound_south < d) {
            bound_force += (1/(agent.Position.z - boxbound_south) - 1/d) * Vector3.forward;
        }

        // 返回边界力的方向和程度
        // 程度的范围为 [0, 1]，通过函数 1-1/(m+1) 将之前计算的边界力大小从 [0, +inf] 映射到 [0, 1]
        return (bound_force.normalized, 1 - 1/(bound_force.magnitude+1));
    }

    public float air_drag_cof = 0.02f; // 空气阻力系数，包括 C（空气阻力系数）、ρ（空气密度）和 S（物体迎风面积）

    void Awake() {
        inst = this;
    }
}
