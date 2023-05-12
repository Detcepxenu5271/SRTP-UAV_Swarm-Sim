using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayOrPauseButton : MonoBehaviour
{
    public SimManager simManager; // [手动绑定]
    public GameObject fastForwardButton; // [手动绑定]
    
    public void OnClick() {
        if (simManager.IsStop) {
            simManager.Play();
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/Pause-100x100");
            fastForwardButton.GetComponent<Button>().interactable = true;
        } else {
            simManager.Pause();
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/Play-100x100");
            fastForwardButton.GetComponent<Button>().interactable = false;
        }
    }

    void OnEnable() {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/Play-100x100");
    }
}
