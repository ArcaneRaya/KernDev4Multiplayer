using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MessagePairVariable", menuName = "Variables/Message Pair", order = 0)]
public class MessagePairVariable : ScriptableObject
{
    public GameManager.MessagePair value;
}
