using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public IntVariable teamScore;
    public Text text;

    public void Start()
    {
        text.text = teamScore.value.ToString();
    }
}
