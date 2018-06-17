using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CanvasLayers
{
    MAIN,
    GAMEFRONT,
    GAMEMID,
    GAMEBACK,
    UIOVERLAY
}

[CreateAssetMenu(fileName = "NewCanvasManager", menuName = "Backend/Canvas Manager", order = 50)]
public class CanvasManager : ScriptableObject
{
    public static readonly Vector2 DefaultCanvasSize = new Vector2(2048, 1536);

    public GameObject canvasMain;
    public GameObject canvasGameFront;
    public GameObject canvasGameMid;
    public GameObject canvasGameBack;
    public GameObject canvasUIOverlay;

    private void OnDisable()
    {
        canvasMain = null;
        canvasGameFront = null;
        canvasGameMid = null;
        canvasGameBack = null;
        canvasUIOverlay = null;
    }

    public void SetCanvas(CanvasLayers layer, GameObject canvas)
    {
        if (!canvas.GetComponent<RectTransform>())
        {
            throw new System.ArgumentException("Gameobject provided does not contain RectTransform, invalid");
        }

        switch (layer)
        {
            case CanvasLayers.MAIN:
                if (canvasMain != null)
                {
                    Debug.LogWarning("GAMEFRONT canvas is reassigned, are there multiple instances set to the same layer?");
                }
                if (canvas.GetComponent<Canvas>())
                {
                    canvasMain = canvas;
                }
                else
                {
                    throw new System.ArgumentException("The main canvas needs to have a canvas component attached to it");
                }
                break;
            case CanvasLayers.GAMEFRONT:
                if (canvasGameFront != null)
                {
                    Debug.LogWarning("GAMEFRONT canvas is reassigned, are there multiple instances set to the same layer?");
                }
                canvasGameFront = canvas;
                break;
            case CanvasLayers.GAMEMID:
                if (canvasGameMid != null)
                {
                    Debug.LogWarning("GAMEMID canvas is reassigned, are there multiple instances set to the same layer?");
                }
                canvasGameMid = canvas;
                break;
            case CanvasLayers.GAMEBACK:
                if (canvasGameBack != null)
                {
                    Debug.LogWarning("GAMEBACK canvas is reassigned, are there multiple instances set to the same layer?");
                }
                canvasGameBack = canvas;
                break;
            case CanvasLayers.UIOVERLAY:
                if (canvasUIOverlay != null)
                {
                    Debug.LogWarning("UIOVERLAY canvas is reassigned, are there multiple instances set to the same layer?");
                }
                canvasUIOverlay = canvas;
                break;
            default:
                break;
        }
    }

    public void RemoveCanvas(GameObject canvas)
    {
        if (canvasMain == canvas)
        {
            canvasMain = null;
        }
        if (canvasGameBack == canvas)
        {
            canvasGameBack = null;
        }
        if (canvasGameMid == canvas)
        {
            canvasGameMid = null;
        }
        if (canvasGameFront == canvas)
        {
            canvasGameFront = null;
        }
        if (canvasUIOverlay == canvas)
        {
            canvasUIOverlay = null;
        }
    }

    public GameObject GetCanvas(CanvasLayers layer)
    {
        switch (layer)
        {
            case CanvasLayers.MAIN:
                return canvasMain;
            case CanvasLayers.GAMEFRONT:
                return canvasGameFront;
            case CanvasLayers.GAMEMID:
                return canvasGameMid;
            case CanvasLayers.GAMEBACK:
                return canvasGameBack;
            case CanvasLayers.UIOVERLAY:
                return canvasUIOverlay;
            default:
                return null;
        }
    }
}
