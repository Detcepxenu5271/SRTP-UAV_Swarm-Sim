using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Boids
{

public class ConfigPanel : MonoBehaviour, IConfigPanel
{
    public Model model; // [手动绑定]
    
    private GameObject cohensionValue;
    private GameObject separationValue;
    private GameObject alignmentValue;
    private GameObject noiseValue;

    public void SetInitValue() {
        cohensionValue.GetComponent<TMP_InputField>().text = model.CohensionFactor.ToString("F6");
        separationValue.GetComponent<TMP_InputField>().text = model.SeparationFactor.ToString("F6");
        alignmentValue.GetComponent<TMP_InputField>().text = model.AlignmentFactor.ToString("F6");
        noiseValue.GetComponent<TMP_InputField>().text = model.NoiseFactor.ToString("F6");
    }
    
    void Awake() {
        cohensionValue = transform.Find("PN-Cohension").Find("IF-Value").gameObject;
        separationValue = transform.Find("PN-Separation").Find("IF-Value").gameObject;
        alignmentValue = transform.Find("PN-Alignment").Find("IF-Value").gameObject;
        noiseValue = transform.Find("PN-Noise").Find("IF-Value").gameObject;
    }

    void Start() {
        SetInitValue();
    }
}

}