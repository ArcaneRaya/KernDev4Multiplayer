using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSetup : MonoBehaviour
{
    public StringVariable generatedMatchName;
    public GameEvent onMatchNameGenerated;
    public GameEvent onDisplayClientLocation;

    public const string matchNameKey = "matchname";

    private void Start()
    {
        if (PlayerPrefs.HasKey(matchNameKey))
        {
            generatedMatchName.value = PlayerPrefs.GetString(matchNameKey);
            Debug.Log("matchname found");
            onMatchNameGenerated.Raise();
        }
        else
        {
            onDisplayClientLocation.Raise();
        }
    }
}
