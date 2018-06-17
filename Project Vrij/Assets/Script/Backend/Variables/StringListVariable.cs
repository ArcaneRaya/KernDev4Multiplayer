using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StringList", menuName = "Variables/String List", order = 0)]
public class StringListVariable : ScriptableObject
{
    public List<string> Value
    {
        get
        {
            if (value == null)
            {
                value = new List<string>();
            }
            return value;
        }
        set
        {
            this.value = value;
        }
    }
    [SerializeField]
    private List<string> value;
}
