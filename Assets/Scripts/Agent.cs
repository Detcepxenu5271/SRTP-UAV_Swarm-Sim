// using System; // Mathf
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Agent
 * 所挂载的游戏物体：Agent
 * 描述：存储单个 agent 的位置、速度等参数，实现单个 agent 的基本行为，控制单个 agent 的决策 */
public class Agent : MonoBehaviour
{
    public static float collide_radius = 0.5f; // 碰撞半径（不允许其他 agent 和障碍物进入）
                                               // 【注】目前没有用到

    public float view_angle; // 视野角度
    public float view_distance; // 视野距离
    public float max_accelerate; // 最大加速度
    public float max_velocity; // 最大速度

    private List<Agent> neighbour_list; // 邻居列表，用于每帧计算时从 AgentManager 获取邻居

    [SerializeField] // 调试用，可在 Inspector 中显示
    private Vector3 accelerate; // 当前加速度（世界坐标系下）
    [SerializeField] // 调试用，可在 Inspector 中显示
    private Vector3 velocity; // 当前速度（世界坐标系下）

    // 位置，只读
    public Vector3 Position {
        get {
            return transform.position;
        }
    }
    // 速度，只读
    public Vector3 Velocity {
        get {
            return velocity;
        }
    }
    // 加速度，读写
    public Vector3 Accelerate {
        get {
            return accelerate;
        }
        set {
            accelerate = value;
        }
    }

    private MeshRenderer meshRenderer; // 用于设置颜色

    public void SimulateStep(float delta_time) {
        neighbour_list.Clear();
        AgentManager.instance.GetNeighbours(this, neighbour_list);
        accelerate = BoidsModel.instance.CalcAccelerate(this, neighbour_list);

        // 【测试】远离地面
        accelerate.y += transform.position.y <= collide_radius ? 1f/Mathf.Abs(transform.position.y) : 0f;

        // 限制加速度大小
        accelerate = Vector3.ClampMagnitude(accelerate, max_accelerate);

        // 用加速度更新速度
        if (accelerate.magnitude > 0) {
            velocity = velocity + accelerate * delta_time; // 计算新的速度
            velocity = Vector3.ClampMagnitude(velocity, max_velocity);
        }
        // 用速度更新坐标
        if (velocity.magnitude > 0) { // 参考代码判断了是否大于 0：推测是为了避免 LookAt 出现意外情况
            transform.LookAt(transform.position + velocity.normalized); // 朝向速度方向
            transform.position += velocity * delta_time; // 计算新的位置
        }
    }

    private void Awake() {
        neighbour_list = new List<Agent>();
        transform.position = AgentManager.instance.GeneratePosition();
        velocity = AgentManager.instance.GenerateVelocity();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer) {
            meshRenderer.material.SetColor("_Color", Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 1f)); // 随机颜色
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 将 time_scale 为 0 作为模拟暂停，不执行 if 中的代码，节省计算资源
        if (Mathf.Abs(AgentManager.instance.time_scale) > MyMath.PRECISION) {
            float delta_time = Time.deltaTime * AgentManager.instance.time_scale; // 经过的时间
            SimulateStep(delta_time);
        }
    }

    /** 绘制朝向和碰撞半径 */
    private void OnDrawGizmos() {
        // 绘制红色的速度朝向和大小（1s 走的距离）
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + velocity);
        // 绘制绿色的碰撞半径
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, collide_radius);
    }
}
