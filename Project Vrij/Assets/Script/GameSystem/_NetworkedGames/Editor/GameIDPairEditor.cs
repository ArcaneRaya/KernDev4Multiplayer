using UnityEngine;
using UnityEditor;
using NetworkedGames;
using System;

[CustomPropertyDrawer(typeof(GamePack.GameIDPair))]
public class GameIDPairEditor : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);

        int originalIndent = EditorGUI.indentLevel;

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.width /= 2;

        EditorGUI.indentLevel = 0;
        SerializedProperty property_id = property.FindPropertyRelative("id");
        SerializedProperty property_gameInfo = property.FindPropertyRelative("gameInfo");

        if (property_gameInfo.objectReferenceValue == null && property_id.intValue > -1)
        {
            property_gameInfo.objectReferenceValue = FindGameInfo(property_id.intValue);
        }

        EditorGUI.PropertyField(position, property_gameInfo, GUIContent.none);
        if (property_gameInfo.objectReferenceValue != null)
        {
            property_id.intValue = (property_gameInfo.objectReferenceValue as GameInfoBase).id;
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

    private GameInfoBase FindGameInfo(int idValue)
    {
        var results = AssetDatabase.FindAssets("t:GameInfoBase");
        foreach (var item in results)
        {
            string path = AssetDatabase.GUIDToAssetPath(item);
            var asset = AssetDatabase.LoadAssetAtPath<GameInfoBase>(path);
            if (asset.id == idValue)
            {
                return asset;
            }
        }
        return null;
    }
}
