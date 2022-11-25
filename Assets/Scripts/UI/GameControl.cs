using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl inst = null;

    // ################ 参数和函数：Time ################

    private bool sim_stoped;
    public bool SimStoped {get => sim_stoped;}

    private float time_scale_saved;

    public void SetTimeScale(float ts) {
        Time.timeScale = ts;
    }

    public void ToggleSim() {
        if (sim_stoped) { // 当前是暂停状态，则取消暂停
            sim_stoped = false;

            Time.timeScale = time_scale_saved;
        } else { // 当前不是暂停状态，则保存当前 timeScale 并暂停
            sim_stoped = true;
            
            time_scale_saved = Time.timeScale;
            Time.timeScale = 0f;
        }
    }

    public void Quit() {
#if UNITY_EDITOR //编辑器中退出游戏
        UnityEditor.EditorApplication.isPlaying = false;
#else //应用程序中退出游戏
        UnityEngine.Application.Quit();
#endif
    }

    void Awake() {
        inst = this;

        sim_stoped = false;
        ToggleSim();
    }
}
