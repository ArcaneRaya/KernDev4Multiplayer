using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkedGames;

[CreateAssetMenu(fileName = "NewClientState", menuName = "Backend/Client State", order = 150)]
public class ClientState : ScriptableObject
{
    public bool gameplayStarted;
    public bool moneyMaker;

    public GameInfoExtended currentGame;
    public List<int> finishedGames;

    public List<GamePack> gamePacks;
    public List<GameInfoExtended> games;


    private void OnEnable()
    {
        gameplayStarted = false;
        Reset();
    }

    public void Reset()
    {
        finishedGames = new List<int>();
    }
}