using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FlattenedReference), true)]
public class PropertyDrawerFlattenedReference : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ReferenceOptions chosenOption;

        if (property.FindPropertyRelative("useConstant").boolValue)
        {
            chosenOption = ReferenceOptions.CONSTANT;
        }
        else
        {
            chosenOption = ReferenceOptions.REFERENCE;
        }

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        int oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect enumRect = new Rect(position.x, position.y + 3, 18, 15);
        Rect contentRect = new Rect(position.x + 18, position.y, position.width - 18, position.height);

        VariableSystemEditorUtility.OptionDropdown(property, enumRect, ref chosenOption);

        switch (chosenOption)
        {
            case ReferenceOptions.CONSTANT:
                EditorGUI.PropertyField(
                    contentRect,
                    property.FindPropertyRelative("constantValue"),
                    GUIContent.none
                );
                break;
            case ReferenceOptions.REFERENCE:
                EditorGUI.PropertyField(
                    contentRect,
                    property.FindPropertyRelative("variable"),
                    GUIContent.none
                );
                break;
            default:
                break;
        }
        EditorGUI.indentLevel = oldIndent;
        EditorGUI.EndProperty();
    }
}
