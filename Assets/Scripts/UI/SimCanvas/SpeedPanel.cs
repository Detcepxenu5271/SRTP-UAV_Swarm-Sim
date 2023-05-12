using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedPanel : MonoBehaviour
{
    public SimManager simManager; // [手动绑定]
    
    private GameObject MultiplierText;

    public void CyclePlaySpeed() {
        simManager.CyclePlaySpeed();
        
        switch (simManager.PlaySpeed) {
        case SimManager.PlaySpeedType.X05:
            MultiplierText.GetComponent<TextMeshProUGUI>().text = "X0.5";
            break;
        case SimManager.PlaySpeedType.X1:
            MultiplierText.GetComponent<TextMeshProUGUI>().text = "X1.0";
            break;
        case SimManager.PlaySpeedType.X2:
            MultiplierText.GetComponent<TextMeshProUGUI>().text = "X2.0";
            break;
        }
    }

    void OnEnable() {
        MultiplierText.GetComponent<TextMeshProUGUI>().text = "X1.0";
    }

    void Awake() {
        MultiplierText = transform.Find("TX-Multiplier").gameObject;
    }
}
