using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreDisplayHelper : MonoBehaviour
{
    public ClientState clientState;

    public void SetScore()
    {
        GetComponent<Text>().text =
            clientState.moneyMaker || !clientState.currentGame.completed ?
            clientState.currentGame.completionScore.ToString() : "0";
    }
}
