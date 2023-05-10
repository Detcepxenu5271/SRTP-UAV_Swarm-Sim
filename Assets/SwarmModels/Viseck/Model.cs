using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viseck
{

public class Model : MonoBehaviour, IModel
{
    public string GetName() {
        return "Viseck";
    }
}
    
}
