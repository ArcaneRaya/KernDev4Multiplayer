using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NetworkedGames;

public class Hacked : GameScreen
{
    //private GameInfoHacked InternalGameInfo
    //{
    //    get
    //    {
    //        return currentGame as GameInfoHacked;
    //    }
    //}

    //protected override void InternalSetup()
    //{
    //    if (InternalGameInfo == null)
    //    {
    //        BuildDebugger.Log("No valid GameInfo provided");
    //        throw new System.NullReferenceException("No valid GameInfo provided");
    //    }
    //    irisBots = InternalGameInfo.environment.SpawnEnvironment(environmentParent.transform);
    //}

    //public GameObject environmentParent;
    //public List<AgentBase> irisBots;
    //public IntVariable reachedFinish;
    //public GameEvent OnHackedReachedFinish;
    ////    public GameObject continueButton;

    //public void ActivateIrisBots()
    //{
    //    foreach (var irisBot in irisBots)
    //    {
    //        if (irisBot.GetType().Equals(typeof(IrisBotProgrammable)))
    //        {
    //            irisBot.environment = InternalGameInfo.environment;
    //            (irisBot as IrisBotProgrammable).Activate();
    //        }
    //    }
    //}

    //public void OnIrisBotFinished()
    //{
    //    foreach (var irisBot in irisBots)
    //    {
    //        if (irisBot.GetType().Equals(typeof(IrisBotProgrammable)))
    //        {
    //            reachedFinish.value = (irisBot as IrisBotProgrammable).ReachedFinish;
    //            OnHackedReachedFinish.Raise();
    //        }
    //    }
    //    Finished();
    //}
    protected override void InternalSetup()
    {
        throw new System.NotImplementedException();
    }
}
