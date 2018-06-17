using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeginpointContainer : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<DraggableUIItem>().onDrop.AddListener(OnDroppedUIElement);
    }

    void OnDisable()
    {
        GetComponent<DraggableUIItem>().onDrop.RemoveListener(OnDroppedUIElement);
    }

    private void OnDroppedUIElement(PointerEventData eventData)
    {
        BeginPointUI beginPointUI = eventData.pointerDrag.GetComponent<BeginPointUI>();
        if (beginPointUI != null)
        {
            beginPointUI.ResetSnap();
        }
    }
}
