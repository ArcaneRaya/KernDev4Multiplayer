using UnityEngine;
using UnityEditor;
using System;

public class DatabaseTools
{
    [MenuItem("Tools/Game Database/Print Database Content")]
    static void PrintDBContent()
    {
        GameUploader.GetAllGamePackIndexes(PrintPackCount);
        GameUploader.GetAllGameDataIndexes(PrintGameCount);
    }

    [MenuItem("Tools/Game Database/Remove Database Content")]
    static void RemoveDBContent()
    {
        GameUploader.GetAllGamePackIndexes(RemovePacks);
        GameUploader.GetAllGameDataIndexes(RemoveGames);
    }

    private static void RemoveGames(string json)
    {
        IDPack games = JsonUtility.FromJson<IDPack>(json);
        foreach (var id in games.values)
        {
            GameUploader.DeleteGameData(id);
        }
    }

    private static void RemovePacks(string json)
    {
        IDPack gamepacks = JsonUtility.FromJson<IDPack>(json);
        foreach (var id in gamepacks.values)
        {
            GameUploader.DeleteGamePack(id);
        }
    }

    private static void PrintGameCount(string json)
    {
        IDPack games = JsonUtility.FromJson<IDPack>(json);
        Debug.Log("Game Count: " + games.values.Length);
    }

    private static void PrintPackCount(string json)
    {
        IDPack gamepacks = JsonUtility.FromJson<IDPack>(json);
        Debug.Log("Pack Count: " + gamepacks.values.Length);
        Debug.Log(gamepacks.values[0]);
    }
}
