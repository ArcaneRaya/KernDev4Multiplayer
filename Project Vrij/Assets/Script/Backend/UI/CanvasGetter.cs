using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CanvasGetter
{
    public CanvasManager canvasManager;
    public CanvasLayers canvasLayer;

    public GameObject Canvas
    {
        get
        {
            if (canvasManager == null)
            {
                throw new System.NullReferenceException("CanvasManager not set");
            }
            return canvasManager.GetCanvas(canvasLayer);
        }
    }
}
