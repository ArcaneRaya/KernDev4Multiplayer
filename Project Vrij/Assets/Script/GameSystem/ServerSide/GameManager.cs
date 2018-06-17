using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class JSonPack
{
    public int[] ids;
    public string[] values;

    public string GetJsonOfId(int id)
    {
        for (int i = 0; i < ids.Length; i++)
        {
            if (ids[i] == id)
            {
                return values[i];
            }
        }
        throw new System.ArgumentException("ID not found in JSonPack");
    }
}

[System.Serializable]
public class IDPack
{
    public int itterator = 0;
    public int[] values;
}

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class MessagePair
    {
        public GamePack.PackIDPair pack;
        public VoiceMessage voiceMessage;
    }

    public GameEvent OnGameDataLoadedFromServer;
    public GameEvent OnPlayMessage;
    public ClientConnection clientConnection;
    //public GameInfoCollection gameInfoCollection;
    //public NetworkedGameInfo hackedGame1;
    //public NetworkedGameInfo hackedGame2;

    public ServerConnection serverConnection;
    public SessionData data;
    public int day;
    public int ignorePackIdsBelow;
    public int ignoreGameIdsBelow;

    //public List<FillerMessage> fillerMessages;
    //public List<FeedItem> feed;

    private IDPack gameIds;
    private IDPack packIds;

    private JSonPack gameJsons;
    private JSonPack packJsons;

    public bool gameStarted;
    public bool gameplayStarted;
    public bool inAudioTransmission;

    public bool isGameDataLoaded;
    public bool isGamePackDataLoaded;

    public NetworkedGames.GameInfoBase invokeTestGame;

    public MessagePair[] messagePairs;

    public MessagePairVariable currentMessagePair;

    public List<int> unblockedPacks;

    private HackManager hackManager;


    public void StartGame()
    {
        gameStarted = true;
        currentMessagePair.value = GetCurrentMessagePair();
    }

    public void StartGameplay()
    {
        BuildDebugger.Log("gameplay started");
        gameplayStarted = true;
        foreach (var connection in clientConnection.clients)
        {
            if (connection.isConnected)
            {
                connection.client.SetGameplayState(true, connection.isLoggedIn);
            }
        }
    }

    public void PlayCurrentMessage()
    {
        if (currentMessagePair.value == null)
        {
            currentMessagePair.value = GetCurrentMessagePair();
            if (currentMessagePair.value == null)
            {
                Debug.LogWarning("No message loaded");
                return;
            }
        }
        if (currentMessagePair.value.voiceMessage.triggerGame != null)
        {
            if (currentMessagePair.value.voiceMessage.triggerMoment == VoiceMessage.TriggerMoment.START)
            {
                InvokeGameOnAll(JsonUtility.ToJson(currentMessagePair.value.voiceMessage.triggerGame));
            }
            else
            {
                InvokeTransmissionBreak(true);
            }
        }
        else
        {
            InvokeTransmissionBreak(true);
        }
        if (currentMessagePair.value.voiceMessage.unBlockPackId > -1)
        {
            unblockedPacks.Add(currentMessagePair.value.voiceMessage.unBlockPackId);
            foreach (var connection in clientConnection.clients)
            {
                if (connection.isConnected)
                {
                    connection.client.gameHandler.UnblockGamePack(currentMessagePair.value.voiceMessage.unBlockPackId);
                }
            }
        }
        OnPlayMessage.Raise();
    }

    public void OnMessageDonePlaying()
    {
        InvokeTransmissionBreak(false);
        if (currentMessagePair.value.voiceMessage.triggerMoment == VoiceMessage.TriggerMoment.END)
        {
            InvokeGameOnAll(JsonUtility.ToJson(currentMessagePair.value.voiceMessage.triggerGame));
        }
        currentMessagePair.value = GetCurrentMessagePair();
    }

    public void InvokeGameOnAll(string json)
    {
        //string json = JsonUtility.ToJson(invokeTestGame);
        foreach (var connection in clientConnection.clients)
        {
            if (connection.isConnected)
            {
                connection.client.gameHandler.InvokeGame(json);
            }
        }
    }

    public void InvokeTransmissionBreak(bool state)
    {
        inAudioTransmission = state;
        foreach (var connection in clientConnection.clients)
        {
            // this is where i fixed it
            if (connection.isConnected)
            {
                connection.client.gameHandler.InvokeTransmissionBreak(state);
            }
        }
    }

    private void OnEnable()
    {
        hackManager = GetComponent<HackManager>();
        unblockedPacks = new List<int>();
        foreach (var pair in messagePairs)
        {
            if (pair.voiceMessage.day >= day)
            {
                pair.voiceMessage.played = false;
            }
            else
            {
                pair.voiceMessage.played = true;
                unblockedPacks.Add(pair.pack.id);
            }
        }
        currentMessagePair.value = new MessagePair();
        inAudioTransmission = false;
        isGameDataLoaded = false;
        isGamePackDataLoaded = false;
        clientConnection.OnClientAdded += OnClientAdded;
        Debug.Log("onenable");
        //serverConnection.GetFillerMessages(SetFillerMessages);
        GameUploader.GetAllGameDataIndexes(SetGameDataIndexes);
        GameUploader.GetAllGamePackIndexes(SetGamePackIndexes);
    }

    private void Update()
    {
        // cheat for testing / if playtest breaks
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayCurrentMessage();
        }
    }


    private void SetGamePackIndexes(string json)
    {
        packIds = JsonUtility.FromJson<IDPack>(json);
        packJsons = new JSonPack();
        packJsons.ids = packIds.values;
        packJsons.values = new string[packIds.values.Length];
        GetNextGamePack();
    }

    private void GetNextGamePack()
    {
        if (packIds.itterator < packIds.values.Length)
        {
            GameUploader.GetGamePack(packIds.values[packIds.itterator], SetGamePack);
        }
        else
        {
            isGameDataLoaded = true;
            if (isGamePackDataLoaded)
            {
                OnGameDataLoadedFromServer.Raise();
            }
        }
    }

    private void SetGamePack(string json)
    {
        packJsons.values[packIds.itterator] = json;
        packIds.itterator++;
        GetNextGamePack();
    }

    private void SetGameDataIndexes(string json)
    {
        gameIds = JsonUtility.FromJson<IDPack>(json);
        List<int> ids = new List<int>(gameIds.values);
        for (int i = ids.Count - 1; i >= 0; i--)
        {
            if (ids[i] < ignoreGameIdsBelow)
            {
                ids.RemoveAt(i);
            }
        }
        gameJsons = new JSonPack();
        gameJsons.ids = ids.ToArray();
        gameJsons.values = new string[ids.Count];
        GetNextGameData();
    }

    private void GetNextGameData()
    {
        if (gameIds.itterator < gameIds.values.Length)
        {
            GameUploader.GetGameData(gameIds.values[gameIds.itterator], SetGameData);
        }
        else
        {
            isGamePackDataLoaded = true;
            if (isGameDataLoaded)
            {
                OnGameDataLoadedFromServer.Raise();
            }
        }
    }

    private void SetGameData(string json)
    {
        gameJsons.values[gameIds.itterator] = json;
        gameIds.itterator++;
        GetNextGameData();
    }

    private void OnClientAdded(ClientConnection connection)
    {
        connection.OnReconnected += OnClientConnected;
        connection.OnGameFinished += OnClientFinishedGame;
        OnClientConnected(connection);
    }

    private void OnClientFinishedGame(ClientConnection connection)
    {
        //BuildDebugger.Log("Game Finished by " + connection.currentTeam.teamName);
        if (!HasReachedNextRequirement(connection))
        {
            hackManager.OnGameFinished();
            return;
        }
        foreach (var conn in clientConnection.clients)
        {
            if (!HasReachedNextRequirement(conn))
            {
                hackManager.OnGameFinished();
                return;
            }
        }
        PlayCurrentMessage();
    }

    private bool HasReachedNextRequirement(ClientConnection connection)
    {
        foreach (var gameIDPair in currentMessagePair.value.pack.packInfo.gameIDPairs)
        {
            if (!connection.finishedGames.Contains(gameIDPair.id))
            {
                //BuildDebugger.Log("Not reached requirement, missing " + gameIDPair.id);
                return false;
            }
        }
        return true;
    }

    private MessagePair GetCurrentMessagePair()
    {
        foreach (var message in messagePairs)
        {
            if (message.voiceMessage.day < day)
            {
                continue;
            }
            if (message.voiceMessage.day > day)
            {
                continue;
            }
            if (!message.voiceMessage.played)
            {
                return message;
            }
        }
        return null;
    }

    private void OnClientConnected(ClientConnection connection)
    {
        connection.client.LoadGames(packJsons, gameJsons, unblockedPacks);
        connection.client.gameHandler.InvokeTransmissionBreak(inAudioTransmission);
        connection.client.SetGameplayState(gameplayStarted, connection.isLoggedIn);
        //foreach (var id in unblockedPacks)
        //{
        //    connection.client.gameHandler.UnblockGamePack(id);
        //}
    }
}
