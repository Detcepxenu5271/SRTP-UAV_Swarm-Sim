using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 挂载到摄像机上，控制摄像机移动
 * 功能说明：WASD 控制前后左右，EQ 控制上下，按住鼠标右键并拖动控制视角
 *           所控制的移动为相对相机自身的移动 */
public class ViewControl : MonoBehaviour
{
    /** 摄像机状态
     * 0：正常状态
     * 1：允许鼠标控制旋转 */
    private int camera_state;

    public float camera_move_speed; // 摄像机移动速度（m/s）
    public float camera_rotate_speed; // 摄影机旋转速度（乘数因子）

    private float camera_rotate_X; // 相机以自身 x 轴为中心的的绝对旋转量
    private float camera_rotate_Y; // 相机以自身 y 轴为中心的的绝对旋转量

    public GameObject camera_view; // UI 中显示摄像机画面的图像

    /** 鼠标的位置是否在 Camera View 中 */
    private bool MouseInView() {
        if(RectTransformUtility.RectangleContainsScreenPoint(
            camera_view.GetComponent<RectTransform>(),
            Input.mousePosition)) {
            return true;
        } else {
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(gameObject.name + "'s ViewControl Start");
    }

    // Update is called once per frame
    void Update()
    {
        // LShift 键加速
        float speed = camera_move_speed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            speed *= 3f;
        }

        // 在 Camera View 中按住鼠标右键，进入视角旋转模式
        if (Input.GetMouseButtonDown(1) && MouseInView()) {
            camera_state = 1;
        }
        if (Input.GetMouseButtonUp(1)) {
            camera_state = 0;
        }
        
        /** 鼠标控制视角旋转 */
        if (Input.GetMouseButton(1) && camera_state == 1) {
            float mouse_x = Input.GetAxis("Mouse X") * camera_rotate_speed;
            float mouse_y = Input.GetAxis("Mouse Y") * camera_rotate_speed;
            // 计算摄像机旋转的欧拉角
            // 鼠标移动向右、上为正，而欧拉角是向右、下为正（逆时针旋转）
            camera_rotate_X -= mouse_y;
            camera_rotate_Y += mouse_x;
            camera_rotate_X = Mathf.Clamp(camera_rotate_X, -90f, 90f);
            transform.rotation = Quaternion.Euler(camera_rotate_X, camera_rotate_Y, 0);
        }

        // 键盘控制移动
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.forward * speed * Time.unscaledDeltaTime);
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.back * speed * Time.unscaledDeltaTime);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.left * speed * Time.unscaledDeltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * speed * Time.unscaledDeltaTime);
        }
        if (Input.GetKey(KeyCode.E)) {
            transform.Translate(Vector3.up * speed * Time.unscaledDeltaTime);
        }
        if (Input.GetKey(KeyCode.Q)) {
            transform.Translate(Vector3.down * speed * Time.unscaledDeltaTime);
        }
    }
}
