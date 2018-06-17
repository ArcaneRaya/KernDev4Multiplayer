using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisBotPanelControl : MonoBehaviour
{

    public Animator panelAnimator;
    public bool eyeOpen;

    private float timeSinceChange;
    private bool eyeClosed;

    // Use this for initialization
    void Start()
    {
        panelAnimator = GetComponent<Animator>();
        eyeClosed = true;
    }

    private void Update()
    {
        if (eyeOpen == eyeClosed)
        {
            ChangeEyeState(eyeOpen);
        }
    }

    public void ChangeEyeState(bool open)
    {
        if (eyeClosed && open)
        {
            panelAnimator.SetBool("OpenEye", true);
            eyeClosed = false;
        }
        else if (!eyeClosed && !open)
        {
            panelAnimator.SetBool("OpenEye", false);
            eyeClosed = true;
        }
    }
}
