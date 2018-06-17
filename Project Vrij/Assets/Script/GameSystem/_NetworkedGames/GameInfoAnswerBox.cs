using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkedGames
{
    [CreateAssetMenu(fileName = "NewAnswerBox", menuName = "Game Setup/Answer Box", order = 0)]
    public class GameInfoAnswerBox : GameInfoExtended
    {
        [Header("Game Settings")]
        [TextArea(3, 10)]
        public string questionText;
        public string requiredAnswer;
        [TextArea(2, 4)]
        public string onFailedResponse;
        public bool takeAnyAnswer;

        public GameInfoAnswerBox() : base()
        {
            this.gameType = GameType.ANSWERBOX;
        }
    }
}