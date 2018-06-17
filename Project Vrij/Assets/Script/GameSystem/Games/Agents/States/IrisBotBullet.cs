using System;
using UnityEngine;

public class IrisBotBullet : MonoBehaviour
{
    private float waitTime;
    private float waitingFor;

    public void Setup(float postShotWaitTime)
    {
        waitingFor = 0;
        waitTime = postShotWaitTime;
    }

    private void Update()
    {
        waitingFor += Time.deltaTime;
        if (waitingFor > waitTime)
        {
            Destroy(gameObject);
        }
    }
}