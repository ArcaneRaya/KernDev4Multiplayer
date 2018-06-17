using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkedGames
{

    public class GameInfoExtended : GameInfoBase
    {
        //public bool IsAvailable
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //}
        [Header("System settings")]
        public GamePack.GameIDPair requiredGame;
        public bool completed;
        public int completionScore;
        [Header("Preface")]
        public string title;
        [TextArea(3, 4)]
        public string tutorialText;

        public bool IsAvailable(GamePack gamePack)
        {
            if (requiredGame.id == -1)
            {
                return true;
            }
            foreach (var gameIDPair in gamePack.gameIDPairs)
            {
                if (gameIDPair.id == requiredGame.id)
                {
                    return (gameIDPair.gameInfo as GameInfoExtended).completed;
                }
            }
            Debug.LogWarning("No gameinfo found for id " + requiredGame.id + ", allowing IsAvailable to default to true");
            return true;
        }
    }

}