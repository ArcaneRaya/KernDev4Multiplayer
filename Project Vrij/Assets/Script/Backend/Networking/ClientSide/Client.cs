using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NetworkedGames;

public class Client : NetworkBehaviour
{
    public Team TeamInfo
    {
        get
        {
            return teamInfo;
        }
    }

    public ClientConnection Connection
    {
        get
        {
            return clientConnection;
        }
    }

    public GameEvent onDisplayLogin;
    public GameEvent onLoginCompleted;

    public ClientGameHandler gameHandler;
    public ClientHacked hackedHandler;
    public SceneManagement sceneManagement;
    public ClientConnection clientConnectionBase;
    public ClientState clientState;
    public Team teamInfo;

    public StringVariable teamName;
    public IntVariable teamScore;

    private ClientConnection clientConnection;

    private JSonPack tempPacks;
    private JSonPack tempGames;
    private List<int> tempUnblockedPacks;
    private int getGameItterator = 0;
    private int getPackItterator = 0;
    private bool gamesLoaded;

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStartLocalPlayer()
    {
        gamesLoaded = false;
        base.OnStartLocalPlayer();
        BuildDebugger.Log("Joined match");
        if (isLocalPlayer)
        {
            CmdGetConnection(Network.player.ipAddress);
        }
    }

