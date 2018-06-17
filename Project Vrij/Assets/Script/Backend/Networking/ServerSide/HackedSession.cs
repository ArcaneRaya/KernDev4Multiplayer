
using System;

[System.Serializable]
public class HackedSession
{
    public delegate void HackedEvent();
    public HackedEvent onFinished;
    public string gameJson;
    public ClientConnection connectionTeam1;
    public ClientConnection connectionTeam2;
    public bool hackedStarted;
    public bool settingUp;
    private bool connection1Ready;
    private bool connection2Ready;
    private int connection1Finish;
    private int connection2Finish;

    public void SetupGame()
    {
        if (string.IsNullOrEmpty(gameJson))
        {
            throw new System.ArgumentException("missing gamejson");
        }
        connectionTeam1.client.hackedHandler.onClientReady += OnConnectionReady;
        connectionTeam2.client.hackedHandler.onClientReady += OnConnectionReady;
        connectionTeam1.client.hackedHandler.onFinishReached += OnClientReachedFinish;
        connectionTeam2.client.hackedHandler.onFinishReached += OnClientReachedFinish;
        connectionTeam1.OnDisconnected += OnConnectionLost;
        connectionTeam2.OnDisconnected += OnConnectionLost;
        connection1Finish = -1;
        connection2Finish = -1;
        connectionTeam1.client.hackedHandler.HackSetup(connectionTeam2.currentTeam.teamName, 1, gameJson);
        connectionTeam2.client.hackedHandler.HackSetup(connectionTeam1.currentTeam.teamName, 2, gameJson);
    }

    private void OnClientReachedFinish(int team, int index)
    {
        if (team == 1)
        {
            connection1Finish = index;
        }
        if (team == 2)
        {
            connection2Finish = index;
        }
        if (connection1Finish > -1 && connection2Finish > -1)
        {
            if (connection1Finish == connection2Finish)
            {
                HackedFinished();
            }
        }
    }

    private void HackedFinished()
    {
        UnityEngine.Debug.Log("finished hacksession");
        QuitHackedGame();
    }

    private void OnConnectionReady(int team, int index)
    {
        UnityEngine.Debug.Log("connection ready " + team);
        if (team == 1)
        {
            connection1Ready = true;
        }
        if (team == 2)
        {
            connection2Ready = true;
        }
        if (connection1Ready && connection2Ready)
        {
            UnityEngine.Debug.Log("session should start");
            hackedStarted = true;
            connectionTeam1.client.hackedHandler.StartHackGame();
            connectionTeam2.client.hackedHandler.StartHackGame();
        }
    }

    private void OnConnectionLost(ClientConnection connection)
    {
        QuitHackedGame();
    }

    public void QuitHackedGame()
    {
        if (hackedStarted)
        {
            if (connectionTeam1 != null)
            {
                if (connectionTeam1.isConnected)
                {
                    connectionTeam1.client.hackedHandler.Quit();
                }
            }
            if (connectionTeam2 != null)
            {
                if (connectionTeam2.isConnected)
                {
                    connectionTeam2.client.hackedHandler.Quit();
                }
            }
        }
        if (connectionTeam1 != null)
        {
            connectionTeam1.isHacked = false;
        }
        if (connectionTeam2 != null)
        {
            connectionTeam2.isHacked = false;
        }
        if (onFinished != null)
        {
            onFinished();
        }
    }
}