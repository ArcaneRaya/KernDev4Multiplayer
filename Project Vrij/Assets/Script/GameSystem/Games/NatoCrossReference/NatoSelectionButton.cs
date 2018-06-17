using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NatoSelectionButton : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    public void Set(string buttonText, System.Action<int> action, int index)
    {
        this.buttonText.text = buttonText;
        if (action == null)
        {
            button.interactable = false;
        }
        else
        {
            button.onClick.AddListener(() => action(index));
            button.interactable = true;
        }
    }
}