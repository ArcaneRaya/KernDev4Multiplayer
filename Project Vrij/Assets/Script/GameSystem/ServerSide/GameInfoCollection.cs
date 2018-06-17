//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "NewGameInfoCollection", menuName = "Game Setup/Game Info Collection", order = 50)]
//public class GameInfoCollection : ScriptableObject
//{
//    public List<GameInfoSet> gameSets;

//    public bool IsIncrementAllowed(int assignmentIndex)
//    {
//        int assignmentNum = 0;
//        foreach (var gameset in gameSets)
//        {
//            assignmentNum += gameset.Count;
//            if (assignmentIndex < assignmentNum)
//            {
//                if (assignmentIndex + 1 < assignmentNum)
//                {
//                    return true;
//                }
//                else
//                {
//                    if (gameset.blockFallThrough)
//                    {
//                        return false;
//                    }
//                    else
//                    {
//                        return true;
//                    }
//                }
//            }
//        }
//        return false;
//    }

//    public NetworkedGameInfo GetGameAt(int assignmentIndex)
//    {
//        int assignmentNum = 0;
//        foreach (var gameset in gameSets)
//        {
//            assignmentNum += gameset.Count;
//            if (assignmentIndex < assignmentNum)
//            {
//                return gameset.GetGameAt(assignmentIndex - (assignmentNum - gameset.Count));
//            }
//        }
//        return null;
//    }
//}