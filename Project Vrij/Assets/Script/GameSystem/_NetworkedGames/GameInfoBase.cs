using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NetworkedGames
{
    public enum GameType
    {
        NONE,
        VIDEO,
        NATO,
        RANDOMLINES,
        VISUALPROGRAMMING,
        HACK,
        WIREREPAIRS,
        MESSAGEDISPLAY,
        ANSWERBOX
    }

    public class GameInfoBase : ScriptableObject
    {
        [HideInInspector]
        public int id = -1;
        [HideInInspector]
        public GameType gameType = GameType.NONE;

        public static GameInfoBase FromJson(string json)
        {
            GameInfoBase unkownGameInfo = ScriptableObject.CreateInstance<GameInfoBase>();
            JsonUtility.FromJsonOverwrite(json, unkownGameInfo);
            switch (unkownGameInfo.gameType)
            {
                case GameType.VISUALPROGRAMMING:
                    GameInfoVisualProgramming gameInfoVisualProgramming = ScriptableObject.CreateInstance<GameInfoVisualProgramming>();
                    JsonUtility.FromJsonOverwrite(json, gameInfoVisualProgramming);
                    //Debug.Log(gameInfoVisualProgramming.title);
                    return gameInfoVisualProgramming;
                case GameType.MESSAGEDISPLAY:
                    GameInfoMessageDisplay gameInfoMessageDisplay = ScriptableObject.CreateInstance<GameInfoMessageDisplay>();
                    JsonUtility.FromJsonOverwrite(json, gameInfoMessageDisplay);
                    return gameInfoMessageDisplay;
                case GameType.HACK:
                    GameInfoHacked gameInfoHacked = CreateInstance<GameInfoHacked>();
                    JsonUtility.FromJsonOverwrite(json, gameInfoHacked);
                    return gameInfoHacked;
                case GameType.ANSWERBOX:
                    GameInfoAnswerBox gameInfoAnswerBox = CreateInstance<GameInfoAnswerBox>();
                    JsonUtility.FromJsonOverwrite(json, gameInfoAnswerBox);
                    return gameInfoAnswerBox;
                case GameType.NATO:
                    GameInfoNato gameInfoNato = CreateInstance<GameInfoNato>();
                    JsonUtility.FromJsonOverwrite(json, gameInfoNato);
                    return gameInfoNato;
                default:
                    throw new NotImplementedException();
            }
        }
    }

}