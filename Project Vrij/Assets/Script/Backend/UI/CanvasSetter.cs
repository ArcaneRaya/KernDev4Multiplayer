using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSetter : MonoBehaviour
{
    public CanvasManager canvasManager;
    public CanvasLayers canvasLayer;

    private void OnEnable()
    {
        canvasManager.SetCanvas(canvasLayer, gameObject);
    }

    private void OnDisable()
    {
        canvasManager.RemoveCanvas(gameObject);
    }
}
