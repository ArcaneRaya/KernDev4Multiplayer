using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkedGames;
using UnityEngine.UI;

public class MessageDisplay : GameScreen
{
    private GameInfoMessageDisplay InternalGameInfo
    {
        get
        {
            return currentGame as GameInfoMessageDisplay;
        }
    }

    protected override void InternalSetup()
    {
        if (InternalGameInfo == null)
        {
            throw new System.NullReferenceException("No GameInfo provided");
        }
        messageDisplay.text = InternalGameInfo.message;
        buttonText.text = InternalGameInfo.buttonText;
        button.SetActive(InternalGameInfo.useButton);
    }

    public Text messageDisplay;
    public GameObject button;
    public Text buttonText;


}
