using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class ScoreObject : MonoBehaviour
{
    public Team team = new Team();
    public Image img;

    public Text teamName;
    public Text score;

    public void init(Team team, int rank)
    {
        this.team = team;
        img.sprite = Resources.Load<Sprite>("Number" + rank.ToString());
        teamName.text = team.teamName;
        score.text = team.score.ToString();
        //teamName.rectTransform.sizeDelta = new Vector2(700f - (70f * ((float)rank - 1f)), 200f - (20f * ((float)rank - 1f)));
        //score.rectTransform.sizeDelta = new Vector2(300f - (30f * ((float)rank - 1f)), 200f - (20f * ((float)rank - 1f)));
        //img.rectTransform.sizeDelta = new Vector2(300f - (30f * ((float)rank - 1f)), 200f - (20f * ((float)rank - 1f)));
    }

    public void UpdateScore(Team team)
    {
        this.team = team;
        teamName.text = team.teamName;
        score.text = team.score.ToString();
    }   
}
