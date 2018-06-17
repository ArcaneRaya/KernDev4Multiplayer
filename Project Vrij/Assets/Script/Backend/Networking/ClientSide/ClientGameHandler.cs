using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NetworkedGames;
using System;

public class ClientGameHandler : NetworkBehaviour
{
    public Client client;

    public void OnContinue()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdOnFinishedGame(client.clientState.currentGame.id);
        if (!client.clientState.finishedGames.Contains(client.clientState.currentGame.id))
        {
            client.clientState.finishedGames.Add(client.clientState.currentGame.id);
            client.AddScore(client.clientState.currentGame.completionScore);
        }
        else
        {
            if (client.clientState.moneyMaker)
            {
                client.AddScore(client.clientState.currentGame.completionScore);
            }
        }
        client.clientState.moneyMaker = false;
        client.sceneManagement.LoadMenuOverlay();
    }

    public void InvokeGame(string json)
    {
        RpcInvokeGame(json);
    }

    public void InvokeTransmissionBreak(bool state)
    {
        RpcInvokeTransmissionBreak(state);
    }

    public void UnblockGamePack(int id)
    {
        RpcUnblockGamePack(id);
    }

    [ClientRpc]
    private void RpcUnblockGamePack(int id)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        foreach (var gamePack in client.clientState.gamePacks)
        {
            if (gamePack.id == id)
            {
                gamePack.blocked = false;
                Debug.Log("unblocked " + gamePack.id);
                client.sceneManagement.ReloadScenes();
                return;
            }
        }
        Debug.Log("no pack found to unblock for " + id);
    }

    //public void InvokeGame(NetworkedGameInfo gameInfo)
    //{
    //    RpcInvokeGame(gameInfo.resourceQuery);
    //}

    [Command]
    private void CmdOnFinishedGame(int id)
    {
        client.Connection.FinishedGame(id);
    }

    [ClientRpc]
    private void RpcInvokeTransmissionBreak(bool state)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        client.sceneManagement.LoadTransmissionOverlay(state);
    }

    [ClientRpc]
    private void RpcInvokeGame(string json)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        GameInfoBase gameToInvoke = GameInfoBase.FromJson(json);
        Debug.Log((gameToInvoke as GameInfoExtended).title);
        client.clientState.currentGame = (gameToInvoke as GameInfoExtended);
        client.sceneManagement.LoadGameScreen(gameToInvoke.gameType);
    }

    //[Command]
    //private void CmdRequestNextGame()
    //{
    //    client.Connection.RequestNextGame();
    //}

    //[ClientRpc]
    //private void RpcInvokeGame(string gameInfo)
    //{
    //    if (!isLocalPlayer)
    //    {
    //        return;
    //    }
    //    BuildDebugger.Log("Invoked " + gameInfo);
    //    GameInfo newGame = Resources.Load(gameInfo, typeof(GameInfo)) as GameInfo;
    //    BuildDebugger.Log("Loaded " + newGame.name);
    //    client.clientState.currentGame = newGame;
    //    client.sceneManagement.LoadGameScreen(newGame.Type);
    //}
}
