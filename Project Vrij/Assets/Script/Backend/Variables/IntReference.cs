using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntReference : FlattenedReference
{
    public bool useConstant = true;
    public int constantValue;
    public IntVariable variable;

    public int Value
    {
        get
        {
            if (useConstant)
            {
                return constantValue;
            }
            else
            {
                if (variable == null)
                {
                    throw new System.NullReferenceException("The variable has not been set in the inspector.");
                }
                return variable.value;
            }
        }
        set
        {
            if (useConstant)
            {
                constantValue = value;
            }
            else
            {
                if (variable == null)
                {
                    throw new System.NullReferenceException("The variable has not been set in the inspector.");
                }
                variable.value = value;
            }
        }
    }
}
