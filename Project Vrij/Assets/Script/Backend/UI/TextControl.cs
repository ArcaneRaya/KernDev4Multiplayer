using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextControl : MonoBehaviour
{
    public StringVariable stringVariable;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    public void SetText()
    {
        if (text == null)
        {
            text = GetComponent<Text>();
        }
        text.text = stringVariable.value;
    }
}
