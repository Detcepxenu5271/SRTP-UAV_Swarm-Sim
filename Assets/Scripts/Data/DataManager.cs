using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string dataFileName;
    public string DataFileName {
        get { return dataFileName; }
        set { dataFileName = value; }
    }
}
