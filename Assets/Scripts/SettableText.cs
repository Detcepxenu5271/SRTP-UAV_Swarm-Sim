using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Text Mesh Pro

/** 设置所挂载的游戏对象中的 Text Mesh Pro 组件的 text 值 */
public class SettableText : MonoBehaviour {
    public string formatString = "{0:#0.0000}";

    public void SetText(string str) {
        GetComponent<TextMeshProUGUI>().text = str;
    }
    public void SetNumericText(int number) {
        GetComponent<TextMeshProUGUI>().text = string.Format(new CultureInfo("en-US"), formatString, number);
    }
    public void SetNumericText(float number) {
        GetComponent<TextMeshProUGUI>().text = string.Format(new CultureInfo("en-US"), formatString, number);
    }
    public void SetNumericText(double number) {
        GetComponent<TextMeshProUGUI>().text = string.Format(new CultureInfo("en-US"), formatString, number);
    }
}
