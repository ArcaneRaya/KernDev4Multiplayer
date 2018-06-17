using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkedGames
{
    [CreateAssetMenu(fileName = "NewVisualProgramming", menuName = "Game Setup/Visual Programming", order = 0)]
    public class GameInfoVisualProgramming : GameInfoExtended
    {
        [System.Serializable]
        public class AvailableAction
        {
            public ProgrammingActions action;
            public int amount;
        }

        [Header("Game Settings")]
        public SerializedIsometricEnvironment environment;
        public AvailableAction[] availableActions;

        public GameInfoVisualProgramming() : base()
        {
            this.gameType = GameType.VISUALPROGRAMMING;
        }
    }
}