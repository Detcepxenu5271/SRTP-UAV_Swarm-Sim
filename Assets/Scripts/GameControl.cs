using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public void Quit() {
#if UNITY_EDITOR //编辑器中退出游戏
        UnityEditor.EditorApplication.isPlaying = false;
#else //应用程序中退出游戏
        UnityEngine.Application.Quit();
#endif
    }
}
