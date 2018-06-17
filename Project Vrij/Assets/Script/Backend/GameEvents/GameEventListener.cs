using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public UnityEvent onRaise;

    private void OnEnable()
    {
        if (gameEvent != null)
        {
            gameEvent.AddListener(this);
        }
    }

    private void OnDisable()
    {
        if (gameEvent != null)
        {
            gameEvent.RemoveListener(this);
        }
    }

    public void OnRaise()
    {
        onRaise.Invoke();
    }
}
