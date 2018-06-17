using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(NetworkedGames.GameInfoBase), true)]
public class GameInfoBaseEditor : Editor
{
    private SerializedProperty property_id;
    private SerializedProperty property_type;

    private void OnEnable()
    {
        property_id = serializedObject.FindProperty("id");
        property_type = serializedObject.FindProperty("gameType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(property_id);
        serializedObject.ApplyModifiedProperties();
        GUI.enabled = false;
        EditorGUILayout.PropertyField(property_type);
        GUI.enabled = true;
        base.OnInspectorGUI();

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            GameUploader.UpdateGameData(
                target as NetworkedGames.GameInfoBase);
        }
        if (GUILayout.Button("Retrieve"))
        {
            GameUploader.GetGameData(property_id.intValue,
                RetrieveGameInfo);
        }
        if (GUILayout.Button("Delete From DB"))
        {
            GameUploader.DeleteGameData(property_id.intValue);
            serializedObject.Update();
            property_id.intValue = -1;
            serializedObject.ApplyModifiedProperties();
        }

        GUILayout.EndHorizontal();
    }

    private void RetrieveGameInfo(string json)
    {
        Debug.Log(json);
    }
}
