using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NetworkedGames;

public class ClientHacked : NetworkBehaviour
{
    public delegate void HackedEvent(int team, int index);
    public HackedEvent onClientReady;
    public HackedEvent onFinishReached;
    public IntVariable reachedFinish;
    public StringVariable opponentName;
    public Client client;

    private int team;
    private string hackedGameJson;
    //private string opponentName;

    public void HackSetup(string opponent, int team, string gameJson)
    {
        //hackedGameJson = gameJson;
        RpcHackSetup(opponent, team, gameJson);
    }

    public void StartHackGame()
    {
        RpcStartHacked();
    }

    public void OnFinishReached()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdFinishReached(team, reachedFinish.value);
    }

    public void Quit()
    {
        RpcQuit();
    }

    [Command]
    private void CmdFinishReached(int teamIndex, int index)
    {
        if (onFinishReached != null)
        {
            onFinishReached(teamIndex, index);
        }
    }

    [Command]
    private void CmdHackPrepared(int teamIndex)
    {
        if (onClientReady != null)
        {
            onClientReady(teamIndex, -1);
        }
    }

    [ClientRpc]
    private void RpcHackSetup(string opponent, int teamIndex, string gameJson)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        this.team = teamIndex;
        opponentName.value = opponent;
        Debug.Log(gameJson);
        hackedGameJson = gameJson;
        CmdHackPrepared(teamIndex);
    }

    [ClientRpc]
    private void RpcStartHacked()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Debug.Log(hackedGameJson);
        GameInfoBase gameToInvoke = GameInfoBase.FromJson(hackedGameJson);
        Debug.Log((gameToInvoke as GameInfoExtended).title);
        client.clientState.currentGame = (gameToInvoke as GameInfoExtended);
        client.clientState.moneyMaker = true;
        client.sceneManagement.LoadGameScreen(gameToInvoke.gameType);
    }

    [ClientRpc]
    private void RpcQuit()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        team = 0;
        client.gameHandler.OnContinue();
    }
}
