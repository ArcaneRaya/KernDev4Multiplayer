#define MATCHMAKER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    private bool isConnected;
    public GameEvent onServerStarted;
    public ClientConnection clientConnection;
    public SessionData data;
    public StringVariable generatedMatchName;
    public GameEvent onConnectionLost;
    public SceneManagement sceneManagement;
    private string myMatchName = "IrisMatch";

    //   private string generatedMatchName = "testMatchRuben";

    private void Start()
    {
#if MATCHMAKER
        StartMatchMaker();
#endif
    }

    public void TryStartServer()
    {
#if MATCHMAKER
        //generatedMatchName = data.school.schoolName + "#" + data.group.groupName;
        matchMaker.CreateMatch(myMatchName, 10, true, "", "", "", 0, 0, OnInternetMatchCreate);
#else
        StartServer();
#endif
    }

    public void TryConnectToServer()
    {

        isConnected = false;
#if MATCHMAKER
        if (matchMaker == null)
        {
            StartMatchMaker();
        }
        matchMaker.ListMatches(0, 10, myMatchName, true, 0, 0, OnInternetMatchList);
#else
        StartClient();
#endif
    }

    public override void OnStartServer()
    {
        BuildDebugger.Log("Server creation succeeded");
        onServerStarted.Raise();
    }

    public override void OnStopServer()
    {
        BuildDebugger.Log("Server stopped");
    }

    private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo responseData)
    {
        if (success)
        {
            NetworkServer.Listen(responseData, responseData.port);
            Debug.Log(responseData.port);

            var serverStarted = StartServer(responseData);
            if (!serverStarted)
            {
                BuildDebugger.Log("Server creation failed");
            }
        }
        else
        {
            BuildDebugger.Log("Could not create match on Unity Matchmaker");
        }
    }

    private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
        {
            if (matches.Count != 0)
            {
                matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
            }
            else
            {
                BuildDebugger.Log("No running matches found");
                onConnectionLost.Raise();
            }
        }
        else
        {
            BuildDebugger.Log("Couldn't connect to Unity Matchmaker");
            onConnectionLost.Raise();
        }
    }

    private void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo responseData)
    {
        if (success)
        {
            StartClient(responseData);
            BuildDebugger.Log("Joining match");
        }
        else
        {
            BuildDebugger.Log("Join match failed");
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        foreach (var connection in clientConnection.clients)
        {
            if (connection.networkConnection == conn)
            {
                connection.Disconnected();
                return;
            }
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("Client connected with IP: " + conn.address);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        isConnected = true;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        if (conn.lastError == NetworkError.Timeout && !isConnected)
        {
            BuildDebugger.Log("Couldn't connect to server, is server running?");
        }
        else
        {
            BuildDebugger.Log("Lost connection to server");
            sceneManagement.Reset();
            SceneManager.LoadScene(0);
        }
    }
}
