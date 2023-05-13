using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public JsonFile jsonFile; // [手动绑定]
    
    private string dataFileName;
    public string DataFileName {
        get { return dataFileName; }
        set { dataFileName = value; }
    }

    public void InitData() {
        jsonFile.ClearData();
    }

    public int DataCount {
        get { return jsonFile.DataCount; }
    }
    public int AgentCount {
        get { return jsonFile.AgentCount; }
    }

    public void AddData(List<Vector3> posList, List<Vector3> velList) {
        jsonFile.AddData(posList, velList);
    }

    public List<Vector3> GetPosList(int index) {
        return jsonFile.GetPosList(index);
    }
    public List<Vector3> GetVelList(int index) {
        return jsonFile.GetVelList(index);
    }

    public void ReadData() {
        jsonFile.FileRead(dataFileName);
    }

    public void WriteData() {
        jsonFile.FileWrite(DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".json");
    }
}
