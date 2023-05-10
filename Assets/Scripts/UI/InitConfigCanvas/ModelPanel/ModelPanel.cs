using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelPanel : MonoBehaviour
{
    private ChooseModelPanel chooseModelPanel;
    private NormalConfigPanel normalConfigPanel;
    private ModelConfigPanel modelConfigPanel;
    
    void Awake() {
        chooseModelPanel = transform.Find("PN-ChooseModel").GetComponent<ChooseModelPanel>();
        normalConfigPanel = transform.Find("PN-NormalConfig").GetComponent<NormalConfigPanel>();
        modelConfigPanel = transform.Find("PN-ModelConfig").GetComponent<ModelConfigPanel>();
    }
    
    void OnEnable() {

    }
}
