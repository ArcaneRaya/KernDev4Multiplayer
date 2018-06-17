using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GamePack))]
public class GamePackEditor : Editor
{
    private SerializedProperty property_id;

    private void OnEnable()
    {
        property_id = serializedObject.FindProperty("id");
    }

    public override void OnInspectorGUI()
    {
        //GUI.enabled = false;
        serializedObject.Update();
        EditorGUILayout.PropertyField(property_id);
        serializedObject.ApplyModifiedProperties();
        //GUI.enabled = true;
        base.OnInspectorGUI();

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            GameUploader.UpdateGamePack(
                target as GamePack);
        }
        if (GUILayout.Button("Retrieve"))
        {
            GameUploader.GetGamePack(property_id.intValue,
                RetrieveGamePack);
        }
        if (GUILayout.Button("Delete From DB"))
        {
            GameUploader.DeleteGamePack(property_id.intValue);
            serializedObject.Update();
            property_id.intValue = -1;
            serializedObject.ApplyModifiedProperties();
        }

        GUILayout.EndHorizontal();
    }

    private void RetrieveGamePack(string json)
    {
        Debug.Log(json);
    }
}
