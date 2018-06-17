using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    IRIS, BJORN
}

[CreateAssetMenu]
public class VoiceMessage : ScriptableObject
{
    public enum TriggerMoment
    {
        START,
        END
    }
    public int day;
    public bool played;
    public bool dayEnding;
    public TriggerMoment triggerMoment;
    public NetworkedGames.GameInfoBase triggerGame;
    public int unBlockPackId;
    public Character character;
    public AudioClip message;
}
