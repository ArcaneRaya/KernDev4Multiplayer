using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisBotBlinkHelper : MonoBehaviour
{
    public GameObject eye;
    public bool blink;
    public float speed = 1;

    private enum BlinkDirection
    {
        OPENING,
        CLOSING
    }

    private IrisBotPanelControl panelControl;
    //private float startPosEye;
    private float lerpPos;
    private BlinkDirection direction;
    private bool isBlinking;

    private void Start()
    {
        panelControl = GetComponent<IrisBotPanelControl>();
        //startPosEye = eye.transform.localPosition.z;
        direction = BlinkDirection.OPENING;
        eye.transform.localScale = new Vector3(1, 1, 0.4f);
        lerpPos = 0;
        isBlinking = true;
        panelControl.eyeOpen = true;
    }

    private void Update()
    {
        if (blink)
        {
            blink = false;
            isBlinking = true;
            direction = BlinkDirection.CLOSING;
            panelControl.eyeOpen = false;
        }

        if (isBlinking)
        {
            if (direction == BlinkDirection.CLOSING)
            {
                lerpPos -= Time.deltaTime * speed;
                if (lerpPos < 0)
                {
                    lerpPos = 0;
                    direction = BlinkDirection.OPENING;
                    panelControl.eyeOpen = true;
                }
            }
            else
            {
                lerpPos += Time.deltaTime * speed;
                if (lerpPos > 1)
                {
                    lerpPos = 1;
                    isBlinking = false;
                }
            }
            eye.transform.localScale = new Vector3(1, 1, Mathf.Lerp(0.4f, 1, lerpPos));
        }
    }
}