    public void UpdateLocalTeamInfo()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdUpdateLocalTeamInfo();
    }

    public void AddScore(int score)
    {
        teamInfo.score += score;
        teamScore.value = teamInfo.score;
        CmdSetScore(teamInfo.score);
    }

    public void LoadGames(JSonPack packs, JSonPack games, List<int> unblockedPacks)
    {
        tempPacks = packs;
        tempGames = games;
        tempUnblockedPacks = unblockedPacks;
        RpcSetPacks(tempPacks.ids.Length);
        //RpcLoadGames(packs, games, unblockedPacks.ToArray());
    }

    public void ResetClient()
    {
        RpcResetClient();
    }

    public void OnConnectionLost()
    {
        clientState.Reset();
        sceneManagement.UnloadAll();
    }

    [Command]
    private void CmdGetGames()
    {
        RpcSetGames(tempGames.ids.Length);
    }

    [Command]
    private void CmdTriggerLoadGames()
    {
        RpcLoadGames(tempUnblockedPacks.ToArray());
    }

    [ClientRpc]
    private void RpcResetClient()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        sceneManagement.UnloadAll();
        CmdGetLoginState();
    }

    [ClientRpc]
    private void RpcSetPacks(int size)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        getPackItterator = 0;
        tempPacks = new JSonPack();
        tempPacks.ids = new int[size];
        tempPacks.values = new string[size];
        CmdGetPack(getPackItterator);
        //CmdGetGames();
    }

    [Command]
    private void CmdGetPack(int itterator)
    {
        RpcSetPack(tempPacks.ids[itterator], tempPacks.values[itterator]);
    }

    [ClientRpc]
    private void RpcSetPack(int v1, string v2)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        tempPacks.ids[getPackItterator] = v1;
        tempPacks.values[getPackItterator] = v2;
        getPackItterator++;
        if (getPackItterator < tempPacks.ids.Length)
        {
            CmdGetPack(getPackItterator);
        }
        else
        {
            CmdGetGames();
        }
        //throw new NotImplementedException();
    }

    [ClientRpc]
    private void RpcSetGames(int size)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        getGameItterator = 0;
        tempGames = new JSonPack();
        tempGames.ids = new int[size];
        tempGames.values = new string[size];
        CmdGetGame(getGameItterator);
        //CmdTriggerLoadGames();
    }

    [Command]
    private void CmdGetGame(int itterator)
    {
        RpcSetGame(tempGames.ids[itterator], tempGames.values[itterator]);
    }

    [ClientRpc]
    private void RpcSetGame(int v1, string v2)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        tempGames.ids[getGameItterator] = v1;
        tempGames.values[getGameItterator] = v2;
        getGameItterator++;
        if (getGameItterator < tempGames.ids.Length)
        {
            CmdGetGame(getGameItterator);
        }
        else
        {
            CmdTriggerLoadGames();
        }
    }

    [ClientRpc]
    private void RpcLoadGames(int[] unblockedPacks)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        UnPackGames(tempPacks, tempGames, unblockedPacks);
    }

    private void UnPackGames(JSonPack packs, JSonPack games, int[] unblockedPacks)
    {
        List<int> unblocked = new List<int>(unblockedPacks);
        clientState.gamePacks = new List<GamePack>();
        foreach (var gamePackJson in packs.values)
        {
            GamePack gamePack = ScriptableObject.CreateInstance<GamePack>();
            JsonUtility.FromJsonOverwrite(gamePackJson, gamePack);
            foreach (var gameIDPair in gamePack.gameIDPairs)
            {
                gameIDPair.gameInfo = GameInfoBase.FromJson(games.GetJsonOfId(gameIDPair.id));
            }
            gamePack.blocked = gamePack.startBlocked;
            if (unblocked.Contains(gamePack.id))
            {
                gamePack.blocked = false;
            }
            clientState.gamePacks.Add(gamePack);
            Debug.Log("GamePack " + clientState.gamePacks[clientState.gamePacks.Count - 1].id);
        }
        gamesLoaded = true;
        CmdGetLoginState();
    }

    [Command]
    private void CmdGetLoginState()
    {
        RpcPassLoginState(clientConnection.isLoggedIn);
    }

    [Command]
    private void CmdSetScore(int score)
    {
        clientConnection.SetScore(score);
    }

    [Command]
    public void CmdGetConnection(string adress)
    {
        //Debug.Log("Player IP posted by player: " + adress);
        NetworkConnection conn = connectionToClient;
        conn.address = adress;
        clientConnection = clientConnectionBase.Get(conn, this);
    }

    [Command]
    private void CmdUpdateLocalTeamInfo()
    {
        clientConnection.serverConnection.GetFinishedGames(clientConnection.currentTeam, SetFinishedGames);
        //RpcSetLocalTeamInfo(clientConnection.currentTeam);
    }

    private void SetFinishedGames(string finishedGamesJson)
    {
        IDPack finishedGames = JsonUtility.FromJson<IDPack>(finishedGamesJson);
        clientConnection.finishedGames = new List<int>(finishedGames.values);
        RpcSetLocalTeamInfo(clientConnection.currentTeam, finishedGames);
    }

    [ClientRpc]
    public void RpcPassLoginState(bool loggedIn)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (!loggedIn)
        {
            BuildDebugger.Log("Alright, time to login");
            onDisplayLogin.Raise();
        }
        else
        {
            BuildDebugger.Log("Don't need to login, already logged");
            UpdateLocalTeamInfo();
        }
    }

    [ClientRpc]
    private void RpcSetLocalTeamInfo(Team team, IDPack finishedGames)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        teamInfo = team;
        teamName.value = teamInfo.teamName;
        teamScore.value = teamInfo.score;
        clientState.finishedGames = new List<int>(finishedGames.values);
        onLoginCompleted.Raise();
    }

    [ClientRpc]
    private void RpcSetLocalTeamInfoRuntime(Team team)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        teamInfo = team;
        teamName.value = teamInfo.teamName;
        teamScore.value = teamInfo.score;
    }

    public void SetGameplayState(bool state, bool isLoggedIn)
    {
        RpcSetGameplayState(state, isLoggedIn);
    }

    [ClientRpc]
    private void RpcSetGameplayState(bool state, bool isLoggedIn)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        BuildDebugger.Log("gameplay state set to " + state);
        clientState.gameplayStarted = state;
        if (isLoggedIn && gamesLoaded)
        {
            sceneManagement.ReloadScenes();
        }
    }
}
