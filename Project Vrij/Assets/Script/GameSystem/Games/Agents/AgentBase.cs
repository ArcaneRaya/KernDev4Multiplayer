using System.Collections;
using System.Collections.Generic;
using NetworkedGames;
using UnityEngine;

public abstract class AgentBase : MonoBehaviour
{
    public float speed = 3;
    public float rotationSpeed = 15;
    public Vector3 oldPosition;
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
    public Queue<AgentState> stateQueue;
    public AgentState currentState;
    [System.NonSerialized]
    public Environment environment;
    private AudioSource audioSource;

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public IrisBotPopup popup;

    protected virtual void Awake()
    {
        spawnPosition = transform.position;
        oldPosition = spawnPosition;
        spawnRotation = transform.localRotation;
        audioSource = gameObject.AddComponent<AudioSource>();
        anim = GetComponent<Animator>();
        popup = GetComponent<IrisBotPopup>();
    }

    public virtual void LoadNextState()
    {
        if (currentState != null)
        {
            currentState.Complete(this);
            currentState = null;
        }
    }

    public virtual void ResetPosition()
    {
        transform.position = spawnPosition;
        transform.localRotation = spawnRotation;
        oldPosition = spawnPosition;
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

    public void PlaySoundDelayed(AudioClip sound, float time)
    {
        audioSource.clip = sound;
        audioSource.PlayDelayed(time);
    }

    protected virtual void Update()
    {
        if (currentState != null)
        {
            if (currentState.Run(this))
            {
                LoadNextState();
            }
        }
    }
}
