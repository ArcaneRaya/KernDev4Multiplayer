using UnityEngine;
using System;
using UnityEngine.UI;
using NetworkedGames;

public class GameIcon : MonoBehaviour
{
    public Image gameIcon;
    public GameObject completedIcon;
    public GameObject lockedIcon;
    public Button button;
    public ClientState clientState;
    public SceneManagement sceneManagement;
    public GameInfoExtended linkedGame;

    public void Setup(GameInfoExtended game, string text, GamePack gamePack)
    {
        linkedGame = game;
        button.GetComponentInChildren<Text>().text = text;
        completedIcon.SetActive(game.completed);
        bool isAvailable = game.IsAvailable(gamePack);
        lockedIcon.SetActive(!isAvailable);
        button.interactable = isAvailable;
        button.onClick.AddListener(Clicked);
    }

    private void Clicked()
    {
        clientState.currentGame = linkedGame;
        //throw new System.NotImplementedException();
        sceneManagement.LoadGameScreen(linkedGame.gameType);
    }
}