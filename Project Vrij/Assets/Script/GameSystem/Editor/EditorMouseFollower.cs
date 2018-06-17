using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

//[CustomEditor(typeof(MouseFollower))]
//public class EditorMouseFollower : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        EditorGUILayout.Space();
//        if (GUILayout.Button("Save"))
//        {
//            var path = EditorUtility.SaveFilePanel(
//                "Save Environment",
//                "",
//                "newEnvironment" + ".asset",
//                "asset");

//            path = "Assets" + path.Substring(Application.dataPath.Length);

//            if (String.IsNullOrEmpty(path))
//            {
//                Debug.LogWarning("no path provided, did not save");
//                return;
//            }
//            else
//            {
//                SaveEnvironment(path);
//            }
//        }
//    }

//    private void SaveEnvironment(string path)
//    {
//        IsometricEnvironment environment = ScriptableObject.CreateInstance<IsometricEnvironment>();
//        MouseFollower mouseFollower = target as MouseFollower;
//        environment.positions = new List<Vector3Int>();
//        environment.cells = new List<IsometricCell>();
//        foreach (var kvp in mouseFollower.contentLink)
//        {
//            environment.positions.Add(Vector3Int.RoundToInt(kvp.Key.transform.position));
//            environment.cells.Add(
//                new IsometricCell(kvp.Value,
//                                  kvp.Key.transform.localRotation.eulerAngles));
//        }
//        AssetDatabase.CreateAsset(environment, path);
//        AssetDatabase.Refresh();
//        Selection.activeObject = environment;
//    }
//}
