using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewShoot", menuName = "Game Setup/Shoot", order = 50)]
public class Shoot : AgentState
{
    public GameObject bulletPrefab;
    private float waitTime;
    private float waitingFor;
    private float postShotWaitTime;
    private bool bulletShot;
    private float waitMultiplier = 1;

    public override void Setup(AgentBase agent)
    {
        //Debug.Log("setup rotat");
        bulletShot = false;
        waitingFor = 0;
        waitTime = (1 / agent.speed) * waitMultiplier;
        postShotWaitTime = waitTime;
        agent.PlaySound(sounds[Random.Range(0, sounds.Length)]);
        agent.anim.Play("IrisBotTurn");
        if (agent.popup != null)
        {
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
        if (!bulletShot)
        {
            bulletShot = true;
            GameObject bullet = Instantiate(bulletPrefab, agent.transform.position + agent.transform.forward, agent.transform.rotation);
            bullet.AddComponent<IrisBotBullet>().Setup(postShotWaitTime);
            agent.environment.ClearPosition(Vector3Int.RoundToInt(agent.transform.position + agent.transform.forward));
        }
        if (waitingFor < postShotWaitTime + waitTime)
        {
            return false;
        }
        return true;
    }

    public override void Complete(AgentBase agent)
    {
    }
}
