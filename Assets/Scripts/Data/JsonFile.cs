using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
using System;

public class JsonFile : MonoBehaviour
{
    public string data_folder_path = "/Data/"; // 数据文件的存放路径
    
    private JsonData simData;

    public void ClearData() {
        simData.Clear();
        simData.SetJsonType(JsonType.Array);
    }

    public int DataCount {
        get { return simData.Count; }
    }
    public int AgentCount {
        get { return DataCount > 0 ? simData[0]["posList"].Count : 0; }
    }
    
    public List<Vector3> GetPosList(int index) {
        List<Vector3> posList = new List<Vector3>();
        for (int i = 0; i < simData[index]["posList"].Count; ++i) {
            float x = float.Parse(simData[index]["posList"][i][0].ToString());
            float y = float.Parse(simData[index]["posList"][i][1].ToString());
            float z = float.Parse(simData[index]["posList"][i][2].ToString());
            posList.Add(new Vector3(x, y, z));
        }
        return posList;
    }

    public List<Vector3> GetVelList(int index) {
        List<Vector3> velList = new List<Vector3>();
        for (int i = 0; i < simData[index]["velList"].Count; ++i) {
            float x = float.Parse(simData[index]["velList"][i][0].ToString());
            float y = float.Parse(simData[index]["velList"][i][1].ToString());
            float z = float.Parse(simData[index]["velList"][i][2].ToString());
            velList.Add(new Vector3(x, y, z));
        }
        return velList;
    }

    public void AddData(List<Vector3> posList, List<Vector3> velList) {
        JsonData data = new JsonData();
        data.SetJsonType(JsonType.Object);

        data["posList"] = new JsonData();
        data["posList"].SetJsonType(JsonType.Array);
        for (int i = 0; i < posList.Count; ++i) {
            data["posList"].Add(new JsonData());
            data["posList"][i].SetJsonType(JsonType.Array);

            JsonData tmp = new JsonData();
            tmp.SetJsonType(JsonType.Double);
            tmp = Math.Round((double)posList[i].x, 3); data["posList"][i].Add(tmp);
            tmp = Math.Round((double)posList[i].y, 3); data["posList"][i].Add(tmp);
            tmp = Math.Round((double)posList[i].z, 3); data["posList"][i].Add(tmp);
        }

        data["velList"] = new JsonData();
        data["velList"].SetJsonType(JsonType.Array);
        for (int i = 0; i < velList.Count; ++i) {
            data["velList"].Add(new JsonData());
            data["velList"][i].SetJsonType(JsonType.Array);

            JsonData tmp = new JsonData();
            tmp.SetJsonType(JsonType.Double);
            tmp = Math.Round((double)velList[i].x, 3); data["velList"][i].Add(tmp);
            tmp = Math.Round((double)velList[i].y, 3); data["velList"][i].Add(tmp);
            tmp = Math.Round((double)velList[i].z, 3); data["velList"][i].Add(tmp);
        }
        simData.Add(data);
    }

    public bool FileRead(string filename) {
        FileStream file;
        string path = Application.dataPath + data_folder_path + filename;

        // 文件不存在
        if (!File.Exists(path)) {
            return false;
        }

        // 打开文件，读取 byte 数据
        file = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[file.Length];
        file.Read(bytes, 0, bytes.Length);
        file.Close();

        if (bytes.Length > 0) {
            // byte 转 Json 对象
            string str = Encoding.UTF8.GetString(bytes);
            simData = JsonMapper.ToObject(str);

            // #DEBUG
            // for (int i = 0; i < simData.Count; ++i) {
            //     Debug.Log("sim data [" + i + "]:");
            //     Debug.Log("posList:");
            //     for (int j = 0; j < simData[i]["posList"].Count; ++j) {
            //         Debug.Log(simData[i]["posList"][j][0] + ", " + simData[i]["posList"][j][1] + ", " + simData[i]["posList"][j][2]);
            //     }
            //     Debug.Log("velList:");
            //     for (int j = 0; j < simData[i]["velList"].Count; ++j) {
            //         Debug.Log(simData[i]["velList"][j][0] + ", " + simData[i]["velList"][j][1] + ", " + simData[i]["velList"][j][2]);
            //     }
            // }
        } else {
            simData.SetJsonType(JsonType.Array);
        }

        return true;
    }

    public void FileWrite(string filename) {
        string path = Application.dataPath + data_folder_path + filename;

        // Json 对象转 byte
        string str = JsonMapper.ToJson(simData);
        byte[] bytes = Encoding.UTF8.GetBytes(str);

        // 写入文件
        FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        file.Write(bytes, 0, bytes.Length);
        file.Flush();
        file.Close();
    }

    // DEBUG
    void Start() {
        // FileRead("form.json");
    }

    void Awake() {
        simData = new JsonData();
        simData.SetJsonType(JsonType.Array);
    }
}
