using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMoveForward", menuName = "Game Setup/Move Forward", order = 50)]
public class MoveForward : AgentState
{
    public Vector3 targetPosition;
    private float waitTime;
    private float waitingFor;
    private float waitMultiplier = 1;
    private bool skip;
    private bool soundplayed;

    public override void Setup(AgentBase agent)
    {
        soundplayed = false;
        waitingFor = 0;
        waitTime = (1 / agent.speed) * waitMultiplier;
        targetPosition = agent.oldPosition + agent.transform.forward;
        skip = !agent.environment.IsWalkable(Vector3Int.RoundToInt(targetPosition));
        if (skip)
        {
            agent.PlaySoundDelayed(errorSound, waitTime);
            agent.anim.Play("IrisBotError");
            if (agent.popup != null)
            {
                Debug.Log("eerste plek");

                agent.popup.SetErrorTexture(texture);
            }
            waitTime *= 3;
        }
        //Debug.Log("Set target to: " + targetPosition + " which is " + (skip ? "not allowed" : "allowed"));
    }

    public override bool Run(AgentBase agent)
    {
        waitingFor += Time.deltaTime;
        if (waitingFor < waitTime)
        {
            return false; ;
        }
        if (skip)
        {
            Debug.Log("skipping");
            return true;
        }
        if (!soundplayed)
        {
            soundplayed = true;
            agent.PlaySound(sounds[Random.Range(0, sounds.Length)]);
            agent.anim.Play("IrisBotMove");
            if (agent.popup != null)
            {
                Debug.Log("test1");

                agent.popup.SetPopupTexture(texture);
            }
        }
        Vector3 nextPosition = agent.transform.position + agent.transform.forward * agent.speed * Time.deltaTime;
        if (Vector3.Distance(agent.oldPosition, nextPosition) > Vector3.Distance(agent.oldPosition, agent.oldPosition + agent.transform.forward))
        {
            agent.transform.position = agent.oldPosition + agent.transform.forward;
            //agent.LoadNextState();
            //Debug.Log("done");
            return true;
        }
        else
        {
            agent.transform.position = nextPosition;
            return false;
        }
    }

    public override void Complete(AgentBase agent)
    {
        if (skip)
        {
            return;
        }
        if (agent.environment.IsWalkable(Vector3Int.RoundToInt(targetPosition)))
        {
            agent.oldPosition = Vector3Int.RoundToInt(targetPosition);
        }
    }
}
