using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EyeTexture { Normal, Cross}

public class IrisBotEyeTexture : MonoBehaviour
{
    public GameObject eye;
    public Texture2D normal;
    public Texture2D cross;

    public void SetTexture(EyeTexture tex)
    {
        if(tex == EyeTexture.Normal)
        {
            eye.gameObject.GetComponent<MeshRenderer>().material.mainTexture = normal;
        }

        else if (tex == EyeTexture.Cross)
        {
            eye.gameObject.GetComponent<MeshRenderer>().material.mainTexture = cross;
        }
    }
}
