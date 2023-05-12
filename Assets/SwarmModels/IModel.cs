using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModel
{
    /** 获取模型名称 */
    string GetName();

    enum ModelMode {
        DirForce,
        DirVel
    }

    ModelMode GetModelMode();

    /** 获取 Agent 集群中各个 Agent 的指向力
     * 传入所有 Agent 的位置和（上一时刻的）速度
     * 返回等长的两个列表，表示每个 Agent 指向力的大小和度量 */
    (List<Vector3>, List<float>) GetDirForce(List<Vector3> posList, List<Vector3> velList);
    
    /** 获取 Agent 集群中各个 Agent 的速度
     * 传入所有 Agent 的位置和（上一时刻的）速度
     * 返回等长的列表，表示每个 Agent 的速度 */
    List<Vector3> GetDirVel(List<Vector3> posList, List<Vector3> velList);
}
