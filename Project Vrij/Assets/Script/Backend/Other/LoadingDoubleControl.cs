using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingDoubleControl : MonoBehaviour
{
    void Awake()
    {
        GameObject cam;
        if ((cam = GameObject.FindGameObjectWithTag("MainCamera")) != null)
        {
            cam.transform.position = gameObject.transform.position;
            cam.transform.localRotation = gameObject.transform.localRotation;
            cam.transform.localScale = gameObject.transform.localScale;

            Destroy(gameObject);
        }
    }
}
