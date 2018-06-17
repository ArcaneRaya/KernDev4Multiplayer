using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkedGames
{
    [CreateAssetMenu(fileName = "NewNato", menuName = "Game Setup/Nato", order = 0)]
    public class GameInfoNato : GameInfoExtended
    {
        public NatoWord word;

        public GameInfoNato() : base()
        {
            this.gameType = GameType.NATO;
        }
    }
}