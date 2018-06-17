using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Button))]
public class TeamButton : MonoBehaviour
{
    public StringVariable teamName;
    private GameEvent clickEvent;
    private RectTransform rectTransform;
    private Button button;
    private Text buttonText;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<Text>();
    }

    public void SetButton(string name, Vector2 position, GameEvent onClicked)
    {
        Start();
        buttonText.text = name;
        rectTransform.anchoredPosition = position;
        button.onClick.AddListener(SetTeamName);
        clickEvent = onClicked;
    }

    private void SetTeamName()
    {
        teamName.value = buttonText.text;
        clickEvent.Raise();
    }
}
