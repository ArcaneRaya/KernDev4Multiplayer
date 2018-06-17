using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLoop", menuName = "Game Setup/Loop", order = 50)]
public class LoopActions : AgentState
{
    public int loopAmount = 2;

    public List<AgentState> loopStates;
    private int timesRun;
    private AgentState currentState;
    private int iterator;

    public override void Complete(AgentBase agent)
    {
        //throw new System.NotImplementedException();
    }

    public override bool Run(AgentBase agent)
    {
        if (loopStates == null)
        {
            return true;
        }
        if (loopStates.Count == 0)
        {
            return true;
        }
        if (currentState != null)
        {
            if (currentState.Run(agent))
            {
                currentState.Complete(agent);
                if (iterator < loopStates.Count)
                {
                    currentState = loopStates[iterator];
                    currentState.Setup(agent);
                    iterator++;
                }
                else
                {
                    if (timesRun < loopAmount)
                    {
                        //Debug.Log("lets do it another time");
                        timesRun++;
                        iterator = 0;
                        currentState = loopStates[iterator];
                        currentState.Setup(agent);
                        iterator++;
                    }
                    else
                    {
                        //Debug.Log(timesRun);
                        return true;
                    }
                }
            }
        }
        else
        {
            return true;
        }
        return false;
    }

    public override void Setup(AgentBase agent)
    {
        //Debug.Log("setup loop action");
        //Debug.Log(loopAmount);
        timesRun = 1;
        iterator = 0;
        if (loopStates != null)
        {
            if (loopStates.Count > 0)
            {
                currentState = loopStates[iterator];
                currentState.Setup(agent);
            }
        }
        iterator++;
    }
}
