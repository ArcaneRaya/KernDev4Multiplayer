using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StringVariable", menuName = "Variables/String", order = 0)]
public class StringVariable : ScriptableObject
{
    public string value;

    public void SetValue(string val)
    {
        value = val;
    }
}
