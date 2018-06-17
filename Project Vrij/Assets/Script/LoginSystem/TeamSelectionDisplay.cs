using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelectionDisplay : MonoBehaviour
{
    public GameEvent OnDisplayTeamCreation;
    public GameEvent OnSetTeam;
    public GameEvent OnCreateTeam;
    public Transform parent;
    public GameObject[] teamButtonPrefabs;
    public StringListVariable availableTeams;

    public void SpawnTeams()
    {
        for (int i = 0; i < availableTeams.Value.Count; i++)
        {
            GameObject newButton = Instantiate(teamButtonPrefabs[i % teamButtonPrefabs.Length], parent);
            newButton.GetComponent<TeamButton>().SetButton(
                availableTeams.Value[i],
                new Vector2(
                    ((i % 4) - 1.5f) * (teamButtonPrefabs[i % teamButtonPrefabs.Length].GetComponent<RectTransform>().sizeDelta.x * 1.2f),
                    Mathf.Floor(i / 4) * -teamButtonPrefabs[i % teamButtonPrefabs.Length].GetComponent<RectTransform>().sizeDelta.y * 1.2f),
                OnSetTeam
            );
        }
    }
}
