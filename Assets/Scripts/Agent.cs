// using System; // Mathf
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Agent
 * 所挂载的游戏物体：Agent
 * 描述：存储单个 agent 的位置、速度等参数，实现单个 agent 的基本行为，控制单个 agent 的决策 */
public class Agent : MonoBehaviour
{
    public static float collide_radius = 0.3f; // 碰撞半径（不允许其他 agent 和障碍物进入）
                                               // 【注】目前没有用到

    public float view_angle = 135; // 视野角度（速度方向和边界方向的夹角）
    public float view_distance = 10; // 视野距离
    public float max_accelerate; // 最大加速度
    // public float max_velocity; // 最大速度
    public float air_drag_factor; // 空气阻力系数

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

    private MeshRenderer mesh_renderer; // 用于设置颜色
    private GameObject trail; // 拖尾

    public void ShowTrail(bool show_trail) {
        trail.SetActive(show_trail);
    }

    private void BoundConstraint() {
        // 地面限制：当与地面接近时，给一个向上的力；如果太近，停止向下的移动
        if (transform.position.y <= collide_radius*5f) {
            accelerate.y += (collide_radius*5f - transform.position.y) * 10f;
            if (transform.position.y <= collide_radius && velocity.y < 0f) {
                velocity.y = 0f;
            }
        }
    }
    private void AirDrag() {
        accelerate += -air_drag_factor * Mathf.Pow(velocity.magnitude, 2) * velocity.normalized;
    }

    public void SimulateStep(float delta_time) {
        // 获取由集群模型给出的力
        neighbour_list.Clear();
        AgentManager.instance.GetNeighbours(this, neighbour_list); // 获取邻居列表
        accelerate = BoidsModel.instance.CalcForce(this, neighbour_list);

        // 限制加速度大小
        accelerate = Vector3.ClampMagnitude(accelerate, max_accelerate);

        BoundConstraint();
        AirDrag();

        // 用加速度更新速度
        if (accelerate.magnitude > 0) {
            velocity = velocity + accelerate * delta_time; // 计算新的速度
            // velocity = Vector3.ClampMagnitude(velocity, max_velocity); // 限制速度大小
        }
        // 用速度更新坐标
        if (velocity.magnitude > 0) { // 参考代码判断了是否大于 0：推测是为了避免 LookAt 出现意外情况
            transform.LookAt(transform.position + velocity.normalized); // 朝向速度方向
            transform.position += velocity * delta_time; // 计算新的位置
        }
    }

    void Awake() {
        neighbour_list = new List<Agent>();
        transform.position = AgentManager.instance.GeneratePosition();
        velocity = AgentManager.instance.GenerateVelocity();
        mesh_renderer = GetComponentInChildren<MeshRenderer>(); // 获取子物体中的 Mesh Renderer 组件
        trail = transform.Find("Trail").gameObject; // 获取子物体 Trail
        if (mesh_renderer) {
            mesh_renderer.material.SetColor("_Color", Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 1f)); // 随机颜色
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Spawn agent");
    }

    // Update is called once per frame
    void Update()
    {
        // 将 time_scale 为 0 作为模拟暂停，不执行 if 中的代码，节省计算资源
        if (!AgentManager.instance.IsTimeStop()) {
            float delta_time = Time.deltaTime * AgentManager.instance.TimeScale; // 经过的时间
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
