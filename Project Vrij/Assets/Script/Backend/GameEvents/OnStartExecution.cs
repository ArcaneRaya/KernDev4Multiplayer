using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartExecution : MonoBehaviour
{
    public GameEvent gameEvent;

    private void Start()
    {
        gameEvent.Raise();
    }
}
