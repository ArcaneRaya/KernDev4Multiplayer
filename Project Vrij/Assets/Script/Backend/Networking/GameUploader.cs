using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

[ExecuteInEditMode]
public static class GameUploader
{

    public static void UpdateGameData(NetworkedGames.GameInfoBase info)
    {
        ServerConnection con = Resources.Load("ServerConnection") as ServerConnection;
        CoroutineSupport.CoroutineComponent.StartCoroutine(con.RequestGameRecordUpdate(info));
    }

    public static void UpdateGamePack(GamePack pack)
    {
        ServerConnection con = Resources.Load("ServerConnection") as ServerConnection;
        CoroutineSupport.CoroutineComponent.StartCoroutine(con.RequestGamePackUpdate(pack));
    }

    public static void GetGameData(int id, Action<string> setAction)
    {
        ServerConnection con = Resources.Load("ServerConnection") as ServerConnection;
        CoroutineSupport.CoroutineComponent.StartCoroutine(con.RequestGameData(id, setAction));
    }

    public static void GetGamePack(int id, Action<string> setAction)
    {
        ServerConnection con = Resources.Load("ServerConnection") as ServerConnection;
        CoroutineSupport.CoroutineComponent.StartCoroutine(con.RequestGamePack(id, setAction));
    }

    public static void GetAllGameDataIndexes(Action<string> setAction)
    {
        ServerConnection con = Resources.Load("ServerConnection") as ServerConnection;
        CoroutineSupport.CoroutineComponent.StartCoroutine(con.RequestAllGameDataIndexes(setAction));
    }

    public static void DeleteGameData(int id)
    {
        ServerConnection con = Resources.Load("ServerConnection") as ServerConnection;
        CoroutineSupport.CoroutineComponent.StartCoroutine(con.RequestDeleteGameData(id));
    }

    public static void GetAllGamePackIndexes(Action<string> setAction)
    {
        ServerConnection con = Resources.Load("ServerConnection") as ServerConnection;
        CoroutineSupport.CoroutineComponent.StartCoroutine(con.RequestAllGamePackIndexes(setAction));
    }

    public static void DeleteGamePack(int id)
    {
        ServerConnection con = Resources.Load("ServerConnection") as ServerConnection;
        CoroutineSupport.CoroutineComponent.StartCoroutine(con.RequestDeleteGamePack(id));
    }
}


