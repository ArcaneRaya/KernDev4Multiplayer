using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class EditorGameEvent : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Raise Event"))
        {
            GameEvent gameEvent = target as GameEvent;
            gameEvent.Raise();
        }
    }
}
