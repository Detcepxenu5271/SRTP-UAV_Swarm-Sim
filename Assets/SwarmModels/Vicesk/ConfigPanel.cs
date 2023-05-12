using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Vicesk
{

public class ConfigPanel : MonoBehaviour, IConfigPanel
{
    public Model model; // [手动绑定]
    
    private GameObject fixedSpeedValue;
    private GameObject radiusValue;
    private GameObject noiseValue;

    public void SetInitValue() {
        fixedSpeedValue.GetComponent<TMP_InputField>().text = model.FixedSpeed.ToString("F6");
        radiusValue.GetComponent<TMP_InputField>().text = model.Radius.ToString("F6");
        noiseValue.GetComponent<TMP_InputField>().text = model.Noise.ToString("F6");
    }

    void Awake() {
        fixedSpeedValue = transform.Find("PN-FixedSpeed").Find("IF-Value").gameObject;
        radiusValue = transform.Find("PN-Radius").Find("IF-Value").gameObject;
        noiseValue = transform.Find("PN-Noise").Find("IF-Value").gameObject;
    }

    void Start() {
        SetInitValue();
    }
}

}