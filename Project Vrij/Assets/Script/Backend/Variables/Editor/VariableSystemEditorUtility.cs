using UnityEngine;
using UnityEditor;

public enum ReferenceOptions
{
    REFERENCE,
    CONSTANT
}

public static class VariableSystemEditorUtility
{
    public static GUIStyle DropdownButtonStyle
    {
        get
        {
            if (internal_dropdownButtonStyle == null)
            {
                internal_dropdownButtonStyle = new GUIStyle("PaneOptions");
                internal_dropdownButtonStyle.fontSize = 1;
            }
            return internal_dropdownButtonStyle;
        }
    }
    private static GUIStyle internal_dropdownButtonStyle;

    public static void OptionDropdown(SerializedProperty property, Rect enumRect, ref ReferenceOptions currentOption)
    {
        EditorGUI.BeginChangeCheck();
        currentOption = (ReferenceOptions)EditorGUI.EnumPopup(enumRect, currentOption, VariableSystemEditorUtility.DropdownButtonStyle);
        if (EditorGUI.EndChangeCheck())
        {
            switch (currentOption)
            {
                case ReferenceOptions.CONSTANT:
                    property.FindPropertyRelative("useConstant").boolValue = true;
                    break;
                case ReferenceOptions.REFERENCE:
                    property.FindPropertyRelative("useConstant").boolValue = false;
                    break;
                default:
                    property.FindPropertyRelative("useConstant").boolValue = false;
                    break;
            }
        }
    }
}
