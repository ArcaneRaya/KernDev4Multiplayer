using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[System.Serializable]
public class ClientLogin : NetworkBehaviour
{
    public Client client;
    public StringListVariable teamList;
    public StringVariable chosenTeam;
    public GameEvent onTeamsReceived;
    public GameEvent onTeamSet;
    public GameEvent onDisplayTeamCreation;

    private Teams availableTeams;

    public void OnDisplayLogin()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Debug.Log("requesting teams");
        CmdRequestTeams();
    }

    public void OnTeamChosen()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        foreach (var team in availableTeams.teams)
        {
            if (team.teamName == chosenTeam.value)
            {
                client.teamInfo = team;
                CmdTeamChosen(team);
                return;
            }
        }
    }

    public void OnCreateTeam()
    {
        Team newTeam = new Team();
        newTeam.teamName = chosenTeam.value;
        //newTeam.
        CmdOnCreateTeam(newTeam);
    }

    public void OnLogout()
    {
        CmdOnLogout();
    }

    private void SetTeams(Teams teams, bool newTeamAllowed)
    {
        RpcSetTeams(teams, newTeamAllowed);
    }

    private void TeamChoiceSucceeded(bool succes, string errorMessage)
    {
        Debug.Log("TeamChoice: " + (succes ? "succeeded" : "failed") + " | " + errorMessage);
        onTeamSet.Raise();
        RpcTeamChoiceSucceeded(succes);
    }

    private void TeamCreationSucceeded(bool succes, string errorMessage)
    {
        BuildDebugger.Log("TeamCreation: " + (succes ? "succeeded" : "failed") + " | " + errorMessage);
        RpcTeamCreationSucceeded(succes);
    }

    [Command]
    private void CmdOnLogout()
    {
        client.Connection.Logout();
    }

    [Command]
    private void CmdOnCreateTeam(Team team)
    {
        client.Connection.CreateTeam(team, TeamCreationSucceeded);
    }

    [Command]
    private void CmdRequestTeams()
    {
        client.Connection.RequestTeams(SetTeams);
    }

    [Command]
    private void CmdTeamChosen(Team team)
    {
        Debug.Log("choice received");
        client.Connection.TeamChosen(team, TeamChoiceSucceeded);
    }

    [ClientRpc]
    private void RpcSetTeams(Teams teams, bool newTeamAllowed)
    {
        Debug.Log("teams received");
        if (!isLocalPlayer)
        {
            return;
        }
        availableTeams = teams;
        teamList.Value = new List<string>();
        foreach (var team in teams.teams)
        {
            teamList.Value.Add(team.teamName);
        }
        onTeamsReceived.Raise();
    }

    [ClientRpc]
    private void RpcTeamChoiceSucceeded(bool succes)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Debug.Log("choice confirmation received");
        if (!succes)
        {
            CmdRequestTeams();
        }
        else
        {
            OnTeamSet();
        }
    }

    [ClientRpc]
    private void RpcTeamCreationSucceeded(bool succes)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Debug.Log("creation confirmation received");
        if (!succes)
        {
            onDisplayTeamCreation.Raise();
        }
        else
        {
            StartCoroutine(WaitForRequest());
        }
    }

    private void OnTeamSet()
    {
        onTeamSet.Raise();
    }

    private IEnumerator WaitForRequest()
    {
        yield return new WaitForSeconds(2);
        CmdRequestTeams();
    }
}
