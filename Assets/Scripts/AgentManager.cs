using System; // Math
using System.Collections;
using System.Collections.Generic; // 该命名空间下有 List<T>
using UnityEngine;
using Random = UnityEngine.Random;

/** agent 管理器
 * 所挂载的游戏物体：AgentManager
 * 描述：用于管理 agent 列表，实现 agent 的初始化和销毁，以及基于时间的模拟 */
public class AgentManager : MonoBehaviour
{
    public static AgentManager instance; // 单例模式

    // 公有参数
    public GameObject agent_prefab; // agent 预制件

    public int agent_count; // 当前要生成的 agent 数量
    public float time_scale; // 速度缩放值，影响 Agent 中每步更新的 delta time

    // 设置初始坐标/速度
    private int spawn_cnt; // 生成到第几个 agent 了（从 0 开始）
    
    public bool is_init_position; // 是否手动设置初始坐标
    public List<Vector3> init_position; // 初始坐标
    public float random_position_range_x; // 随机生成坐标时的 x 轴随机范围为 [-rand.._x, rand.._x]，下同
    public float random_position_range_y;
    public float random_position_range_z;

    public bool is_init_velocity; // 是否手动设置初始速度
    public List<Vector3> init_velocity; // 初始速度
    public float random_phi_lower_bound; // 随机生成初始速度的最大俯角，角度表示
    public float random_phi_upper_bound; // 随机生成初始速度的最大仰角，角度表示
    public float random_velocity_bound; // 随机生成初始速度，大小为 [0, ran...bound]

    // agent 列表
    private List<Agent> agent_list; // agent 列表，存储 Agent 游戏物体下挂载的脚本定义的 Agent 对象

    /** 在初始化 Agent 列表时，生成 Agent 的坐标 */
    public Vector3 GeneratePosition() {
        if (!is_init_position) { // 不手动设置初始坐标，则随机生成
            float x = Random.Range(-random_position_range_x, random_position_range_x);
            float y = Random.Range(Agent.collide_radius, random_position_range_y);
            float z = Random.Range(-random_position_range_z, random_position_range_z);
            // Debug.Log("generate random position: " + x + ", " + y + ", " + z);
            return new Vector3(x, y, z);
        } else {
            return init_position[spawn_cnt];
        }
    }
    /** 在初始化 Agent 列表时，生成 Agent 的速度 */
    public Vector3 GenerateVelocity() {
        if (!is_init_velocity) { // 不手动设置初始坐标，则随机生成全向的速度
            float lower = MyMath.instance.DegreeToRadian(random_phi_lower_bound); // 角度转弧度
            float upper = MyMath.instance.DegreeToRadian(random_phi_upper_bound);
            float phi = Random.Range(Mathf.PI/2f + lower, Mathf.PI/2f - upper); // 速度和 y 轴夹角 φ
            float theta = Random.Range(0, 2f*Mathf.PI); // 速度在 xz 平面投影和 x 轴夹角 θ，[0, 2π]
            float magnitude = Random.Range(0, random_velocity_bound); // 速度大小
            // Debug.Log("generate random velocity: " + phi*180/Mathf.PI + ", " + theta*180/Mathf.PI);
            return MyMath.instance.VelocityAngleToVector(phi, theta) * magnitude;
        } else {
            return init_velocity[spawn_cnt];
        }
    }

    /** 生成 Agent
     * 描述：根据 传入的 count 数量或当前 agent_count，在 SimRoom 场景中生成智能体
     *       如果传入了初始坐标 List，则手动设置初始坐标，否则随机生成[TODO] */
    public void SpawnAgent(int count = 0) {
        if (count > 0) { // 当传入大于 0 的 count 时，将 agent_count 设为传入的值
            agent_count = count;
        }
        // 清除原有的 agent
        if (agent_list.Count > 0) {
            foreach (Agent a in agent_list) {
                Destroy(a.gameObject); // 销毁脚本所挂载的游戏物体
            }
            agent_list.Clear(); // 清空列表元素
        }
        // 依次生成 agent_count 个 agent
        spawn_cnt = 0;
        for (int i = 0; i < agent_count; ++i) {
            GameObject agent_obj = Instantiate(agent_prefab); // 根据预制件创建一个 agent 游戏物体
            Agent agent = agent_obj.GetComponent<Agent>(); // 获取 agent 游戏物体挂载的的脚本（Agent 类）
            // 设置加速度，默认为 0
            agent.Accelerate = new Vector3(0f, 0f, 0f);
            // 添加 agent 到列表中
            agent_list.Add(agent);
            spawn_cnt++;
        }
    }

    public void GetNeighbours(Agent agent, List<Agent> neighbour_list) {
         foreach (Agent a in agent_list) {
            if (a != agent
             && Vector3.Distance(agent.Position, a.Position) <= agent.view_distance
             && Vector3.Angle(agent.Velocity, a.Position-agent.Position) <= agent.view_angle)
            neighbour_list.Add(a);
         }
    }

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        agent_list = new List<Agent>();
        SpawnAgent();
    }

    // Update is called once per frame
    void Update() {

    }
}
