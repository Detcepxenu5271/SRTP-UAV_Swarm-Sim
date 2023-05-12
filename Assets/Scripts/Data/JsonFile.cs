using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class JsonFile : MonoBehaviour
{
    public string data_folder_path = "/Data/"; // 数据文件的存放路径
    
    private JsonData simData;

    private bool FileRead(string filename) {
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
            for (int i = 0; i < simData.Count; ++i) {
                Debug.Log("sim data [" + i + "]:");
                Debug.Log("posList:");
                for (int j = 0; j < simData[i]["posList"].Count; ++j) {
                    Debug.Log(simData[i]["posList"][j][0] + ", " + simData[i]["posList"][j][1] + ", " + simData[i]["posList"][j][2]);
                }
                Debug.Log("velList:");
                for (int j = 0; j < simData[i]["velList"].Count; ++j) {
                    Debug.Log(simData[i]["velList"][j][0] + ", " + simData[i]["velList"][j][1] + ", " + simData[i]["velList"][j][2]);
                }
            }
        } else {
            simData.SetJsonType(JsonType.Array);
        }

        return true;
    }

    // DEBUG
    void Start() {
        // FileRead("form.json");
    }

    void Awake() {
        simData = new JsonData();
    }
}
