using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour
{
    // ################ 参数：对象所挂载的游戏物体的组件，以及其他相关游戏物体 ################

    private Rigidbody rb = null;
    private SphereCollider sphere_collider = null;
    private MeshRenderer mesh_renderer = null; // 用于设置颜色
    private GameObject trail = null; // 拖尾

    // ################ 参数和函数：Agent 信息获取 ################

    private List<Agent> neighbour_list;
    private int get_neighbour_counter;
    private int get_neighbour_interval = 25; // 每隔一定的帧数才更新 neighbour_list

    /** 更新 neighbour_list */
    public void UpdateNeighbourList() {
        neighbour_list.Clear();
        AgentManager.inst.GetNeighbours(this, neighbour_list);
        get_neighbour_counter = 0;
    }

    // ################ 参数和函数：物理 ################

    public float Mass {get => rb.mass;}
    public Vector3 Position {get => rb.position;}
    public Vector3 Velocity {get => rb.velocity;}
    public float ColliderRadius {get => sphere_collider.radius;}

    /** 直接设置 agent 的位置
     * 只能在初始化的时候调用一次（否则由于使用 rigidbody，直接修改 transform 可能会出问题 */
    public void SetPosition(Vector3 pos) {
        // rb.position = pos;
        transform.position = pos;
    }

    /** 直接设置 agent 的速度 */
    /*public void SetVelocity(Vector3 vel) {
        rb.velocity = vel;
    }*/

    /** 计算主动指向力
     * 包括模型传来的指向力（根据集群中的其他个体计算）
     * 和避免环境中的边界、障碍的力 */
    private Vector3 CalcDirForce() {
        Vector3 model_force, env_force, dir_force;

        // 从智能体所用的模型获取指向方向和指向程度
        Vector3 dir;
        float deg;

        (dir, deg) = BoidsModel.inst.GetDirForce(this, neighbour_list);
        // (dir, deg) = (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized, Random.Range(0f, 1f));
        model_force = deg * dir;

        (dir, deg) = EnvironmentConfig.inst.GetBoundForce(this);
        env_force = deg * dir;

        dir_force = model_force + env_force;
        if (dir_force.magnitude > 1) dir_force.Normalize();
        dir_force *= (Mass * Physics.gravity.magnitude);

        // 力的方向为 dir，大小为 [0, mg]，由 [0, 1] 的值 deg 线性决定
        return dir_force;
    }

    /** 合力
     * 传入主动指向力
     * 将升力（与重力互为反作用力）和空气阻力加入合力 */
    private Vector3 ResultantForce(Vector3 dir_force) {
        Vector3 lift_force = Mass * (-Physics.gravity);
        Vector3 air_drag = 0.5f * EnvironmentConfig.inst.air_drag_cof * Mathf.Pow(Velocity.magnitude, 2) * (-Velocity.normalized);
        return lift_force + air_drag + dir_force;
    }

    // ################ 函数：视觉效果 ################

    /** 切换拖尾是否显示 */
    public void ShowTrail(bool show_trail) {
        if (trail) {
            trail.SetActive(show_trail);
        }
    }

    /** 设置随机颜色 */
    private void SetRandomColor() {
        if (mesh_renderer) {
            mesh_renderer.material.SetColor("_Color", Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 1f)); // 随机颜色
        }
    }

    // ################ 函数：生命周期 ################

    void Awake() {
        // 获取组件和游戏物体
        rb = GetComponent<Rigidbody>();
        sphere_collider = GetComponent<SphereCollider>();
        mesh_renderer = GetComponentInChildren<MeshRenderer>();
        trail = transform.Find("Trail").gameObject; // 获取子物体 Trail

        SetRandomColor();

        neighbour_list = new List<Agent>();
        get_neighbour_counter = 0;
    }

    void FixedUpdate() {
        get_neighbour_counter++;
        if (get_neighbour_counter == get_neighbour_interval) { // 更新 neighbour_list
            UpdateNeighbourList();
        }
        rb.AddForce(ResultantForce(CalcDirForce()), ForceMode.Force); // 添加力

        transform.LookAt(Velocity.normalized); // 朝向速度方向
    }

    // ################ 函数：Gizmos ################

    /** 在 Scene 视图中绘制朝向和碰撞半径 */
    private void OnDrawGizmos() {
        if (rb != null) {
            // 绘制红色的速度朝向和大小（1 cm = 1 m/s）
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + 0.01f*Velocity);
            // 绘制绿色的碰撞半径
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ColliderRadius);
        }
    }
}
