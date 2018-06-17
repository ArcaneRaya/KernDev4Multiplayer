using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionLayer : MonoBehaviour
{

    public StringVariable descriptionText;
    public UIDisplay uIDisplay;

    public void CheckInstructionLayer()
    {
        if (descriptionText.value == "")
        {
            uIDisplay.DisplayImmediate(false);
        }
    }
}
