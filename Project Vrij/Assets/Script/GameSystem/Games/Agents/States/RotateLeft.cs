using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewRotateLeft", menuName = "Game Setup/Rotate Left", order = 50)]
public class RotateLeft : AgentState
{
    public float degrees;
    private float waitTime;
    private float waitingFor;
    private float waitMultiplier = 1;

    public override void Setup(AgentBase agent)
    {
        waitingFor = 0;
        waitTime = (1 / agent.speed) * waitMultiplier;
        degrees = 90;
        agent.PlaySound(sounds[Random.Range(0, sounds.Length)]);
        agent.anim.Play("IrisBotTurn");
        if(agent.popup!= null)
        {
            Debug.Log("dicks");
            agent.popup.SetPopupTexture(texture);
        }
    }

    public override bool Run(AgentBase agent)
    {
        waitingFor += Time.deltaTime;
        if (waitingFor < waitTime)
        {
            return false;
        }
        float currentDegrees = Time.deltaTime * agent.rotationSpeed;
        if (degrees - currentDegrees > 0)
        {
            agent.transform.Rotate(Vector3.up, -currentDegrees);
            degrees -= currentDegrees;
            return false;
        }
        else
        {
            agent.transform.Rotate(Vector3.up, -degrees);
            //agent.LoadNextState();
            return true;
        }
    }

    public override void Complete(AgentBase agent)
    {
    }
}
