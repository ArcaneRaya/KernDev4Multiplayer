using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor;

//[CustomPropertyDrawer(typeof(NetworkedGameInfo))]
//public class NetworkedGameInfoPropertyDrawer : PropertyDrawer
//{
//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return EditorGUIUtility.singleLineHeight * 2.3f;

//    }
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        //base.OnGUI(position, property, label);
//        position.height -= EditorGUIUtility.singleLineHeight * 1.3f;
//        EditorGUI.BeginChangeCheck();
//        SerializedProperty gameInfoProperty = property.FindPropertyRelative("gameInfo");
//        SerializedProperty queryProperty = property.FindPropertyRelative("resourceQuery");
//        EditorGUI.ObjectField(position, gameInfoProperty);
//        if (EditorGUI.EndChangeCheck())
//        {
//            if (gameInfoProperty.objectReferenceValue == null)
//            {
//                queryProperty.stringValue = string.Empty;
//            }
//            else
//            {
//                queryProperty.stringValue = AssetDatabase.GetAssetPath(gameInfoProperty.objectReferenceValue).Replace("Assets/Resources/", "").Replace(".asset", "");
//            }
//        }
//        position.y += EditorGUIUtility.singleLineHeight;
//        GUI.enabled = false;
//        EditorGUI.PropertyField(position, queryProperty, GUIContent.none);
//        GUI.enabled = true;
//    }
//}

//#endif

//[System.Serializable]
//public class NetworkedGameInfo
//{
//#if UNITY_EDITOR
//    public GameInfo gameInfo;
//#endif
//    public string resourceQuery;
//}
