using UnityEngine;
using System.Collections;

[System.Serializable]
public class NatoWord
{
    public string sentencePreWord;
    public string word;
    public string sentencePostWord;
    public string[] alternatives;
    public bool solved;
}
