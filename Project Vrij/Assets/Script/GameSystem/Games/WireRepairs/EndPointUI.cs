using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndPointUI : MonoBehaviour
{
    public delegate void WireRepairEvent();
    public WireRepairEvent PointChanged;

    public int id;
    public bool isCorrect;

    private void Start()
    {
        isCorrect = false;
        GetComponent<DraggableUIItem>().isDraggable = false;
        GetComponent<DraggableUIItem>().onDrop.AddListener(OnDrop);
    }

    private void OnDrop(PointerEventData eventData)
    {
        BeginPointUI beginPoint = eventData.pointerDrag.GetComponent<BeginPointUI>();
        if (beginPoint != null)
        {
            //            Debug.Log("dropped beginpoint");
            if (beginPoint.id == id)
            {
                isCorrect = true;
                beginPoint.SetSnap(transform.parent, GetComponent<RectTransform>().anchoredPosition);
                beginPoint.GetComponent<DraggableUIItem>().onEndDrag.AddListener(LostBeginPoint);
                FirePointChanged();
            }
        }
    }

    private void LostBeginPoint(PointerEventData eventData)
    {
        BeginPointUI beginPoint = eventData.pointerDrag.GetComponent<BeginPointUI>();
        if (beginPoint != null)
        {
            if (!beginPoint.linked)
            {
                isCorrect = false;
                beginPoint.GetComponent<DraggableUIItem>().onEndDrag.RemoveListener(LostBeginPoint);
                FirePointChanged();
            }
        }
        else
        {
            isCorrect = false;
            beginPoint.GetComponent<DraggableUIItem>().onEndDrag.RemoveListener(LostBeginPoint);
            FirePointChanged();
        }
    }

    private void FirePointChanged()
    {
        if (PointChanged != null)
        {
            PointChanged();
        }
    }
}
