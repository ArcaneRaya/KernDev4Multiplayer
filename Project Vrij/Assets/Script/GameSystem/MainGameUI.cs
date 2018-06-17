using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameUI : MonoBehaviour
{

    public SceneManagement sceneManagement;
    public ClientState clientState;
    public GameObject gamesButton;
    public GameObject randomGameButton;

    public void Start()
    {
        foreach (var pack in clientState.gamePacks)
        {
            pack.Setup(clientState.finishedGames);
        }
        if (!clientState.gameplayStarted)
        {
            gamesButton.SetActive(false);
            randomGameButton.SetActive(false);
            return;
        }
        bool allTutPacksDone = true;
        bool randomPackAvailable = false;
        foreach (var gamepack in clientState.gamePacks)
        {
            if (!gamepack.blocked && gamepack.IsAvailable(clientState) && gamepack.packType == GamePack.PackType.RANDOM)
            {
                randomPackAvailable = true;
                //return;
            }
            if (!gamepack.blocked && gamepack.IsAvailable(clientState) && !gamepack.IsCompleted && gamepack.packType == GamePack.PackType.TUTORIAL)
            {
                allTutPacksDone = false;
            }
        }
        if (allTutPacksDone && randomPackAvailable)
        {
            randomGameButton.SetActive(true);
        }
        else
        {
            randomGameButton.SetActive(false);
        }
    }

    public void OnDisplayTeamHome()
    {
        sceneManagement.LoadMenuScene(SceneManagement.MenuType.TEAMHOME);
    }

    public void OnDisplayGamePicker()
    {
        sceneManagement.LoadMenuScene(SceneManagement.MenuType.GAMEPICKER);
    }

    public void OnStartRandomGame()
    {
        int randomPackPosition = Random.Range(0, clientState.gamePacks.Count);
        while (!(clientState.gamePacks[randomPackPosition].IsAvailable(clientState) &&
                 !clientState.gamePacks[randomPackPosition].blocked &&
                 clientState.gamePacks[randomPackPosition].packType == GamePack.PackType.RANDOM
               ))
        {
            randomPackPosition++;
            randomPackPosition = randomPackPosition % clientState.gamePacks.Count;
        }
        int randomGamePosition = Random.Range(0, clientState.gamePacks[randomPackPosition].gameIDPairs.Count);
        NetworkedGames.GameInfoBase randomGame = clientState.gamePacks[randomPackPosition].gameIDPairs[randomGamePosition].gameInfo;
        clientState.moneyMaker = true;
        clientState.currentGame = randomGame as NetworkedGames.GameInfoExtended;
        sceneManagement.LoadGameScreen(randomGame.gameType);
    }
}
