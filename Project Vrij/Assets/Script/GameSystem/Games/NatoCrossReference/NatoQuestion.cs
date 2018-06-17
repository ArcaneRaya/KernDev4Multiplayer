using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NatoQuestion : MonoBehaviour
{
    public Nato natoCrossReference;
    public GameObject natoQuestionObject;
    public NatoQuestionText questionText;
    public GameObject answerButtonPrefab;
    public int distanceFromMiddle = 100;
    public Text preText;
    public Text postText;

    private NatoAnswerButton[] answerButtons;
    private string[] setAnswers;
    private NatoWord currentWord;

    private void Awake()
    {
        Cleanup();
    }

    public void Setup(NatoWord natoWord)
    {
        natoQuestionObject.SetActive(true);
        currentWord = natoWord;
        CreateButtons(natoWord.alternatives.Length + 1);
        questionText.Set(currentWord.word);
        SetButtons(currentWord);
        preText.text = natoWord.sentencePreWord;
        postText.text = natoWord.sentencePostWord;
    }

    public void Answer(string answer)
    {
        if (answer == currentWord.word)
        {
            Cleanup();
            currentWord.solved = true;
            natoCrossReference.WordCompleted();
            //throw new System.NotImplementedException();
            //            natoCrossReference.WordCompleted();
        }
        else
        {
            for (int i = 0; i < setAnswers.Length; i++)
            {
                if (setAnswers[i] == answer)
                {
                    answerButtons[i].Disable();
                    break;
                }
            }
        }
    }

    private void CreateButtons(int amount)
    {
        answerButtons = new NatoAnswerButton[amount];
        float offset = 600 / (amount - 1);
        for (int i = 0; i < amount; i++)
        {
            GameObject buttonObj = Instantiate(answerButtonPrefab, natoQuestionObject.transform);
            buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(distanceFromMiddle, -300 + offset * i);
            answerButtons[i] = buttonObj.GetComponent<NatoAnswerButton>();
        }
    }

    private void SetButtons(NatoWord natoWord)
    {
        List<string> possibleAnswers = new List<string>();
        possibleAnswers.Add(natoWord.word);
        for (int i = 0; i < answerButtons.Length - 1; i++)
        {
            if (natoWord.alternatives.Length <= i)
            {
                break;
            }
            possibleAnswers.Add(natoWord.alternatives[i]);
        }
        setAnswers = new string[possibleAnswers.Count];
        int itterator = 0;
        while (possibleAnswers.Count > 0)
        {
            int randomPosition = Random.Range(0, possibleAnswers.Count);
            setAnswers[itterator] = possibleAnswers[randomPosition];
            possibleAnswers.RemoveAt(randomPosition);
            itterator++;
        }
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < setAnswers.Length)
            {
                answerButtons[i].Set(setAnswers[i], this);
            }
            else
            {
                answerButtons[i].Show(false);
            }
        }
    }

    private void Cleanup()
    {
        if (answerButtons != null)
        {
            for (int i = answerButtons.Length - 1; i >= 0; i--)
            {
                Destroy(answerButtons[i].gameObject);
            }
        }
        natoQuestionObject.SetActive(false);
    }
}
