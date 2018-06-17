using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkedGames;
using UnityEngine.UI;

public class AnswerBox : GameScreen
{
    private GameInfoAnswerBox InternalGameInfo
    {
        get
        {
            return currentGame as GameInfoAnswerBox;
        }
    }

    protected override void InternalSetup()
    {
        if (InternalGameInfo == null)
        {
            throw new System.NullReferenceException("No GameInfo provided");
        }
        questionDisplay.text = InternalGameInfo.questionText;
        failedTextContainer.text = InternalGameInfo.onFailedResponse;
    }

    public GameEvent onFailed;
    public Text questionDisplay;
    public InputField inputField;
    public Text failedTextContainer;

    public void CheckAnswer()
    {
        if (inputField.text.ToLower() == InternalGameInfo.requiredAnswer || InternalGameInfo.takeAnyAnswer)
        {
            Debug.Log("correct");
            OnFinished.Raise();
        }
        else
        {
            Debug.Log("wrong");
            onFailed.Raise();
        }
    }
}
