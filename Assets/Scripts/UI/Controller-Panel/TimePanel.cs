using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimePanel : MonoBehaviour
{
    public GameObject time_scale_slider; // 实际是 TimeSlider-Slider 下的 Slider
    public GameObject toggle_sim_text; // 实际是 ToggleSim-Button 下的 Text

    private bool sim_stopped;

    public void ToggleSim() {
        if (sim_stopped) {
            sim_stopped = false;

            time_scale_slider.GetComponent<Slider>().interactable = true;
            toggle_sim_text.GetComponent<TMP_Text>().text = "Stop Sim";
        } else {
            sim_stopped = true;

            time_scale_slider.GetComponent<Slider>().interactable = false;
            toggle_sim_text.GetComponent<TMP_Text>().text = "Start Sim";
        }
    }

    void Awake() {
        sim_stopped = true;
    }
}
