using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelConfigPanel : MonoBehaviour
{
    public GlobalController globalController; // [手动绑定]
    
    private List<GameObject> modelConfigPanels;

    /** 切换模型配置面板，index 为模型编号（子物体编号） */
    public void ToggleModelConfigPanel(int index) {
        if (modelConfigPanels == null) {
            Debug.Log("ModelConfigPanel: modelConfigPanels is null");
            Awake();
        }
        
        if (index < 0 || index >= modelConfigPanels.Count) {
            Debug.Log("ModelConfigPanel: index " + index + " out of range");
            return;
        }
        
        foreach (GameObject modelConfigPanel in modelConfigPanels) {
            modelConfigPanel.SetActive(false);
        }
        modelConfigPanels[index].GetComponent<IConfigPanel>().SetInitValue();
        modelConfigPanels[index].SetActive(true);
    }

    void Awake() {
        if (modelConfigPanels == null) modelConfigPanels = new List<GameObject>();
    }

    void OnEnable() {
        if (globalController.SimMode >= 0) {
            ToggleModelConfigPanel(globalController.SimMode);
        }
    }

    void Start() {
        // 获取所有子物体
        foreach (Transform child in transform) {
            modelConfigPanels.Add(child.gameObject);
        }

        if (globalController.SimMode >= 0) { // 防止第一次 OnEnable 时 Start 还没有执行
            ToggleModelConfigPanel(globalController.SimMode);
        }
    }
}
