using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkedGames;

[CreateAssetMenu]
public class GamePack : ScriptableObject
{
    public enum PackType
    {
        TUTORIAL,
        RANDOM
    }

    [System.Serializable]
    public class GameIDPair
    {
        public int id = -1;
        public GameInfoBase gameInfo;
    }

    [System.Serializable]
    public class PackIDPair
    {
        public int id = -1;
        public GamePack packInfo;
    }

    //public bool IsAvailable
    //{
    //    get
    //    {
    //        throw new System.NotImplementedException();
    //        //bool assignmentRequirement = true;
    //        //if (requiredCompletedBefore != null)
    //        //{
    //        //    return assignmentRequirement && requiredCompletedBefore.IsCompleted;
    //        //}
    //        //else
    //        //{
    //        //    return assignmentRequirement;
    //        //}
    //    }
    //}

    public bool IsCompleted
    {
        get
        {
            foreach (var gameIDPair in gameIDPairs)
            {
                if (!(gameIDPair.gameInfo as GameInfoExtended).completed)
                {
                    return false;
                }
            }
            return true;
        }
    }

    [HideInInspector]
    public int id = -1;

    public PackType packType;
    public PackIDPair requiredCompletedBefore;
    public bool startBlocked;
    public bool blocked;

    public List<GameIDPair> gameIDPairs;
    //private List<GameInfoExtended> games;


    public void Setup(List<int> finishedGames)
    {
        foreach (var gameIDPair in gameIDPairs)
        {
            if (finishedGames.Contains(gameIDPair.id))
            {
                (gameIDPair.gameInfo as GameInfoExtended).completed = true;
            }
            else
            {
                (gameIDPair.gameInfo as GameInfoExtended).completed = false;

            }
        }
    }

    public bool IsAvailable(ClientState clientState)
    {
        if (requiredCompletedBefore.id == -1)
        {
            return true;
        }
        foreach (var gamePack in clientState.gamePacks)
        {
            if (gamePack.id == requiredCompletedBefore.id)
            {
                return gamePack.IsCompleted;
            }
        }
        Debug.LogWarning("No gamepack found for id " + requiredCompletedBefore.id + ", allowing IsAvailable to default to true");
        return true;
    }
}
