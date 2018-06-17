using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LoginMaster : MonoBehaviour
{
    public ServerConnection serverConnection;
    //    public LoadScreen loadScreen;
    //    public StringReference schoolName;
    public SessionData data;
    public StringVariable generatedMatchName;

    [Header("Location")]
    public StringReference chosenLocation;
    //    public UIDisplay locationScreen;
    public StringListVariable locationList;
    public GameEvent onDisplayLocationPicker;

    [Header("School")]
    public StringReference chosenSchool;
    //    public UIDisplay schoolLogin;
    public StringListVariable schoolList;
    public GameEvent onDisplaySchoolPicker;

    [Header("Groups")]
    public StringReference chosenGroup;
    //    public UIDisplay groupLogin;
    public StringListVariable groupList;
    public GameEvent onDisplayGroupPicker;

    [Header("Teams")]
    public GameEvent onTeamsReceived;

    [Header("ClientSide")]
    public bool clientLogin;
    public GameEvent onMatchNameGenerated;

    private List<Location> locations;
    private List<School> schools;
    private List<Group> groups;
    //    private List<Team> teams;

    private const string NEW_ITEM = "Maak nieuw";

    private void Start()
    {
        if (clientLogin)
        {
            return;
        }
        //locationScreen.DisplayImmediate(true);
        //schoolLogin.DisplayImmediate(false);
        data.Reset();
        //    RequestLocations();
    }

    public void RequestLocations()
    {
        serverConnection.GetLocations(SetLocations);
    }

    public void OnLocationSet()
    {
        serverConnection.GetSchools(SetSchools, chosenLocation.Value);
    }

    public void OnSchoolSet()
    {
        Debug.Log("received");
        foreach (var school in schools)
        {
            if (school.schoolName == chosenSchool.Value)
            {
                serverConnection.GetGroups(SetGroups, school);
                data.school = school;
                return;
            }
        }
        throw new ArgumentException("selected school was not loaded in the first place. whuuut");
    }

    public void OnGroupSet()
    {
        if (chosenGroup.Value == NEW_ITEM)
        {
            Debug.Log("make new group");
        }
        foreach (var group in groups)
        {
            if (group.groupName == chosenGroup.Value)
            {
                data.group = group;
                generatedMatchName.value = (data.school.schoolName + "#" + data.group.groupName).Replace("_", "").Replace(" ", "");
                if (clientLogin)
                {
                    PlayerPrefs.SetString(ClientSetup.matchNameKey, generatedMatchName.value);
                    onMatchNameGenerated.Raise();
                }
                else
                {
                    RequestTeams();
                }
                return;
            }
        }
        throw new ArgumentException("selected group was not loaded in the first place. whuuut");
    }

    public void RequestTeams()
    {
        serverConnection.GetTeams(SetTeams, data.group);
    }

    private void SetLocations(List<Location> receivedLocations)
    {
        locations = receivedLocations;
        locationList.Value = new List<string>();
        foreach (var item in locations)
        {
            locationList.Value.Add(item.location);
        }
        onDisplayLocationPicker.Raise();
    }

    private void SetSchools(List<School> receivedSchools)
    {
        schools = receivedSchools;
        schoolList.Value = new List<string>();
        foreach (var item in schools)
        {
            Debug.Log(item.schoolName);
            schoolList.Value.Add(item.schoolName);
        }
        onDisplaySchoolPicker.Raise();
    }

    private void SetGroups(List<Group> receivedGroups)
    {
        groups = receivedGroups;
        groupList.Value = new List<string>();
        foreach (var item in groups)
        {
            Debug.Log(item.groupName);
            groupList.Value.Add(item.groupName);
        }
        groupList.Value.Add(NEW_ITEM);
        onDisplayGroupPicker.Raise();
    }

    private void SetTeams(List<Team> receivedTeams)
    {
        //teams = receivedTeams;
        Teams teams = new Teams();
        teams.teams = receivedTeams.ToArray();
        if (data.teams == null)
        {
            data.teams = teams;
        }
        else if (data.teams.teams == null)
        {
            data.teams = teams;
        }
        else if (data.teams.teams.Length == 0)
        {
            data.teams = teams;
        }
        else
        {
            foreach (var team in teams.teams)
            {
                foreach (var dataTeam in data.teams.teams)
                {
                    if (team.teamID == dataTeam.teamID)
                    {
                        team.chosen = dataTeam.chosen;
                        break;
                    }
                }
            }
            data.teams = teams;
        }
        onTeamsReceived.Raise();
    }
}
