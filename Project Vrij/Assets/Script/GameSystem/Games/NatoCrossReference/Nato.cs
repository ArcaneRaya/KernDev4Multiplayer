using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NetworkedGames;

public class Nato : GameScreen
{

    private GameInfoNato InternalGameInfo
    {
        get
        {
            return currentGame as GameInfoNato;
        }
    }

    public NatoQuestion natoQuestion;
    //public NatoQuestionSelection natoQuestionSelection;
    //public GameObject continueButton;

    protected override void InternalSetup()
    {
        if (InternalGameInfo == null)
        {
            throw new System.NullReferenceException("No GameInfo provided");
        }
        InternalGameInfo.word.solved = false;
        natoQuestion.Setup(InternalGameInfo.word);
    }

    public void WordCompleted()
    {
        Debug.Log("succeeded!");
        Finished();
    }
}
