using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkedGames
{
    [CreateAssetMenu(fileName = "NewHacked", menuName = "Game Setup/Hacked", order = 0)]
    public class GameInfoHacked : GameInfoVisualProgramming
    {
        //[Header("Game Settings")]
        //public SerializedIsometricEnvironment environment;
        //public GameInfoVisualProgramming.AvailableAction[] availableActions;

        public GameInfoHacked() : base()
        {
            this.gameType = GameType.HACK;
        }
    }
}