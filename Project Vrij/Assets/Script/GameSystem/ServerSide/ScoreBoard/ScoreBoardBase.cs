using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ScoreBoardBase : MonoBehaviour
{
    public bool active = false;
    public SessionData data;
    public GameObject scoreObjectPrefab;
    public Image blur;
    public Animator anim;
    private RectTransform childTransform;
    private Text totalScore;
    private float distance;
    private List<Team> connectedTeams = new List<Team>();
    private List<int> teamIndexes = new List<int>();
    private List<ScoreObject> scoreObjects = new List<ScoreObject>();
    private LobbyObject[] lobbyObjects;

    private float intervall = 2f;
    private float currTime = 0;

    void Start()
    {
        blur.material.SetFloat("_Size", 0);
        childTransform = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        lobbyObjects = GetComponentsInChildren<LobbyObject>();
        foreach(LobbyObject lobbyObject in lobbyObjects)
        {
            lobbyObject.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (active)
        {
            if (Time.time > currTime)
            {
                currTime += intervall;
                RefreshData();
                Updatescores();
            }
        }
    }

    public void Activate()
    {
        StartCoroutine(OpenScoreBoard());
        anim.SetTrigger("Rise");
        for (int i = 0; i < lobbyObjects.Length; i++)
        {
            lobbyObjects[i].Dissolve();
        }
        active = true;
    }

    public void AddTeam()
    {
        foreach (Team team in data.teams.teams)
        {
            if (team.chosen && !teamIndexes.Contains(team.teamID))
            {
                connectedTeams.Add(team);
                teamIndexes.Add(team.teamID);
                lobbyObjects[connectedTeams.Count - 1].gameObject.SetActive(true);
                lobbyObjects[connectedTeams.Count - 1].AssignTeam(team);
            }
        }
    }

    private IEnumerator OpenScoreBoard()
    {
        totalScore = GameObject.FindGameObjectWithTag("TotalScore").GetComponent<Text>();
        totalScore.text = "Totale Opbrengst: " + TotalPoints();
        yield return new WaitForSeconds(4f);
        float height = Screen.height * 0.6f;
        float offset = height / connectedTeams.Count;
        for (int i = 0; i < connectedTeams.Count; i++)
        {
            ScoreObject temp = Instantiate(scoreObjectPrefab, childTransform).GetComponent<ScoreObject>();
            temp.GetComponent<RectTransform>().localPosition = new Vector2(0, ((height * 0.5f) - (i * offset * 1.2f) - (offset * 0.5f)));
            //float size = offset / temp.GetComponent<RectTransform>().sizeDelta.y;
            //temp.GetComponent<RectTransform>().localScale = new Vector3(temp.GetComponent<RectTransform>().localScale.x, size, size);
            float ratio = 1f - (0.1f * i);
            temp.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, offset * 0.5f);
            temp.teamName.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, offset * (1.5f * ratio));
            temp.img.GetComponent<RectTransform>().sizeDelta = new Vector2(offset * (1.5f * ratio) , offset * (1.5f * ratio) );
            temp.score.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.1f, offset);
            temp.init(connectedTeams[i], i + 1);
            scoreObjects.Add(temp);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private int TotalPoints()
    {
        int total = 0;
        foreach(Team team in data.teams.teams)
        {
            total += team.score;
        }
        return total;
    }

    public void RefreshData()
    {
        connectedTeams.Clear();
        foreach (int teamIndex in teamIndexes)
        {
            foreach(Team team in data.teams.teams)
            {
                if (team.teamID == teamIndex)
                {
                    connectedTeams.Add(team);
                }
            }
        }
    }

    public List<Team> SortRanks()
    {
        List<Team> rankList = new List<Team>();
        List<Team> unsorted = connectedTeams;

        rankList = unsorted.OrderBy(x => x.score).ToList();
        rankList.Reverse();
        return rankList;
    }

    public void Updatescores()
    {
        totalScore.text = "Totale Opbrengst: " + TotalPoints();
        List<Team> rankList = SortRanks();
        for (int i = 0; i < scoreObjects.Count; i++)
        {
            Debug.Log(i);
            scoreObjects[i].UpdateScore(rankList[i]);
        }
    }
}
