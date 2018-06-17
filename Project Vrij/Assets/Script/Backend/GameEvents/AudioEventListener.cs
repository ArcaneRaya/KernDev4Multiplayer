using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventListener : MonoBehaviour
{
    public AudioEvent audioEvent;
    public UnityEvent onRaise;

    private void OnEnable()
    {
        if (audioEvent != null)
        {
            audioEvent.AddListener(this);
        }
    }

    private void OnDisable()
    {
        if (audioEvent != null)
        {
            audioEvent.RemoveListener(this);
        }
    }

    public void OnRaise(string name, AudioClip voiceLine)
    {
        VoiceLineManager manager = onRaise.GetPersistentTarget(0) as VoiceLineManager;
        manager.Setup(name, voiceLine);
    }
}
