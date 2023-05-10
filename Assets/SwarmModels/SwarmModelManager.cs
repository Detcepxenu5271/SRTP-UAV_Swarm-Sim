using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmModelManager : MonoBehaviour
{
    private List<GameObject> models;

    public List<string> GetModelNames() {
        if (models == null) {
            Debug.Log("SwarmModelManager: models is null");
            Awake();
        }
        
        List<string> names = new List<string>();
        foreach (GameObject model in models) {
            names.Add(model.GetComponent<IModel>().GetName());
        }
        return names;
    }

    void Start() {
        // 获取所有子物体
        foreach (Transform child in transform) {
            models.Add(child.gameObject);
        }
    }

    void Awake() {
        if (models == null) models = new List<GameObject>();
    }
}
