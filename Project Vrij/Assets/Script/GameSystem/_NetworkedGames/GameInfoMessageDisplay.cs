using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkedGames
{
    [CreateAssetMenu(fileName = "NewMessageDisplay", menuName = "Game Setup/Message Display", order = 0)]
    public class GameInfoMessageDisplay : GameInfoExtended
    {
        [System.Serializable]
        public class AvailableAction
        {
            public ProgrammingActions action;
            public int amount;
        }

        [Header("Game Settings")]
        [TextArea(3, 10)]
        public string message;
        public bool useButton;
        public string buttonText;
        public string postClickMessage;

        public GameInfoMessageDisplay() : base()
        {
            this.gameType = GameType.MESSAGEDISPLAY;
        }
    }
}