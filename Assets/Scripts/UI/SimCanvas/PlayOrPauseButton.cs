using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayOrPauseButton : MonoBehaviour
{
    public SimManager simManager; // [手动绑定]
    
    public void OnClick() {
        if (simManager.IsStop) {
            simManager.Play();
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/Pause-100x100");
        } else {
            simManager.Pause();
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/Play-100x100");
        }
    }

    void OnEnable() {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/Play-100x100");
    }
}
