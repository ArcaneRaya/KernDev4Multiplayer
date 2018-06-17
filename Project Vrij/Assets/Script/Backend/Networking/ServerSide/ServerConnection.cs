using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "NewServerConnection", menuName = "Backend/Server Connection", order = 100)]
public class ServerConnection : ScriptableObject
{
    [Header("Events Login Menu")]
    public GameEvent onLoading;
    public GameEvent onTeamsChanged;

    [SerializeField]
    private List<School> schools;
    [SerializeField]
    private List<Team> teams;
    [SerializeField]
    private GlobalEvent currentEvent;
    private const string baseURL = "http://studenthome.hku.nl/~yassine.minjon/IrisV2/IrisV2SQL";


    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public void GetLocations(Action<List<Location>> setAction)
    {
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestLocations(setAction));
    }

    public void GetSchools(Action<List<School>> setAction, string location)
    {
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestSchools(setAction, location));
    }

    public void GetGroups(Action<List<Group>> setAction, School school)
    {
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestGroups(setAction, school));
    }

    public void GetTeams(Action<List<Team>> setAction, Group group)
    {
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestTeams(setAction, group));
    }

    public void GetFillerMessages(Action<List<FillerMessage>> setAction)
    {
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestFillerMessages(setAction));
    }

    public void GetSessionData(SessionData data, int groupID)
    {
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestSessionData(data, groupID));
    }

    public GlobalEvent GetGlobalEvent()
    {
        throw new System.NotImplementedException();
    }

    public void LoginSchool(School school, string pass)
    {
        throw new System.NotImplementedException();
    }

    public void LoginTeam(Team team, string pass)
    {
        throw new System.NotImplementedException();
    }

    public void CreateTeam(Group group, string teamName, string pass, Action<bool, string> succeeded)
    {
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestTeamCreation(group, teamName, pass, succeeded));
    }

    public void UpdateTeamScore(Team team, int score, int assignmentIndex)
    {
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestScoreUpdate(team, score, assignmentIndex));
    }

    public void UpdateSessionData(SessionData data)
    {
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestSessionDataUpdate(data));
    }

    public void UpdateFinishedGame(Team team, int id)
    {
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestFinishedGameUpdate(team, id));
    }

    public void GetFinishedGames(Team team, Action<string> setAction)
    {
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestFinishedGames(team, setAction));
    }

    private IEnumerator UpdateConnection(float timeToNextPing)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator RequestLocations(Action<List<Location>> setAction)
    {
        #region www handling
        WWW www = new WWW(baseURL + "LocationFinder.php");
        onLoading.Raise();
        yield return www;
        #endregion

        // check for errors
        if (checkError(www))
        {
            #region json handling
            Locations locationArray = new Locations();
            List<Location> answer = new List<Location>();
            locationArray = JsonUtility.FromJson(www.text, typeof(Locations)) as Locations;
            foreach (var item in locationArray.locations)
            {
                answer.Add(item);
            }
            #endregion
            setAction(answer);

        }
    }

    public IEnumerator RequestSchools(Action<List<School>> setAction, string location)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("schoolLocation", location);
        WWW www = new WWW(baseURL + "SchoolFinder.php", form);
        onLoading.Raise();
        yield return www;
        #endregion

        // check for errors
        if (checkError(www))
        {
            #region json handling
            Schools schoolArray = new Schools();
            List<School> answer = new List<School>();
            schoolArray = JsonUtility.FromJson(www.text, typeof(Schools)) as Schools;
            foreach (var item in schoolArray.schools)
            {
                answer.Add(item);
            }
            #endregion
            setAction(answer);

        }
    }

    public IEnumerator RequestGroups(Action<List<Group>> setAction, School school)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("schoolID", school.schoolID);
        WWW www = new WWW(baseURL + "GroupFinder.php", form);
        onLoading.Raise();
        yield return www;
        #endregion

        // check for errors
        if (checkError(www))
        {
            #region json handling
            Groups groupArray = new Groups();
            List<Group> answer = new List<Group>();
            groupArray = JsonUtility.FromJson(www.text, typeof(Groups)) as Groups;
            foreach (var item in groupArray.groups)
            {
                answer.Add(item);
            }
            #endregion
            setAction(answer);
        }
    }

    public IEnumerator RequestTeams(Action<List<Team>> setAction, Group group)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("groupID", group.groupID);
        WWW www = new WWW(baseURL + "TeamFinder.php", form);
        onLoading.Raise();
        yield return www;
        #endregion

        // check for errors
        if (checkError(www))
        {
            #region json handling
            Teams teamArray = new Teams();
            List<Team> answer = new List<Team>();
            teamArray = JsonUtility.FromJson(www.text, typeof(Teams)) as Teams;
            foreach (var item in teamArray.teams)
            {
                answer.Add(item);
            }
            #endregion
            setAction(answer);
        }
    }

    public IEnumerator RequestFillerMessages(Action<List<FillerMessage>> setAction)
    {
        #region www handling
        WWW www = new WWW(baseURL + "FillerMessageFinder.php");
        onLoading.Raise();
        yield return www;
        #endregion

        // check for errors
        if (checkError(www))
        {
            #region json handling
            FillerMessages fillerMessageArray = new FillerMessages();
            List<FillerMessage> answer = new List<FillerMessage>();
            fillerMessageArray = JsonUtility.FromJson(www.text, typeof(FillerMessages)) as FillerMessages;
            foreach (var item in fillerMessageArray.fillerMessages)
            {
                answer.Add(item);
            }
            #endregion
            CoroutineSupport.CoroutineComponent.StartCoroutine(RequestFillerMessagesImages(setAction, answer));
        }
    }

    public IEnumerator RequestFillerMessagesImages(Action<List<FillerMessage>> setAction, List<FillerMessage> answer)
    {
        Debug.Log(Time.time);
        foreach (FillerMessage message in answer)
        {
            #region www handling
            WWW www = new WWW("http://studenthome.hku.nl/~yassine.minjon/IrisV2/" + message.imageUrl.Replace('\\', ' '));
            yield return www;
            #endregion
            message.image = www.texture;
        }
        Debug.Log("done");
        Debug.Log(Time.time);
        setAction(answer);
    }

    public IEnumerator RequestSessionData(SessionData data, int groupID)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("groupID", groupID);
        WWW www = new WWW(baseURL + "SessionDataFinder.php", form);
        onLoading.Raise();
        yield return www;
        #endregion

        // check for errors
        if (checkError(www))
        {
            #region json handling
            JsonUtility.FromJsonOverwrite(www.text, data);
            #endregion
        }
    }

    public IEnumerator RequestTeamCreation(Group group, string teamName, string pass, Action<bool, string> succeeded)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("groupID", group.groupID);
        form.AddField("teamName", teamName);
        form.AddField("pass", pass);
        WWW www = new WWW(baseURL + "TeamAdder.php", form);
        onLoading.Raise();
        yield return www;
        #endregion

        // check for errors
        if (checkError(www))
        {
            if (www.text == "success")
            {
                succeeded(true, "success");
                onTeamsChanged.Raise();
            }
            else
            {
                succeeded(false, www.text);
            }
        }
    }

    public IEnumerator RequestScoreUpdate(Team team, int score, int assignmentIndex)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("teamID", team.teamID);
        form.AddField("score", score);
        form.AddField("puzzelID", assignmentIndex);
        WWW www = new WWW(baseURL + "TeamScoreUpdater.php", form);
        //onLoading.Raise();
        yield return www;
        #endregion

        // check for errors
        if (checkError(www))
        {
            if (www.text == "success")
            {
                // score geupdate
            }
            else
            {
                // score niet geupdate
            }
        }
    }

    public IEnumerator RequestSessionDataUpdate(SessionData data)
    {
        string json = JsonUtility.ToJson(data);
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("sessionData", json);
        form.AddField("groupID", data.group.groupID);
        WWW www = new WWW(baseURL + "SessionDataAdder.php", form);
        yield return www;
        #endregion

        // check for errors
        if (checkError(www))
        {
            if (www.text == "success")
            {
                // score geupdate
                Debug.Log("all is well");
            }
            else
            {
                // score niet geupdate
            }
        }
        CoroutineSupport.CoroutineComponent.StartCoroutine(RegularSessionDataUpdate(data));

    }

    public IEnumerator RequestFinishedGameUpdate(Team team, int id)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("teamID", team.teamID);
        form.AddField("gameID", id);
        WWW www = new WWW(baseURL + "FinishedGameAdder.php", form);
        //onLoading.Raise();
        yield return www;
        #endregion

        // check for errors
        if (checkError(www))
        {
            if (www.text == "success")
            {
                // score geupdate
                Debug.Log("all is well");
            }
            else
            {
                // score niet geupdate
            }
        }
    }

    public IEnumerator RequestFinishedGames(Team team, Action<string> setAction)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("teamID", team.teamID);
        WWW www = new WWW(baseURL + "FinishedGamesFinder.php", form);
        //onLoading.Raise();
        yield return www;
        #endregion

        // check for errors
        if (checkError(www))
        {
            if (www.text == "success")
            {
                // score geupdate
                Debug.Log("all is well");
            }
            else
            {
                // score niet geupdate
            }
        }
        setAction(www.text);
    }

    public IEnumerator RequestGameRecordUpdate(NetworkedGames.GameInfoBase info)
    {
        if (info.id == -1)
        {
            WWWForm form = new WWWForm();
            WWW www = new WWW(baseURL + "GameDataIndexer.php", form);
            yield return www;

            if (checkError(www))
            {
                Debug.Log(www.text);
                info.id = Int32.Parse(www.text);
            }
        }

        #region www handling
        WWWForm formU = new WWWForm();
        formU.AddField("gameID", info.id);
        formU.AddField("json", JsonUtility.ToJson(info));
        WWW wwwU = new WWW(baseURL + "GameDataUpdater.php", formU);
        yield return wwwU;
        #endregion
        Debug.Log(wwwU.text);
    }

    public IEnumerator RequestGamePackUpdate(GamePack pack)
    {
        if (pack.id == -1)
        {
            WWWForm form = new WWWForm();
            WWW www = new WWW(baseURL + "GamePackIndexer.php", form);
            yield return www;

            if (checkError(www))
            {
                pack.id = Int32.Parse(www.text);
            }
        }

        #region www handling
        WWWForm formU = new WWWForm();
        formU.AddField("gamePackID", pack.id);
        formU.AddField("json", JsonUtility.ToJson(pack));
        WWW wwwU = new WWW(baseURL + "GamePackUpdater.php", formU);
        yield return wwwU;
        #endregion
        Debug.Log(wwwU.text);
    }

    public IEnumerator RequestGameData(int id, Action<string> setAction)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("gameID", id);
        WWW www = new WWW(baseURL + "GameDataFinder.php", form);
        yield return www;
        setAction(www.text);
        #endregion
    }

    public IEnumerator RequestGamePack(int id, Action<string> setAction)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("gamePackID", id);
        WWW www = new WWW(baseURL + "GamePackFinder.php", form);
        yield return www;
        setAction(www.text);
        #endregion
    }

    public IEnumerator RequestAllGameDataIndexes(Action<string> setAction)
    {
        WWW www = new WWW(baseURL + "GameDataIndexFinder.php");
        yield return www;
        setAction(www.text);
    }

    public IEnumerator RequestAllGamePackIndexes(Action<string> setAction)
    {
        WWW www = new WWW(baseURL + "GamePackIndexFinder.php");
        yield return www;
        setAction(www.text);
    }

    public IEnumerator RequestDeleteGameData(int id)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("gameID", id);
        WWW www = new WWW(baseURL + "GameDataDeleter.php", form);
        yield return www;
        Debug.Log(www.text);
        #endregion
    }

    public IEnumerator RequestDeleteGamePack(int id)
    {
        #region www handling
        WWWForm form = new WWWForm();
        form.AddField("gamePackID", id);
        WWW www = new WWW(baseURL + "GamePackDeleter.php", form);
        yield return www;
        Debug.Log(www.text);
        #endregion
    }

    private IEnumerator RegularSessionDataUpdate(SessionData data)
    {
        yield return new WaitForSeconds(20f);
        CoroutineSupport.CoroutineComponent.StartCoroutine(RequestSessionDataUpdate(data));
    }

    private bool checkError(WWW www)
    {
        if (www.error == null) { Debug.Log("WWW Ok!: " + www.text); return true; }
        else { Debug.Log("WWW Error: " + www.error); return false; }
    }
}


[System.Serializable]
public class Schools
{
    public School[] schools;
}

[System.Serializable]
public class School
{
    public string schoolName;
    public int schoolID;
}

[System.Serializable]
public class Teams
{
    public Team[] teams;
}

[System.Serializable]
public class Team
{
    public string teamName;
    public string pass;
    public int teamID;
    public int assignmentIndex;
    public int score;
    public bool chosen = false;
}

[System.Serializable]
public class Locations
{
    public Location[] locations;
}

[System.Serializable]
public class Location
{
    public string location;
}

[System.Serializable]
public class Groups
{
    public Group[] groups;
}

[System.Serializable]
public class Group
{
    public string groupName;
    public int groupID;
}

[System.Serializable]
public class FillerMessages
{
    public FillerMessage[] fillerMessages;
}

[System.Serializable]
public class FillerMessage
{
    public string message;
    public string imageUrl;
    public Texture2D image;
}




