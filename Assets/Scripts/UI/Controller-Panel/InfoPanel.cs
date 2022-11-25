using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel inst = null;

    public GameObject count_text;

    public void SetCountText() {
        count_text.GetComponent<TMP_Text>().text = Convert.ToString(AgentManager.inst.SpawnCnt);
    }

    void Awake() {
        inst = this;
    }
}
