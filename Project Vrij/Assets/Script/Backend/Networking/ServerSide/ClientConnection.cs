using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "NewClientConnection", menuName = "Backend/Client Connection", order = 100)]
public class ClientConnection : ScriptableObject
{
    private static List<ClientConnection> Clients;

    public List<ClientConnection> clients
    {
        get
        {
            if (Clients == null)
            {
                Clients = new List<ClientConnection>();
            }
            return Clients;
        }
        set
        {
            Clients = value;
        }
    }

    // new functionality
    public delegate void ConnectionChange(ClientConnection connection);
    public ConnectionChange OnClientAdded;
    public ConnectionChange OnGameRequested;
    public ConnectionChange OnDisconnected;
    public ConnectionChange OnReconnected;
    public ConnectionChange OnGameFinished;

    public SessionData data;
    public GameEvent teamJoined;
    public ServerConnection serverConnection;
    public Team currentTeam;
    public NetworkConnection networkConnection;
    public Client client;
    public bool isConnected;
    public bool isHacked;
    public bool isLoggedIn;
    public int connectionID;
    public string address;
    public List<int> finishedGames;

    // return teams and whether team creation is allowed through setTeams action
    public void RequestTeams(Action<Teams, bool> setTeams)
    {
        bool teamCreationAllowed = true;
        setTeams(data.teams, teamCreationAllowed);
    }

    // return if team choice succeeded and if not, return reason
    public void TeamChosen(Team team, Action<bool, string> succeeded)
    {
        bool choiceSucceeded = false;
        string errorMessage = "team not found";
        // hier checkt ie het team dat de tablet binnen heeft gekregen, 
        // maar de spelsituatie is mogelijk al veranderd, moeten de global 
        // version checken op de server

        foreach (Team temp in data.teams.teams)
        {
            if (temp.teamName == team.teamName)
            {
                if (temp.chosen)
                {
                    choiceSucceeded = false;
                    errorMessage = "this team is already claimed";
                    break;
                }
                else
                {
                    currentTeam = team;
                    isLoggedIn = true;
                    choiceSucceeded = true;
                    //errorMessage = "team not found";
                    temp.chosen = true;
                    errorMessage = "success";
                    teamJoined.Raise();
                    break;
                    //foreach (Team temp in data.teams.teams)
                    //{
                    //    if (temp.teamName == team.teamName)
                    //    {
                    //        temp.chosen = true;
                    //        errorMessage = "success";
                    //        teamJoined.Raise();
                    //    }
                    //}
                }
            }
        }
        //if (team.chosen)
        //{
        //    choiceSucceeded = false;
        //    errorMessage = "this team is already claimed";
        //}
        //else
        //{
        //    currentTeam = team;
        //    isLoggedIn = true;
        //    errorMessage = "team not found";
        //    foreach (Team temp in data.teams.teams)
        //    {
        //        if (temp.teamName == team.teamName)
        //        {
        //            temp.chosen = true;
        //            errorMessage = "success";
        //            teamJoined.Raise();
        //        }
        //    }

        //}
        succeeded(choiceSucceeded, errorMessage);
    }

    // return if team creation succeeded and if not, return reason
    public void CreateTeam(Team team, Action<bool, string> succeeded)
    {
        serverConnection.CreateTeam(data.group, team.teamName, team.pass, succeeded);

        //deze word nu aan het eind van de creationg in serverconnection uitgevoerd
        //succeeded(creationSucceeded, errorMessage);
    }

    public void Logout()
    {
        foreach (Team temp in data.teams.teams)
        {
            if (temp.teamName == currentTeam.teamName)
            {
                temp.chosen = false;
                teamJoined.Raise();
            }
        }
        currentTeam = new Team();
        isLoggedIn = false;
        client.ResetClient();
    }

    public void FinishedGame(int id)
    {
        serverConnection.UpdateFinishedGame(currentTeam, id);
        finishedGames.Add(id);
        foreach (Team team in data.teams.teams)
        {
            if (team.teamID == currentTeam.teamID)
            {
                team.assignmentIndex = id;
            }
        }
        if (OnGameFinished != null)
        {
            OnGameFinished(this);
        }
    }

    public void FinishedGamePack(int id)
    {

    }

    public void SetScore(int score)
    {
        currentTeam.score = score;
        serverConnection.UpdateTeamScore(currentTeam, score, 0);
        foreach (Team team in data.teams.teams)
        {
            if (team.teamID == currentTeam.teamID)
            {
                team.score = score;
            }
        }
    }

    public ClientConnection Get(NetworkConnection clientConnection, Client client)
    {
        BuildDebugger.Log("connections available: " + clients.Count);
        foreach (var connection in clients)
        {
            if (connection.address == clientConnection.address && !connection.isConnected)
            {
                BuildDebugger.Log("reconnected client connection " + clientConnection.connectionId + " from ip: " + clientConnection.address);
                connection.networkConnection = clientConnection;
                connection.isConnected = true;
                connection.client = client;

                foreach (Team temp in data.teams.teams)
                {
                    if (temp.teamName == currentTeam.teamName)
                    {
                        if (temp.chosen == false)
                        {
                            temp.chosen = true;
                            teamJoined.Raise();
                        }
                        else
                        {
                            currentTeam = new Team();
                            isLoggedIn = false;
                        }
                        break;
                    }
                }

                if (connection.OnReconnected != null)
                {
                    connection.OnReconnected(connection);
                }
                return connection;
            }
        }
        BuildDebugger.Log("added client connection " + clientConnection.connectionId + " from ip: " + clientConnection.address);
        ClientConnection newConnection = Instantiate(this);
        newConnection.networkConnection = clientConnection;
        newConnection.address = clientConnection.address;
        newConnection.connectionID = clientConnection.connectionId;
        newConnection.isConnected = true;
        newConnection.client = client;
        newConnection.isLoggedIn = false;
        clients.Add(newConnection);
        if (OnClientAdded != null)
        {
            OnClientAdded(newConnection);
        }
        return newConnection;
    }

    public void Disconnected()
    {
        BuildDebugger.Log("lost client connection " + connectionID + " from ip: " + address);
        isConnected = false;

        foreach (Team temp in data.teams.teams)
        {
            if (temp.teamName == currentTeam.teamName)
            {
                temp.chosen = false;
                teamJoined.Raise();
            }
        }

        if (OnDisconnected != null)
        {
            OnDisconnected(this);
        }
    }

    // called when client is no longer connected to server 
    public void Remove()
    {
        BuildDebugger.Log("removed client connection for team: " + currentTeam.teamName);
        clients.Remove(this);
        Destroy(this);
    }
}