using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetworkedGames;

public class GamePicker : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject titlePrefab;
    public GameObject gameiconPrefab;
    public float distanceBetweenGamePacks;
    public float gamePadding;
    public ClientState clientState;
    public SceneManagement sceneManagement;

    private void Start()
    {
        Vector2 viewSize = CanvasManager.DefaultCanvasSize + scrollRect.GetComponent<RectTransform>().sizeDelta;
        float yOffset = 0;
        //int progression = assignmentProgression.value;
        foreach (var pack in clientState.gamePacks)
        {
            if (pack.packType == GamePack.PackType.TUTORIAL)
            {
                if (pack.IsAvailable(clientState) && !pack.blocked)
                {
                    yOffset = SpawnGamePack(scrollRect.content.transform, pack, yOffset, viewSize);
                    if (yOffset == -1)
                    {
                        return;
                    }
                    yOffset -= distanceBetweenGamePacks;
                }
            }
        }
        scrollRect.content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Mathf.Abs(yOffset));
    }

    private float SpawnGamePack(Transform target, GamePack pack, float yOffset, Vector2 viewSize)
    {
        //throw new System.NotImplementedException();
        float innitXOffset = 0;
        float xOffset = innitXOffset;

        // spawn title
        GameObject packTitle = Instantiate(titlePrefab, target);
        packTitle.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset, yOffset);
        packTitle.GetComponent<Text>().text = pack.name;
        yOffset -= packTitle.GetComponent<RectTransform>().sizeDelta.y;

        int countOffset = 0;
        // spawn gameIcons
        for (int i = 0; i < pack.gameIDPairs.Count; i++)
        {
            if (pack.gameIDPairs[i].gameInfo.gameType == GameType.MESSAGEDISPLAY)
            {
                countOffset++;
                if ((pack.gameIDPairs[i].gameInfo as GameInfoExtended).completed || !(pack.gameIDPairs[i].gameInfo as GameInfoExtended).IsAvailable(pack))
                {
                    continue;
                }
                else
                {
                    clientState.currentGame = pack.gameIDPairs[i].gameInfo as GameInfoExtended;
                    sceneManagement.LoadGameScreen(pack.gameIDPairs[i].gameInfo.gameType);
                    return -1;
                }
            }
            if (pack.gameIDPairs[i].gameInfo.gameType == GameType.ANSWERBOX)
            {
                countOffset++;
                if ((pack.gameIDPairs[i].gameInfo as GameInfoExtended).completed || !(pack.gameIDPairs[i].gameInfo as GameInfoExtended).IsAvailable(pack))
                {
                    continue;
                }
                else
                {
                    clientState.currentGame = pack.gameIDPairs[i].gameInfo as GameInfoExtended;
                    sceneManagement.LoadGameScreen(pack.gameIDPairs[i].gameInfo.gameType);
                    return -1;
                }
            }
            GameObject gameIcon = Instantiate(gameiconPrefab, target);
            gameIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset, yOffset);
            gameIcon.GetComponent<GameIcon>().Setup(pack.gameIDPairs[i].gameInfo as GameInfoExtended, (i - countOffset + 1).ToString(), pack);
            xOffset += gameIcon.GetComponent<RectTransform>().sizeDelta.x + gamePadding;
            if (viewSize.x - xOffset < gameIcon.GetComponent<RectTransform>().sizeDelta.x)
            {
                yOffset -= gameIcon.GetComponent<RectTransform>().sizeDelta.y + gamePadding;
                xOffset = innitXOffset;
            }
        }
        if (xOffset != innitXOffset)
        {
            yOffset -= gameiconPrefab.GetComponent<RectTransform>().sizeDelta.y + gamePadding;
        }
        return yOffset;
    }
}
