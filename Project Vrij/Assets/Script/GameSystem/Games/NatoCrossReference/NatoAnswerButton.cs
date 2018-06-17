using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NatoAnswerButton : MonoBehaviour
{
    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }
    public StringEvent onClicked;
    public Text answerText;
    public Button button;
    private string buttonText;

    public void Set(string answer, NatoQuestion natoQuestion)
    {
        Show(true);
        onClicked.AddListener(natoQuestion.Answer);
        answerText.text = answer;
        buttonText = answer;
        button.interactable = true;
    }

    public void Disable()
    {
        button.interactable = false;
    }

    public void Show(bool show)
    {
        answerText.text = "";
        button.image.enabled = show;
    }

    public void ButtonClicked()
    {
        onClicked.Invoke(buttonText);
    }

    private void OnEnable()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ButtonClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ButtonClicked);
    }
}
