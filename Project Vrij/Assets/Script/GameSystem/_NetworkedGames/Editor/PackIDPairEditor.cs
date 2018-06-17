using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(GamePack.PackIDPair))]
public class PackIDPairEditor : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);

        int originalIndent = EditorGUI.indentLevel;

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.width /= 2;

        EditorGUI.indentLevel = 0;
        SerializedProperty property_id = property.FindPropertyRelative("id");
        SerializedProperty property_gameInfo = property.FindPropertyRelative("packInfo");
        EditorGUI.PropertyField(position, property_gameInfo, GUIContent.none);
        if (property_gameInfo.objectReferenceValue != null)
        {
            property_id.intValue = (property_gameInfo.objectReferenceValue as GamePack).id;
        }
        else
        {
            property_id.intValue = -1;
        }
        GUI.enabled = false;
        position.x += position.width;
        EditorGUI.PropertyField(position, property_id, GUIContent.none);
        GUI.enabled = true;

        EditorGUI.indentLevel = originalIndent;

        EditorGUI.EndProperty();
    }
}
