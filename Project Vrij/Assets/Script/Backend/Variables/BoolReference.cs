using UnityEngine;
using System.Collections;

[System.Serializable]
public class BoolReference : FlattenedReference
{
    public bool useConstant = true;
    public bool constantValue;
    public BoolVariable variable;

    public bool Value
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
