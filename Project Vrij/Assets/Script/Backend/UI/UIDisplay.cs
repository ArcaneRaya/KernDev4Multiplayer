using UnityEngine;
using System.Collections;

public class UIDisplay : MonoBehaviour
{
    public CanvasGetter canvasGetter;
    public GameObject displayContainer;

    public virtual void DisplayImmediate(bool display)
    {
        StopAllCoroutines();
        displayContainer.SetActive(display);
    }

    public virtual void DisplayAnimated(bool display)
    {
        StopAllCoroutines();
        if (displayContainer.GetComponent<CanvasGroup>() == null)
        {
            displayContainer.AddComponent<CanvasGroup>();
        }
        StopAllCoroutines();
        StartCoroutine(Fade(display, displayContainer.GetComponent<CanvasGroup>()));
    }

    private IEnumerator Fade(bool display, CanvasGroup group)
    {
        if (display == true)
        {
            yield return new WaitForSeconds(0.5f);
            displayContainer.SetActive(display);
        }
        float target = (display == true) ? 1f : 0f;
        float start = (target == 1f) ? 0f : 1f;

        for (float i = 0; i < 1; i += 0.2f)
        {
            group.alpha = Mathf.Lerp(start, target, i);
            yield return new WaitForSeconds(0.01f);
        }
        group.alpha = target;
        displayContainer.SetActive(display);
    }
}