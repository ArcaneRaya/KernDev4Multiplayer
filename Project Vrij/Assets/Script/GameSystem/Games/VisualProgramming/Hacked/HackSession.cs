//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class HackSession : MonoBehaviour
//{
//    public NetworkedGameInfo gameInfo1;
//    public NetworkedGameInfo gameInfo2;
//    public ClientConnection teamOne;
//    public ClientConnection teamTwo;

//    public int teamOneFinish;
//    public int teamTwoFinish;
//    private GameInfoCollection gameInfoCollection;

//    public static void SetupSession(GameInfoCollection currentCollection, NetworkedGameInfo gameInfo1, NetworkedGameInfo gameInfo2, ClientConnection t1, ClientConnection t2)
//    {
//        GameObject hackSessionObject = new GameObject("hackSession");
//        HackSession hackSession = hackSessionObject.AddComponent<HackSession>();
//        hackSession.gameInfo1 = gameInfo1;
//        hackSession.gameInfo2 = gameInfo2;
//        hackSession.teamOne = t1;
//        hackSession.teamTwo = t2;
//        hackSession.gameInfoCollection = currentCollection;
//        hackSession.StartSession();
//    }

//    protected void StartSession()
//    {
//        teamOne.client.gameHandler.InvokeGame(gameInfo1);
//        teamTwo.client.gameHandler.InvokeGame(gameInfo2);
//        teamOne.client.hackedHandler.HackSetup(1);
//        teamOne.client.hackedHandler.onFinishReached += OnFinishReached;
//        teamOne.OnDisconnected += OnUnexpectedDisconnect;
//        teamTwo.OnDisconnected += OnUnexpectedDisconnect;
//        teamTwo.client.hackedHandler.HackSetup(2);
//        teamTwo.client.hackedHandler.onFinishReached += OnFinishReached;
//        teamOneFinish = -1;
//        teamTwoFinish = -1;
//        teamOne.isHacked = true;
//        teamTwo.isHacked = true;
//    }

//    private void OnFinishReached(int team, int index)
//    {
//        switch (team)
//        {
//            case 1:
//                teamOneFinish = index;
//                break;
//            case 2:
//                teamTwoFinish = index;
//                break;
//            default:
//                throw new System.ArgumentException("team has to be 1 or 2");
//        }
//        if (index != -1)
//        {
//            if (teamOneFinish == teamTwoFinish)
//            {
//                Debug.Log("succeeded!");
//                teamOne.client.hackedHandler.onFinishReached -= OnFinishReached;
//                teamTwo.client.hackedHandler.onFinishReached -= OnFinishReached;
//                teamOne.OnDisconnected -= OnUnexpectedDisconnect;
//                teamTwo.OnDisconnected -= OnUnexpectedDisconnect;
//                teamOne.client.hackedHandler.Done();
//                teamTwo.client.hackedHandler.Done();
//                teamOne.client.gameHandler.InvokeGame(gameInfoCollection.GetGameAt(teamOne.currentTeam.assignmentIndex - 1));
//                teamOne.isHacked = false;
//                teamTwo.client.gameHandler.InvokeGame(gameInfoCollection.GetGameAt(teamTwo.currentTeam.assignmentIndex - 1));
//                teamTwo.isHacked = false;
//                Destroy(gameObject);
//            }
//        }
//    }

//    private void OnUnexpectedDisconnect(ClientConnection connection)
//    {
//        teamOne.client.hackedHandler.onFinishReached -= OnFinishReached;
//        teamTwo.client.hackedHandler.onFinishReached -= OnFinishReached;
//        teamOne.OnDisconnected -= OnUnexpectedDisconnect;
//        teamTwo.OnDisconnected -= OnUnexpectedDisconnect;
//        teamOne.isHacked = false;
//        teamTwo.isHacked = false;
//        if (connection == teamOne)
//        {
//            teamTwo.client.gameHandler.InvokeGame(gameInfoCollection.GetGameAt(teamTwo.currentTeam.assignmentIndex - 1));
//            teamTwo.client.hackedHandler.Done();
//        }
//        else
//        {
//            teamOne.client.gameHandler.InvokeGame(gameInfoCollection.GetGameAt(teamOne.currentTeam.assignmentIndex - 1));
//            teamOne.client.hackedHandler.Done();
//        }
//        Destroy(gameObject);
//    }
//}
