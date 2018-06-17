using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class LoopUIHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float componentWidth;
    public List<VisualProgrammingActionRepresentation> linkedActions;
    private RectTransform rectTransform;
    private bool previousFrameDragged;
    private bool isSpawnpoint;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        GetComponent<DraggableUIItem>().onDrop.AddListener(OnDrop);
        linkedActions = new List<VisualProgrammingActionRepresentation>();
    }

    private void Start()
    {
        isSpawnpoint = GetComponent<VisualProgrammingActionUI>() != null;
        Debug.Log(isSpawnpoint);
        if (!isSpawnpoint)
        {
            UpdateDisplay();
        }
    }

    private void Update()
    {
        if (isSpawnpoint)
        {
            return;
        }
        foreach (var representation in linkedActions)
        {
            if (representation.GetComponent<DraggableUIItem>().isDragged)
            {
                previousFrameDragged = true;
                SortActions();
                UpdateDisplay();
                break;
            }
            else if (previousFrameDragged)
            {
                previousFrameDragged = false;
                SortActions();
                UpdateDisplay();
                break;
            }
        }
    }

    private void OnDrop(PointerEventData eventData)
    {
        if (isSpawnpoint)
        {
            return;
        }
        if (eventData.pointerDrag == null)
        {
            return;
        }
        if (eventData.pointerDrag == gameObject)
        {
            return;
        }
        //Debug.Log(eventData.pointerDrag + " dropped");
        VisualProgrammingActionRepresentation representation;
        if ((representation = eventData.pointerDrag.GetComponent<VisualProgrammingActionRepresentation>()) != null)
        {
            representation.isLocked = true;
            representation.transform.SetParent(transform);
            if (!linkedActions.Contains(representation))
            {
                linkedActions.Add(representation);
            }
            SortActions();
            UpdateDisplay();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSpawnpoint)
        {
            return;
        }
        if (eventData.pointerDrag == null)
        {
            return;
        }
        if (eventData.pointerDrag == gameObject)
        {
            return;
        }
        //Debug.Log(eventData.pointerDrag + " entered");
        VisualProgrammingActionRepresentation representation;
        if ((representation = eventData.pointerDrag.GetComponent<VisualProgrammingActionRepresentation>()) != null)
        {
            representation.isLocked = true;
            representation.transform.SetParent(transform);
            if (!linkedActions.Contains(representation))
            {
                linkedActions.Add(representation);
            }
            SortActions();
            UpdateDisplay();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSpawnpoint)
        {
            return;
        }
        if (eventData.pointerDrag == null)
        {
            return;
        }
        if (eventData.pointerDrag == gameObject)
        {
            return;
        }
        //Debug.Log(eventData.pointerDrag + " left");
        VisualProgrammingActionRepresentation representation;
        if ((representation = eventData.pointerDrag.GetComponent<VisualProgrammingActionRepresentation>()) != null)
        {
            representation.transform.SetParent(transform.parent);
            if (!GetComponent<VisualProgrammingActionRepresentation>().isLocked)
            {
                representation.isLocked = false;
            }
            else
            {
                //Debug.Log("transfered to parent loop");
                if (!transform.parent.GetComponent<LoopUIHelper>().linkedActions.Contains(representation))
                {
                    transform.parent.GetComponent<LoopUIHelper>().linkedActions.Add(representation);
                }
                transform.parent.GetComponent<LoopUIHelper>().SortActions();
                transform.parent.GetComponent<LoopUIHelper>().UpdateDisplay();
            }
            if (linkedActions.Contains(representation))
            {
                linkedActions.Remove(representation);
            }
            SortActions();
            UpdateDisplay();
        }
    }

    private void SortActions()
    {
        if (isSpawnpoint)
        {
            return;
        }
        linkedActions.Sort((a, b) =>
        {
            float objectAX = a.GetComponent<AnimatedUIMovement>().TargetPosition.x;
            float objectBX = b.GetComponent<AnimatedUIMovement>().TargetPosition.x;
            if (!a.GetComponent<DraggableUIItem>().isDragged)
            {
                objectAX += componentWidth / 2;
            }
            if (!b.GetComponent<DraggableUIItem>().isDragged)
            {
                objectBX += componentWidth / 2;
            }
            if (a.GetComponent<DraggableUIItem>().isDragged)
            {
                if (objectAX < objectBX)
                {
                    objectBX -= componentWidth;
                }
            }
            if (b.GetComponent<DraggableUIItem>().isDragged)
            {
                if (objectBX > objectAX)
                {
                    objectAX -= componentWidth;
                }
            }
            return objectAX.CompareTo(objectBX);
        });
    }

    private void UpdateDisplay()
    {
        if (isSpawnpoint)
        {
            return;
        }
        float xOffset = componentWidth / 2;
        for (int i = linkedActions.Count - 1; i >= 0; i--)
        {
            // TODO not sure if neccesary
            //if (Mathf.Abs(linkedActions[i].GetComponent<RectTransform>().anchoredPosition.y) > componentWidth && !linkedActions[i].GetComponent<DraggableUIItem>().isDragged)
            //{
            //    linkedActions[i].isLocked = false;
            //    linkedActions.RemoveAt(i);
            //}
            //else 
            if (linkedActions[i].transform.parent != transform)
            {
                if (linkedActions[i].transform.parent.GetComponent<LoopUIHelper>() == null)
                {
                    linkedActions[i].isLocked = false;
                }
                linkedActions.RemoveAt(i);
            }
            else if (linkedActions[i].GetComponent<RectTransform>().anchoredPosition.x + componentWidth / 2 > rectTransform.sizeDelta.x)
            {
                linkedActions[i].transform.SetParent(transform.parent);
                if (!GetComponent<VisualProgrammingActionRepresentation>().isLocked)
                {
                    linkedActions[i].isLocked = false;
                }
                else
                {
                    //Debug.Log("transfered to parent loop");
                    if (!transform.parent.GetComponent<LoopUIHelper>().linkedActions.Contains(linkedActions[i]))
                    {
                        transform.parent.GetComponent<LoopUIHelper>().linkedActions.Add(linkedActions[i]);
                    }
                    transform.parent.GetComponent<LoopUIHelper>().SortActions();
                    transform.parent.GetComponent<LoopUIHelper>().UpdateDisplay();
                }
                if (linkedActions.Contains(linkedActions[i]))
                {
                    linkedActions.Remove(linkedActions[i]);
                }
            }
        }
        foreach (var representation in linkedActions)
        {
            if (!representation.GetComponent<DraggableUIItem>().isDragged)
            {
                representation.gameObject.GetComponent<AnimatedUIMovement>().SetPosition(new Vector2(xOffset, 0));
            }
            else
            {
                previousFrameDragged = true;
            }
            if (representation.transform.parent == transform)
            {
                xOffset += representation.GetComponent<RectTransform>().sizeDelta.x;
            }
        }
        xOffset += componentWidth;
        xOffset += componentWidth / 2;
        rectTransform.sizeDelta = new Vector2(xOffset, componentWidth);
    }
}
