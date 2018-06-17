using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetClientSetup : MonoBehaviour, IPointerDownHandler
{
    public GameEvent onDisplayClientLocation;
    public int tapCount;
    public int requiredTapCountForReset = 5;
    public float timeSinceLastTap;
    public float resetCountTime = 2;

    public void OnPointerDown(PointerEventData eventData)
    {
        timeSinceLastTap = 0;
        tapCount++;
        if (tapCount >= requiredTapCountForReset)
        {
            onDisplayClientLocation.Raise();
        }
    }

    private void Update()
    {
        if (tapCount > 0)
        {
            timeSinceLastTap += Time.deltaTime;
            if (timeSinceLastTap > resetCountTime)
            {
                tapCount = 0;
            }
        }
    }
}
