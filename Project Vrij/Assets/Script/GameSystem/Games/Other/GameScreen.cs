using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkedGames;

public abstract class GameScreen : MonoBehaviour
{
#if UNITY_EDITOR
    public bool testRun;
#endif
    public ClientState clientState;
    public GameEvent OnFinished;
    public GameEvent OnHideTutorial;
    public GameInfoExtended currentGame;

    [Header("Game Setup")]
    public GameEvent onGameLoaded;
    public StringVariable gameTitle;
    public StringVariable gameDescription;

    //protected bool showingTutorial;

    protected void Start()
    {
#if UNITY_EDITOR
        if (testRun)
        {
            if (currentGame == null)
            {
                Debug.LogWarning("No game loaded for testRun, nothing will happen.");
                return;
            }
            clientState.currentGame = currentGame;
            Setup(currentGame);
            return;
        }
#endif
        //    gameProgression.GameScreenReady(this);
        Setup(clientState.currentGame);
    }

    public void Setup(GameInfoExtended gameInfo)
    {
        gameTitle.value = gameInfo.title;
        gameDescription.value = gameInfo.tutorialText;
        //showingTutorial = gameInfo.tutorialText != "";
        currentGame = gameInfo;
        InternalSetup();
        onGameLoaded.Raise();
    }

    protected abstract void InternalSetup();

    public void Show()
    {
        throw new System.NotImplementedException();
    }

    protected void Finished()
    {
        OnFinished.Raise();
    }

    //protected void ContinueToNextGame()
    //{
    //    gameProgression.LoadNextGame();
    //}
}
