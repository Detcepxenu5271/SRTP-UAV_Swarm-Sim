using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmModelManager : MonoBehaviour
{
    public GlobalController globalController; // [手动绑定]
    public Box box; // [手动绑定]
    
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

    private (List<Vector3>, List<float>) GetDirForce(List<Vector3> posList, List<Vector3> velList) {
        if (globalController.SimMode == -1) {
            return (null, null);
        }
        return models[globalController.SimMode].GetComponent<IModel>().GetDirForce(posList, velList);
    }
    private List<Vector3> GetDirVel(List<Vector3> posList, List<Vector3> velList) {
        if (globalController.SimMode == -1) {
            return null;
        }
        return models[globalController.SimMode].GetComponent<IModel>().GetDirVel(posList, velList);
    }

    public void CallModelForAgents(List<Agent> agents) {
        List<Vector3> posList = new List<Vector3>();
        List<Vector3> velList = new List<Vector3>();

        foreach (Agent agent in agents) {
            posList.Add(agent.Position);
            velList.Add(agent.Velocity);
        }
        
        switch (models[globalController.SimMode].GetComponent<IModel>().GetModelMode()) {
        case IModel.ModelMode.DirForce:
            List<Vector3> dirList;
            List<float> degList;
            (dirList, degList) = models[globalController.SimMode].GetComponent<IModel>().GetDirForce(posList, velList);
            for (int i = 0; i < agents.Count; ++i) {
                agents[i].CalledFixedUpdate(dirList[i], degList[i]);
            }
            break;
        case IModel.ModelMode.DirVel:
            List<Vector3> resVelList;
            resVelList = models[globalController.SimMode].GetComponent<IModel>().GetDirVel(posList, velList);
            for (int i = 0; i < agents.Count; ++i) {
                agents[i].CalledFixedUpdate(resVelList[i]);
                // box.AcrossBound(agents[i]);
            }
            break;
        default:
            break;
        }
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
