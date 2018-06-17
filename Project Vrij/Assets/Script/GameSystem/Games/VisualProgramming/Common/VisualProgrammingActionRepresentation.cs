using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VisualProgrammingActionRepresentation : MonoBehaviour
{
    public VisualProgrammingAction action;
    public bool highlight;
    public bool parented;
    public bool isLocked;

    public void Setup(VisualProgrammingAction action)
    {
        this.action = action;
        GetComponent<DraggableUIItem>().onEndDrag.AddListener(OnEndDrag);
    }

    private void OnEndDrag(PointerEventData obj)
    {
        Debug.Log("drag ended");
        GetComponent<AnimatedUIMovement>().SetPositionImmediate(GetComponent<RectTransform>().anchoredPosition);
    }

    private void Update()
    {
        if (!GetComponent<DraggableUIItem>().isDragged)
        {
            if (!parented)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Highlight(bool highlight)
    {
        this.highlight = highlight;
    }
}
