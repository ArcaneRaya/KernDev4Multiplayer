using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(GameEvent))]
public class PropertyDrawerGameEvent : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Calculate rects
        int raiseRectWidth = 60;
        int offset = 5;
        Rect eventRect = new Rect(position.x, position.y, position.width - raiseRectWidth - offset, position.height);
        Rect raiseRect = new Rect(position.x + position.width - raiseRectWidth, position.y, raiseRectWidth, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(eventRect, property, GUIContent.none);
        if (GUI.Button(raiseRect, "Raise"))
        {
            GameEvent gameEvent = property.objectReferenceValue as GameEvent;
            gameEvent.Raise();
        }

        EditorGUI.EndProperty();
    }
}
