using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyObject : MonoBehaviour
{
    public Team team;
    private Text text;
    private CanvasGroup alpha;

    void Start()
    {
        text = GetComponentInChildren<Text>();
        alpha = GetComponent<CanvasGroup>();
    }

    public void AssignTeam(Team team)
    {
        this.team = team;
        text.text = team.teamName;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        alpha = GetComponent<CanvasGroup>();

        for(float i = 0; i < 1; i += 0.1f)
        {
            alpha.alpha = i;
            yield return new WaitForSeconds(0.05f);
        }
        alpha.alpha = 1f;
    }

    public void Dissolve()
    {
        if (isActiveAndEnabled)
        {
            StartCoroutine(DissolveEnum());
        }
    }

    private IEnumerator DissolveEnum()
    {
        for (float i = 1; i > 0; i -= 0.1f)
        {
            alpha.alpha = i;
            yield return new WaitForSeconds(0.05f);
        }
        alpha.alpha = 0f;
        gameObject.SetActive(false);
    }
	
}
