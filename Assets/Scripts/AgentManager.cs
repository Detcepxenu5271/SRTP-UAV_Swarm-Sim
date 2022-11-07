using System; // Math
using System.Collections;
using System.Collections.Generic; // �������ռ����� List<T>
using UnityEngine;
using Random = UnityEngine.Random;

/** agent ������
 * �����ص���Ϸ���壺AgentManager
 * ���������ڹ��� agent �б�ʵ�� agent �ĳ�ʼ�������٣��Լ�����ʱ���ģ�� */
public class AgentManager : MonoBehaviour
{
    public static AgentManager instance; // ����ģʽ

    // ���в���
    public GameObject agent_prefab; // agent Ԥ�Ƽ�

    public int agent_count; // ��ǰҪ���ɵ� agent ����
    public float time_scale; // �ٶ�����ֵ��Ӱ�� Agent ��ÿ�����µ� delta time

    // ���ó�ʼ����/�ٶ�
    private int spawn_cnt; // ���ɵ��ڼ��� agent �ˣ��� 0 ��ʼ��
    
    public bool is_init_position; // �Ƿ��ֶ����ó�ʼ����
    public List<Vector3> init_position; // ��ʼ����
    public float random_position_range_x; // �����������ʱ�� x �������ΧΪ [-rand.._x, rand.._x]����ͬ
    public float random_position_range_y;
    public float random_position_range_z;

    public bool is_init_velocity; // �Ƿ��ֶ����ó�ʼ�ٶ�
    public List<Vector3> init_velocity; // ��ʼ�ٶ�
    public float random_phi_lower_bound; // ������ɳ�ʼ�ٶȵ���󸩽ǣ��Ƕȱ�ʾ
    public float random_phi_upper_bound; // ������ɳ�ʼ�ٶȵ�������ǣ��Ƕȱ�ʾ
    public float random_velocity_bound; // ������ɳ�ʼ�ٶȣ���СΪ [0, ran...bound]

    // agent �б�
    private List<Agent> agent_list; // agent �б��洢 Agent ��Ϸ�����¹��صĽű������ Agent ����

    /** �ڳ�ʼ�� Agent �б�ʱ������ Agent ������ */
    public Vector3 GeneratePosition() {
        if (!is_init_position) { // ���ֶ����ó�ʼ���꣬���������
            float x = Random.Range(-random_position_range_x, random_position_range_x);
            float y = Random.Range(Agent.collide_radius, random_position_range_y);
            float z = Random.Range(-random_position_range_z, random_position_range_z);
            // Debug.Log("generate random position: " + x + ", " + y + ", " + z);
            return new Vector3(x, y, z);
        } else {
            return init_position[spawn_cnt];
        }
    }
    /** �ڳ�ʼ�� Agent �б�ʱ������ Agent ���ٶ� */
    public Vector3 GenerateVelocity() {
        if (!is_init_velocity) { // ���ֶ����ó�ʼ���꣬���������ȫ����ٶ�
            float lower = MyMath.instance.DegreeToRadian(random_phi_lower_bound); // �Ƕ�ת����
            float upper = MyMath.instance.DegreeToRadian(random_phi_upper_bound);
            float phi = Random.Range(Mathf.PI/2f + lower, Mathf.PI/2f - upper); // �ٶȺ� y ��н� ��
            float theta = Random.Range(0, 2f*Mathf.PI); // �ٶ��� xz ƽ��ͶӰ�� x ��н� �ȣ�[0, 2��]
            float magnitude = Random.Range(0, random_velocity_bound); // �ٶȴ�С
            // Debug.Log("generate random velocity: " + phi*180/Mathf.PI + ", " + theta*180/Mathf.PI);
            return MyMath.instance.VelocityAngleToVector(phi, theta) * magnitude;
        } else {
            return init_velocity[spawn_cnt];
        }
    }

    /** ���� Agent
     * ���������� ����� count ������ǰ agent_count���� SimRoom ����������������
     *       ��������˳�ʼ���� List�����ֶ����ó�ʼ���꣬�����������[TODO] */
    public void SpawnAgent(int count = 0) {
        if (count > 0) { // ��������� 0 �� count ʱ���� agent_count ��Ϊ�����ֵ
            agent_count = count;
        }
        // ���ԭ�е� agent
        if (agent_list.Count > 0) {
            foreach (Agent a in agent_list) {
                Destroy(a.gameObject); // ���ٽű������ص���Ϸ����
            }
            agent_list.Clear(); // ����б�Ԫ��
        }
        // �������� agent_count �� agent
        spawn_cnt = 0;
        for (int i = 0; i < agent_count; ++i) {
            GameObject agent_obj = Instantiate(agent_prefab); // ����Ԥ�Ƽ�����һ�� agent ��Ϸ����
            Agent agent = agent_obj.GetComponent<Agent>(); // ��ȡ agent ��Ϸ������صĵĽű���Agent �ࣩ
            // ���ü��ٶȣ�Ĭ��Ϊ 0
            agent.Accelerate = new Vector3(0f, 0f, 0f);
            // ��� agent ���б���
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
