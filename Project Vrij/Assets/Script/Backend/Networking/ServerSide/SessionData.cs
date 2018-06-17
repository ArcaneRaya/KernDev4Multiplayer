using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSessionData", menuName = "Backend/Session Data", order = 100)]
[System.Serializable]
public class SessionData : ScriptableObject
{
    public School school;
    public Group group;
    public Teams teams;
    public int day;

    public List<FeedItem> feed;

    public void Reset()
    {
        school = null;
        group = null;
        teams = null;
        feed = null;
    }
}
