using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatoQuestionSelection : MonoBehaviour
{
    public Nato natoCrossReference;
    public GameObject natoQuestionSelectionObject;
    public GameObject selectionObjectPrefab;
    public float fieldWidth = 1000;
    public float fieldHeight = 600;
    public int itemsPerRow = 4;
    private NatoWord[] natoWords;
    private NatoSelectionButton[] natoSelectionButtons;

    private void Awake()
    {
        Cleanup();
    }

    public void Setup(NatoWord[] natoWords)
    {
        this.natoWords = natoWords;
        Show(true);
        SetupButtons();
        UpdateDisplay();
    }

    public void SetupButtons()
    {
        Vector2 startPos;
        float buttonOffsetX;
        float rowOffsetY;
        if (natoWords.Length <= itemsPerRow)
        {
            startPos = new Vector2(-(fieldWidth / 2), 0);
            buttonOffsetX = fieldWidth / (natoWords.Length - 1);
            rowOffsetY = 0;
        }
        else
        {
            buttonOffsetX = fieldWidth / (itemsPerRow - 1);
            int rowAmount = Mathf.CeilToInt(natoWords.Length / (float)itemsPerRow);
            rowOffsetY = fieldHeight / rowAmount;
            startPos = new Vector2(-(fieldWidth / 2), rowOffsetY * rowAmount / 2.0f);
        }
        natoSelectionButtons = new NatoSelectionButton[natoWords.Length];
        Vector2 currentPos = startPos;
        for (int i = 0; i < natoWords.Length; i++)
        {
            GameObject selectionButton = Instantiate(selectionObjectPrefab, natoQuestionSelectionObject.transform);
            selectionButton.name += i.ToString();
            selectionButton.GetComponent<RectTransform>().anchoredPosition = currentPos;
            natoSelectionButtons[i] = selectionButton.GetComponent<NatoSelectionButton>();
            if ((i + 1) % itemsPerRow == 0)
            {
                Debug.Log(rowOffsetY);
                Debug.Log(startPos);
                Debug.Log(currentPos);
                currentPos = new Vector2(startPos.x, currentPos.y - rowOffsetY);
            }
            else
            {
                currentPos += new Vector2(buttonOffsetX, 0);
            }
        }
    }

    public void UpdateDisplay()
    {
        //for (int i = 0; i < natoWords.Length; i++)
        //{
        //    if (!natoWords[i].puzzle || natoWords[i].solved)
        //    {
        //        natoSelectionButtons[i].Set(natoWords[i].word, null, i);
        //    }
        //    else
        //    {
        //        throw new System.NotImplementedException();
        //        //natoSelectionButtons[i].Set(
        //        //NatoDictionary.ProvideSentence(natoWords[i].word),
        //        //natoCrossReference.WordSelected,
        //        //i);
        //    }
        //}
    }

    private void Cleanup()
    {
        natoQuestionSelectionObject.SetActive(false);
    }

    internal void Show(bool show)
    {
        natoQuestionSelectionObject.SetActive(show);
    }
}
