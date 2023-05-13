using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public SimManager simManager; // [由 AgentManager 生成时绑定]
    public AgentManager agentManager; // [由 AgentManager 生成时绑定]
    public Box box; // [由 AgentManager 生成时绑定]
    
    // ################ 参数：对象所挂载的游戏物体的组件，以及其他相关游戏物体 ################

    private Rigidbody rb = null;
    private SphereCollider sphere_collider = null;
    private MeshRenderer mesh_renderer = null; // 用于设置颜色
    private GameObject trail = null; // 拖尾

    // ################ 参数和函数：物理 ################

    private bool isStatic;
    public bool IsStatic {
        get { return isStatic; }
        set { isStatic = value; }
    }
    public void SetStatic(bool isSet) {
        if (isSet) {
            isStatic = true;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
        } else {
            isStatic = false;
        }
    }

    public float Mass {get => rb.mass;}
    public Vector3 Position {
        get => isStatic ? transform.position : rb.position;
    }
    public Vector3 Velocity {
        get => isStatic ? staticVelocity : rb.velocity;
    }
    public float ColliderRadius {get => sphere_collider.radius;}

    private Vector3 staticVelocity;

    /** 直接设置 agent 的位置
     * 只能在初始化的时候调用一次（否则由于使用 rigidbody，直接修改 transform 可能会出问题 */
    public void SetStaticPosition(Vector3 pos) {
        // rb.position = pos;
        transform.position = pos;
    }

    public void SetPosition(Vector3 pos) {
        rb.position = pos;
    }

    public void SetStaticVelocity(Vector3 vel) {
        staticVelocity = vel;
    }

    /** 直接设置 agent 的速度 */
    public void SetVelocity(Vector3 vel) {
        rb.velocity = vel;
    }

    /** 计算主动指向力
     * 包括模型传来的指向力（根据集群中的其他个体计算）
     * 和避免环境中的边界、障碍的力 */
    private Vector3 CalcDirForce(Vector3 modelDir, float modelDeg) {
        Vector3 model_force, env_force, dir_force;

        // 从智能体所用的模型获取指向方向和指向程度
        Vector3 dir = modelDir;
        float deg = modelDeg;

        // (dir, deg) = (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized, Random.Range(0f, 1f));
        model_force = deg * dir;

        // 获取远离 Box 边界的力
        (dir, deg) = box.GetBoundForce(this);
        env_force = deg * dir;

        dir_force = model_force + env_force;
        if (dir_force.magnitude > 1) dir_force.Normalize();
        dir_force *= (Mass * Physics.gravity.magnitude);

        // 力的方向为 dir，大小为 [0, mg]，由 [0, 1] 的值 deg 线性决定
        return dir_force;
    }

    /** 合力
     * 传入主动指向力
     * 将空气阻力加入合力 */
    private Vector3 ResultantForce(Vector3 dir_force) {
        // Vector3 lift_force = Mass * (-Physics.gravity); 取消升力和重力的设定
        Vector3 air_drag = 0.5f * simManager.AirDragCof * Mathf.Pow(Velocity.magnitude, 2) * (-Velocity.normalized);
        return /* lift_force +  */air_drag + dir_force;
    }

    // TODO !!! TEST
    private Vector3 ResultantVel(Vector3 vel) {
        // -Velocity: 抵消原来的速度，这样就做到了“把速度变为 vel”，而不是“将速度改变 vel”
        Vector3 resVel =  -Velocity + vel;

        Vector3 dir;
        float deg;
        (dir, deg) = box.GetBoundForce(this);
        
        resVel += dir*deg * Time.fixedDeltaTime;
        return resVel;
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
    }

    /** 由其他（AgentManager 中的）FixedUpdate 函数调用
     * DirForce 类型 */
    public void CalledFixedUpdate(Vector3 dir, float deg) {
        if (!simManager.IsStop) {
            rb.AddForce(ResultantForce(CalcDirForce(dir, deg)), ForceMode.Force); // 添加力

            transform.LookAt(Velocity.normalized); // 朝向速度方向
        }
    }
    /** 由其他（AgentManager 中的）FixedUpdate 函数调用
     * DirVel 类型 */
    public void CalledFixedUpdate(Vector3 vel) {
        if (!simManager.IsStop) {
            rb.AddForce(ResultantVel(vel), ForceMode.VelocityChange); // 改变速度

            transform.LookAt(Velocity.normalized); // 朝向速度方向
        }
    }
    /** 由其他（AgentManager 中的）FixedUpdate 函数调用
     * “导入数据”类型 */
    public void CalledFixedUpdate(Vector3 pos, Vector3 vel) {
        if (!simManager.IsStop) {
            SetStaticPosition(pos);
            SetStaticVelocity(vel);

            transform.LookAt(Velocity.normalized); // 朝向速度方向
        }
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
