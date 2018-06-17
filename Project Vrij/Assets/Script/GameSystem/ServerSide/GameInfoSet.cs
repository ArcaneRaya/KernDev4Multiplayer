//using UnityEngine;
//using System.Collections;

//public enum SetType
//{
//    PROGRESSIONSET, GAMESET
//}

//[CreateAssetMenu(fileName = "NewGameInfoSet", menuName = "Backend/GameInfo Set", order = 50)]
//public class GameInfoSet : ScriptableObject
//{
//    public int Count
//    {
//        get
//        {
//            switch (setType)
//            {
//                case SetType.PROGRESSIONSET:
//                    int count = 0;
//                    foreach (var set in sets)
//                    {
//                        count += set.Count;
//                    }
//                    return count;
//                case SetType.GAMESET:
//                    return games.Length;
//                default:
//                    return 0;
//            }
//        }
//    }

//    //   public int id;
//    public SetType setType;
//    public NetworkedGameInfo[] games;
//    public GameInfoSet[] sets;
//    public bool blockFallThrough;

//    private int progressionPosition;

//    public NetworkedGameInfo GetGameAt(int index)
//    {
//        switch (setType)
//        {
//            case SetType.PROGRESSIONSET:
//                int assignmentNum = 0;
//                foreach (var set in sets)
//                {
//                    assignmentNum += set.Count;
//                    if (index < assignmentNum)
//                    {
//                        return set.GetGameAt(index - (assignmentNum - set.Count));
//                    }
//                }
//                return null;
//            case SetType.GAMESET:
//                return games[index];
//            default:
//                return null;
//        }
//    }

//    public void Setup()
//    {
//        progressionPosition = 0;
//        switch (setType)
//        {
//            case SetType.PROGRESSIONSET:
//                foreach (var set in sets)
//                {
//                    set.Setup();
//                }
//                break;
//            default:
//                break;
//        }
//    }

//    public NetworkedGameInfo GetNextGame()
//    {
//        switch (setType)
//        {
//            case SetType.PROGRESSIONSET:
//                while (sets.Length > progressionPosition)
//                {
//                    if (sets[progressionPosition] == null)
//                    {
//                        NextPosition();
//                        continue;
//                    }
//                    NetworkedGameInfo nextGame = sets[progressionPosition].GetNextGame();
//                    if (nextGame != null)
//                    {
//                        return nextGame;
//                    }
//                    else
//                    {
//                        NextPosition();
//                    }
//                }
//                break;
//            case SetType.GAMESET:
//                while (games.Length > progressionPosition)
//                {
//                    NetworkedGameInfo nextGame = games[progressionPosition];
//                    NextPosition();
//                    if (nextGame != null)
//                    {
//                        return nextGame;
//                    }
//                }
//                break;
//            default:
//                break;
//        }
//        return null;
//    }

//    private void NextPosition()
//    {
//        progressionPosition++;
//        switch (setType)
//        {
//            case SetType.PROGRESSIONSET:
//                if (progressionPosition >= sets.Length && blockFallThrough && sets.Length > 0)
//                {
//                    progressionPosition = 0;
//                }
//                break;
//            case SetType.GAMESET:
//                if (progressionPosition >= games.Length && blockFallThrough && games.Length > 0)
//                {
//                    progressionPosition = 0;
//                }
//                break;
//            default:
//                break;
//        }
//    }
//}
