using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimePanel : MonoBehaviour
{
    private GameObject timeValue;

    public void SetTimeValue(float time) {
        timeValue.GetComponent<TMP_Text>().text = time.ToString("F2");
    }

    void OnEnable() {
        SetTimeValue(0.0f);
    }

    void Awake() {
        timeValue = transform.Find("TX-Value").gameObject;
    }
}
