using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using NetworkedGames;
using System;

[CreateAssetMenu(fileName = "NewSceneManagement", menuName = "Backend/Scene Management", order = 50)]
public class SceneManagement : ScriptableObject
{
    [System.Serializable]
    public enum MenuType
    {
        GAMEPICKER,
        SOCIALFEED,
        TEAMHOME
    }

    public SceneField natoScene;
    public SceneField hackScene;
    public SceneField visualProgrammingScene;
    public SceneField wireRepair;
    public SceneField menuOverlayScene;
    public SceneField teamHome;
    public SceneField gamePicker;
    public SceneField socialFeed;
    public SceneField messageDisplay;
    public SceneField transmissionOverlay;
    public SceneField answerBoxDisplay;
    public string activeGameScene;
    public string activeMenuScene;
    public bool menuOverlayActivated;
    public bool transmissionOverlayActivated;

    private MenuType recentMenu;

    private void OnEnable()
    {
        activeGameScene = string.Empty;
        activeMenuScene = string.Empty;
        menuOverlayActivated = false;
        transmissionOverlayActivated = false;
        recentMenu = MenuType.TEAMHOME;
    }

    public void Reset()
    {
        OnEnable();
    }

    public void ReloadScenes()
    {
        string gameScene = activeGameScene;
        MenuType curMenutype = recentMenu;
        UnloadAll();

        if (gameScene != string.Empty)
        {
            activeGameScene = gameScene;
            SceneManager.LoadScene(gameScene, LoadSceneMode.Additive);
        }
        else
        {
            recentMenu = curMenutype;
            LoadMenuOverlay();
        }
    }

    public void LoadGameScreen(GameType gameType)
    {
        if (!(activeGameScene == string.Empty))
        {
            SceneManager.UnloadSceneAsync(activeGameScene);
            activeGameScene = string.Empty;
        }

        SceneField sceneToLoad;
        switch (gameType)
        {
            case GameType.NATO:
                sceneToLoad = natoScene;
                break;
            case GameType.HACK:
                sceneToLoad = hackScene;
                break;
            case GameType.VISUALPROGRAMMING:
                sceneToLoad = visualProgrammingScene;
                break;
            case GameType.WIREREPAIRS:
                sceneToLoad = wireRepair;
                break;
            case GameType.MESSAGEDISPLAY:
                sceneToLoad = messageDisplay;
                break;
            case GameType.ANSWERBOX:
                sceneToLoad = answerBoxDisplay;
                break;
            default:
                throw new System.NotImplementedException("LoadGameScreen has not been setup for " + gameType);
        }
        activeGameScene = sceneToLoad;
        UnloadMenuOverlay();
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

    internal void LoadTransmissionOverlay(bool state)
    {
        if (state && !transmissionOverlayActivated)
        {
            SceneManager.LoadScene(transmissionOverlay, LoadSceneMode.Additive);
            transmissionOverlayActivated = true;
        }
        else if (!state && transmissionOverlayActivated)
        {
            SceneManager.UnloadSceneAsync(transmissionOverlay);
            transmissionOverlayActivated = false;
        }
    }

    public void LoadMenuScene(MenuType menuType)
    {
        if (!(activeMenuScene == string.Empty))
        {
            SceneManager.UnloadSceneAsync(activeMenuScene);
            activeMenuScene = string.Empty;
        }

        SceneField sceneToLoad;
        switch (menuType)
        {
            case MenuType.GAMEPICKER:
                sceneToLoad = gamePicker;
                break;
            case MenuType.SOCIALFEED:
                sceneToLoad = socialFeed;
                break;
            case MenuType.TEAMHOME:
                sceneToLoad = teamHome;
                break;
            default:
                throw new System.NotImplementedException("LoadMenuScene has not been setup for " + menuType);
        }
        activeMenuScene = sceneToLoad;
        recentMenu = menuType;
        UnloadGameScene();
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

    public void LoadMenuOverlay()
    {
        if (!menuOverlayActivated)
        {
            SceneManager.LoadScene(menuOverlayScene, LoadSceneMode.Additive);
            menuOverlayActivated = true;
            LoadMenuScene(recentMenu);
        }

    }

    public void UnloadAll()
    {
        recentMenu = MenuType.TEAMHOME;
        UnloadMenuOverlay();
        UnloadGameScene();
    }

    private void UnloadMenuOverlay()
    {
        if (menuOverlayActivated)
        {
            if (activeMenuScene != string.Empty)
            {
                SceneManager.UnloadSceneAsync(activeMenuScene);
                activeMenuScene = string.Empty;
            }
            SceneManager.UnloadSceneAsync(menuOverlayScene);
            menuOverlayActivated = false;
        }
    }

    private void UnloadGameScene()
    {
        if (activeGameScene != string.Empty)
        {
            SceneManager.UnloadSceneAsync(activeGameScene);
            activeGameScene = string.Empty;
        }
    }
}
