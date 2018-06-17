using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{

    Camera m_Camera;

    void Start()
    {
        GameObject camObject = GameObject.FindGameObjectWithTag("GameCamera");
        if (camObject == null)
        {
            camObject = GameObject.FindGameObjectWithTag("MainCamera");
        }
        if (camObject == null)
        {
            Debug.LogWarning("could not find game camera");
        }
        else
        {
            m_Camera = camObject.GetComponent<Camera>();
        }
    }

    void Update()
    {
        if (m_Camera == null)
        {
            return;
        }
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
                         m_Camera.transform.rotation * Vector3.up);
    }
}