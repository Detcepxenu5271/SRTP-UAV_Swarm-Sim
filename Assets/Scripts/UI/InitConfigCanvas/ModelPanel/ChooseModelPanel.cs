using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChooseModelPanel : MonoBehaviour
{
    public SwarmModelManager swarmModelManager; // [手动绑定]
    
    private GameObject chooseDropdown;

    // 初始化下拉菜单中的模型名称
    public void InitOptions() {
        if (chooseDropdown == null) {
            Debug.Log("ChooseModelPanel: chooseDropdown is null");
            Awake();
        }
        
        chooseDropdown.GetComponent<TMP_Dropdown>().ClearOptions();
        List<string> modelNames = swarmModelManager.GetModelNames();
        foreach (string modelName in modelNames) {
            chooseDropdown.GetComponent<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData(modelName));
        }
        chooseDropdown.GetComponent<TMP_Dropdown>().RefreshShownValue();
    }

    void OnEnable() {
        InitOptions();
    }

    void Awake() {
        if (chooseDropdown == null) chooseDropdown = transform.Find("DD-Choose").gameObject;
    }
}
