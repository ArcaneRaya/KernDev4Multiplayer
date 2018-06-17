using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IrisBotPopup : MonoBehaviour
{
    public Image img;
    public Image errorImg;

    public void SetPopupTexture(Sprite tex)
    {
        Debug.Log("SET CALLED");
        RectTransform tfi = img.gameObject.GetComponent<RectTransform>();
        RectTransform tfe = errorImg.gameObject.GetComponent<RectTransform>();
        tfi.localScale = new Vector2(0, 0);
        tfe.localScale = new Vector2(0, 0);
        StopAllCoroutines();
        StartCoroutine(AnimateTexture(tex));
    }

    private IEnumerator AnimateTexture(Sprite tex)
    {
        Debug.Log("fuck dit");
        Debug.Log(UnityEngine.StackTraceUtility.ExtractStackTrace());
        img.sprite = tex;
        RectTransform tf = img.gameObject.GetComponent<RectTransform>();
        for (float i = 0; i < 1; i += 0.05f)
        {
            tf.localScale = new Vector2(i, i);
            yield return new WaitForSeconds(0.025f);
        }

        tf.localScale = new Vector2(1, 1);
        for (float i = 1; i > 0; i -= 0.05f)
        {
            tf.localScale = new Vector2(i, i);
            yield return new WaitForSeconds(0.025f);
        }
        tf.localScale = new Vector2(0, 0);
    }

    public void SetErrorTexture(Sprite tex)
    {
        RectTransform tfi = img.gameObject.GetComponent<RectTransform>();
        RectTransform tfe = errorImg.gameObject.GetComponent<RectTransform>();
        tfi.localScale = new Vector2(0, 0);
        tfe.localScale = new Vector2(0, 0);
        StopAllCoroutines();
        StartCoroutine(AnimateErrorTexture(tex));
    }

    private IEnumerator AnimateErrorTexture(Sprite tex)
    {
        Debug.Log("fucking great");
        img.sprite = tex;
        RectTransform tf = img.gameObject.GetComponent<RectTransform>();
        RectTransform tfe = errorImg.gameObject.GetComponent<RectTransform>();
        for (float i = 0; i < 1; i += 0.05f)
        {
            tf.localScale = new Vector2(i, i);
            tfe.localScale = new Vector2(i, i);
            yield return new WaitForSeconds(0.025f);
        }
        tf.localScale = new Vector2(1, 1);
        tfe.localScale = new Vector2(1, 1);
        for (float i = 1; i > 0; i -= 0.05f)
        {
            tf.localScale = new Vector2(i, i);
            tfe.localScale = new Vector2(i, i);
            yield return new WaitForSeconds(0.025f);
        }
        tf.localScale = new Vector2(0, 0);
        tfe.localScale = new Vector2(0, 0);
    }
}
