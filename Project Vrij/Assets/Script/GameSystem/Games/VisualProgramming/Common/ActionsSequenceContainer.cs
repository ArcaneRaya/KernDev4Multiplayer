using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionsSequenceContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public IrisBotProgrammable linkedBot;
    public List<VisualProgrammingActionRepresentation> actionRepresentations;
    public GameObject content;
    public float actionWidth = 100;
    public float actionSpacing = 20;
    public bool previousFrameDragged;

    private void Update()
    {
        if (linkedBot == null)
        {
            return;
        }
        foreach (var representation in actionRepresentations)
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

    private void SortActions()
    {
        actionRepresentations.Sort((a, b) =>
        {

            float objectAX = a.GetComponent<AnimatedUIMovement>().TargetPosition.x;
            float objectBX = b.GetComponent<AnimatedUIMovement>().TargetPosition.x;
            if (!a.GetComponent<DraggableUIItem>().isDragged)
            {
                objectAX += actionWidth / 2;
            }
            if (!b.GetComponent<DraggableUIItem>().isDragged)
            {
                objectBX += actionWidth / 2;
            }
            if (a.GetComponent<DraggableUIItem>().isDragged)
            {
                if (objectAX < objectBX)
                {
                    objectBX -= actionWidth;
                }
            }
            if (b.GetComponent<DraggableUIItem>().isDragged)
            {
                if (objectBX > objectAX)
                {
                    objectAX -= actionWidth;
                }
            }
            if (a.isLocked)
            {
                objectAX += a.transform.parent.GetComponent<RectTransform>().anchoredPosition.x;
            }
            if (b.isLocked)
            {
                objectBX += b.transform.parent.GetComponent<RectTransform>().anchoredPosition.x;
            }
            return objectAX.CompareTo(objectBX);
        });
    }

    public void Setup(IrisBotProgrammable irisBot)
    {
        linkedBot = irisBot;
        actionRepresentations = new List<VisualProgrammingActionRepresentation>();
        float xOffset = 0;
        foreach (var action in linkedBot.actions)
        {
            GameObject representationObject = Instantiate(action.UIRepresentation, content.transform);
            VisualProgrammingActionRepresentation representation = representationObject.AddComponent<VisualProgrammingActionRepresentation>();
            representation.Setup(action);
            representation.parented = true;
            representation.GetComponent<AnimatedUIMovement>().SetPositionImmediate(new Vector2(xOffset, 0));
            actionRepresentations.Add(representation);
            xOffset += actionWidth + actionSpacing;
        }
        UpdateDisplay();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (linkedBot == null)
        {
            return;
        }
        if (linkedBot.isPlaying)
        {
            return;
        }
        VisualProgrammingActionRepresentation representation;
        if ((representation = eventData.pointerDrag.GetComponent<VisualProgrammingActionRepresentation>()) != null)
        {
            representation.transform.SetParent(content.transform);
            representation.parented = true;
            if (!actionRepresentations.Contains(representation))
            {
                actionRepresentations.Add(representation);
                SortActions();
            }
            UpdateDisplay();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (linkedBot == null)
        {
            return;
        }
        if (linkedBot.isPlaying)
        {
            return;
        }
        if (eventData.dragging)
        {
            Debug.Log("entered");
            VisualProgrammingActionRepresentation representation;
            if ((representation = eventData.pointerDrag.GetComponent<VisualProgrammingActionRepresentation>()) != null)
            {
                representation.transform.SetParent(content.transform);
                representation.parented = true;
                previousFrameDragged = true;
                if (!actionRepresentations.Contains(representation))
                {
                    actionRepresentations.Add(representation);
                    SortActions();
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (linkedBot == null)
        {
            return;
        }
        if (linkedBot.isPlaying)
        {
            return;
        }
        if (eventData.dragging)
        {
            Debug.Log("left");
            VisualProgrammingActionRepresentation representation;
            if ((representation = eventData.pointerDrag.GetComponent<VisualProgrammingActionRepresentation>()) != null)
            {
                previousFrameDragged = true;
                if (actionRepresentations.Contains(representation))
                {
                    RemoveAction(representation);
                }
                UpdateDisplay();
            }
        }
    }

    private void AddRepresentation(VisualProgrammingActionRepresentation representation, float xPosition)
    {
        if (linkedBot == null)
        {
            return;
        }
        float xOffset = 0;
        for (int i = 0; i < actionRepresentations.Count; i++)
        {
            xOffset += actionWidth / 2;
            if (xOffset > xPosition)
            {
                actionRepresentations.Insert(i, representation);
                xOffset = -1;
                break;
            }
            xOffset += actionWidth / 2;
            xOffset += actionSpacing;
        }
        if (xOffset > -1)
        {
            actionRepresentations.Add(representation);
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        float xOffset = 0;
        for (int i = actionRepresentations.Count - 1; i >= 0; i--)
        {
            if (Mathf.Abs(actionRepresentations[i].GetComponent<RectTransform>().anchoredPosition.y) > actionWidth * 2 && !actionRepresentations[i].GetComponent<DraggableUIItem>().isDragged)
            {
                Debug.Log("outside");
                RemoveAction(actionRepresentations[i]);
            }
        }
        foreach (var representation in actionRepresentations)
        {
            if (!representation.GetComponent<DraggableUIItem>().isDragged)
            {
                if (!representation.isLocked)
                {
                    representation.gameObject.GetComponent<AnimatedUIMovement>().SetPosition(new Vector2(xOffset, 0));
                }
            }
            else
            {
                previousFrameDragged = true;
            }
            if (!representation.isLocked)
            {
                xOffset += representation.GetComponent<RectTransform>().sizeDelta.x;
            }
        }
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(xOffset + actionWidth + actionSpacing, content.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void RemoveAction(VisualProgrammingActionRepresentation representation)
    {
        VisualProgrammingActionRepresentation childRep;
        for (int i = representation.transform.childCount - 1; i >= 0; i--)
        {
            if ((childRep = representation.transform.GetChild(i).GetComponent<VisualProgrammingActionRepresentation>()) != null)
            {
                if (actionRepresentations.Contains(childRep))
                {
                    RemoveAction(childRep);
                }
            }
        }
        representation.parented = false;
        actionRepresentations.Remove(representation);
    }
}
