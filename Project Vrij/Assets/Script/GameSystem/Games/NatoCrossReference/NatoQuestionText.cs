using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NatoQuestionText : MonoBehaviour
{

    public Text questionText;

    public void Set(string word)
    {
        if (word == null)
        {
            throw new System.ArgumentNullException("word");
        }
        questionText.text = NatoDictionary.ProvideSentence(word);
    }
}
