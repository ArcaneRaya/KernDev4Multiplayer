using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentState : ScriptableObject
{
    public AudioClip[] sounds;
    public AudioClip errorSound;
    public Sprite texture;
    public abstract void Setup(AgentBase agent);
    public abstract bool Run(AgentBase agent);
    public abstract void Complete(AgentBase agent);
}
