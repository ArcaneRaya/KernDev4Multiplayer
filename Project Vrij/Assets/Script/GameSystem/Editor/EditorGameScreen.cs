using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

//[CustomEditor(typeof(GameScreen), true)]
//public class EditorGameScreen : Editor
//{
//    private GameInfo simulationGameInfo;

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        EditorGUILayout.Space();
//        EditorGUILayout.LabelField("Simulate Game");
//        simulationGameInfo = EditorGUILayout.ObjectField(simulationGameInfo, typeof(GameInfo), false) as GameInfo;
//        if (GUILayout.Button("Simulate"))
//        {
//            Simulate();
//        }
//    }

//    private void Simulate()
//    {
//        serializedObject.Update();
//        serializedObject.FindProperty("currentGame").objectReferenceValue = simulationGameInfo;
//        serializedObject.FindProperty("testRun").boolValue = true;
//        serializedObject.ApplyModifiedProperties();
//        EditorUtility.SetDirty(target);
//        EditorApplication.isPlaying = true;
//    }
//}
