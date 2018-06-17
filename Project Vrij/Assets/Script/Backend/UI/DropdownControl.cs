using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class DropdownControl : MonoBehaviour
{

    public StringListVariable options;
    public StringReference selectedOption;

    private Dropdown dropdown
    {
        get
        {
            return GetComponent<Dropdown>();
        }
    }

    public void SetOptions()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options.Value);
        dropdown.onValueChanged.RemoveListener(UpdateSelection);
        dropdown.onValueChanged.AddListener(UpdateSelection);
        if (dropdown.value >= dropdown.options.Count)
        {
            dropdown.value = 0;
        }
        if (dropdown.options.Count > 0)
        {
            UpdateSelection(dropdown.value);
        }
    }

    public void UpdateSelection(int selection)
    {
        //Debug.Log(selection);
        selectedOption.Value = dropdown.options[selection].text;
    }
}
