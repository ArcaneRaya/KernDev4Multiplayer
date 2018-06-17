using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkedGames;

public class HackManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameInfoHacked[] hackedGames;
    //public List<HackedSession> hackedSessions;
    public HackedSession session;
    public int gameFinishCount;


    public bool isHacking;

    public void OnGameFinished()
    {
        gameFinishCount++;
        if (session.settingUp)
        {
            return;
        }
        int chance = Random.Range(0, 100);
        //return;
        if (isHacking)
        {
            return;
        }
        if (gameFinishCount > 15)
        {
            StartHacked();
        }
    }

    public void StartHacked()
    {
        isHacking = true;
        //if (isHacking)
        //{
        //    return;
        //}
        //isHacking = true;
        session.QuitHackedGame();
        session = new HackedSession();
        session.settingUp = true;
        session.connectionTeam1 = GetRandomConnection();
        if (session.connectionTeam1 == null)
        {
            Debug.Log("no connection found");
            return;
        }
        session.connectionTeam1.isHacked = true;
        session.connectionTeam2 = GetRandomConnection();
        if (session.connectionTeam2 == null)
        {
            Debug.Log("no next connection found");
            session.connectionTeam1.isHacked = false;
            return;
        }
        session.connectionTeam2.isHacked = true;
        session.gameJson = JsonUtility.ToJson(hackedGames[Random.Range(0, hackedGames.Length)]);

        session.SetupGame();
        session.onFinished += () => { gameFinishCount = 0; session.settingUp = false; };
    }

    private ClientConnection GetRandomConnection()
    {
        int randomClientPosition = Random.Range(0, gameManager.clientConnection.clients.Count);
        ClientConnection connection = gameManager.clientConnection.clients[randomClientPosition];
        int tryCount = 0;
        while (!connection.isConnected || connection.isHacked || !connection.isLoggedIn)
        {
            if (tryCount > gameManager.clientConnection.clients.Count)
            {
                return null;
            }
            randomClientPosition++;
            randomClientPosition = randomClientPosition % gameManager.clientConnection.clients.Count;
            connection = gameManager.clientConnection.clients[randomClientPosition];
            tryCount++;
        }
        return connection;
    }
}