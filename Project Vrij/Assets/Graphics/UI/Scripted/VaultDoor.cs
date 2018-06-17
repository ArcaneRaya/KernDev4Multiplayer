using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VaultDoor : MonoBehaviour
{
    public GameEvent doorOpened;
    public UnityEvent OnPressedSpace;
    private Animator anim;
    private Text text;
    private bool readyToStartServer = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        text = GetComponentInChildren<Text>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        //StartCoroutine(Fade());
    }

    public void Open()
    {
        //StopAllCoroutines();
        //text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        anim.SetTrigger("Open");
    }

    void Update()
    {
        if(readyToStartServer)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                OnPressedSpace.Invoke();
                StopAllCoroutines();
                text.gameObject.SetActive(false);
            }
        }
    }

    public void StartFade()
    {
        StartCoroutine(Fade());
        readyToStartServer = true;
    }

    private IEnumerator Fade()
    {
        for(float i = 0; i < 1; i+=0.05f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, i);
            yield return new WaitForSeconds(0.05f);
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        for (float i = 1; i > 0; i -= 0.05f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, i);
            yield return new WaitForSeconds(0.05f);
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        StartCoroutine(Fade());
    }

    public void Proceed()
    {
        doorOpened.Raise();
    }
}
