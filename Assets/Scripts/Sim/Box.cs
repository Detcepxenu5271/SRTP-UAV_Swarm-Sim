using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // ################ 参数和函数：Box ################

    // box 边界限制的世界坐标
    public float boxboundUp    =  0.5f;
    public float boxboundDown  = -0.5f;
    public float boxboundNorth =  0.5f;
    public float boxboundSouth = -0.5f;
    public float boxboundEast  =  0.5f;
    public float boxboundWest  = -0.5f;

    /** 生成一个 Box 内的随机坐标
     * 距离边界 d 的范围内不生成 */
    public Vector3 BoxRandomPosition(float d) {
        float x = Random.Range(boxboundWest  + d, boxboundEast  - d);
        float y = Random.Range(boxboundDown  + d, boxboundUp    - d);
        float z = Random.Range(boxboundSouth + d, boxboundNorth - d);

        return new Vector3(x, y, z);
    }

    public float boundAvoidDis;

    /** 获取远离边界的力 */
    public (Vector3, float) GetBoundForce(Agent agent) {
        Vector3 bound_force = Vector3.zero; // 分别计算 xyz 三个轴，累加边界力
        float d = boundAvoidDis; // 远离边界的触发距离

        if (boxboundEast - agent.Position.x < d) {
            // 1/t - 1/d，t 是坐标到边界的距离，值域为 [0, +inf]
            bound_force += (1/(boxboundEast - agent.Position.x) - 1/d) * Vector3.left;
        } else
        if (agent.Position.x - boxboundWest < d) {
            bound_force += (1/(agent.Position.x - boxboundWest) - 1/d) * Vector3.right;
        }

        if (boxboundUp - agent.Position.y < d) {
            bound_force += (1/(boxboundUp - agent.Position.y) - 1/d) * Vector3.down;
        } else
        if (agent.Position.y - boxboundDown < d) {
            bound_force += (1/(agent.Position.y - boxboundDown) - 1/d) * Vector3.up;
        }

        if (boxboundNorth - agent.Position.z < d) {
            bound_force += (1/(boxboundNorth - agent.Position.z) - 1/d) * Vector3.back;
        } else
        if (agent.Position.z - boxboundSouth < d) {
            bound_force += (1/(agent.Position.z - boxboundSouth) - 1/d) * Vector3.forward;
        }

        // 返回边界力的方向和程度
        // 程度的范围为 [0, 1]，通过函数 1-1/(m+1) 将之前计算的边界力大小从 [0, +inf] 映射到 [0, 1]
        return (bound_force.normalized, 1 - 1/(bound_force.magnitude+1));
    }

    public void AcrossBound(Agent agent) {
        float x = agent.Position.x;
        float y = agent.Position.y;
        float z = agent.Position.z;
        if (x > boxboundEast) {
            agent.SetPosition(new Vector3(boxboundWest + (x - boxboundEast), y, z));
        } else
        if (x < boxboundWest) {
            agent.SetPosition(new Vector3(boxboundEast + (x - boxboundWest), y, z));
        }

        if (y > boxboundUp) {
            agent.SetPosition(new Vector3(x, boxboundDown + (y - boxboundUp), z));
        } else
        if (y < boxboundDown) {
            agent.SetPosition(new Vector3(x, boxboundUp + (y - boxboundDown), z));
        }

        if (z > boxboundNorth) {
            agent.SetPosition(new Vector3(x, y, boxboundSouth + (z - boxboundNorth)));
        } else
        if (z < boxboundSouth) {
            agent.SetPosition(new Vector3(x, z, boxboundNorth + (z - boxboundSouth)));
        }
    }
}
