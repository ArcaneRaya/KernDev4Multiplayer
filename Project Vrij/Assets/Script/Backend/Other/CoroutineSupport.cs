using UnityEngine;
using System.Collections;

public class CoroutineSupport : MonoBehaviour
{
    public static CoroutineSupport CoroutineComponent
    {
        get
        {
            if (coroutineComponent == null)
            {
                GameObject obj = new GameObject("CoroutineSupport");
                coroutineComponent = obj.AddComponent<CoroutineSupport>();
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(obj);
                }
            }
            return coroutineComponent;
        }
    }
    private static CoroutineSupport coroutineComponent;
}
