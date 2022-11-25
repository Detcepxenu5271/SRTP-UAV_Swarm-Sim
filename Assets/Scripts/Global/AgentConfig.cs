using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentConfig : MonoBehaviour
{
    public static AgentConfig inst = null;

    public float collider_radius = 0.01f;
    public float bound_avoid_dis = 0.1f; // 在这个距离内就会开始避开边界

    public float view_distance = 0.3f;
    public float view_angle = 135f;

    void Awake() {
        inst = this;
    }
}
