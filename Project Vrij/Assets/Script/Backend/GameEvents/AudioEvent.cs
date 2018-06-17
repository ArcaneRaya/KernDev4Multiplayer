using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioEvent", menuName = "Backend/Audio Event")]
public class AudioEvent : ScriptableObject
{
    [SerializeField]
    public string callerName;
    public AudioClip voiceMessage;
    private List<AudioEventListener> listeners;

    public void AddListener(AudioEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void RemoveListener(AudioEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnRaise(callerName, voiceMessage);
        }
    }
}
