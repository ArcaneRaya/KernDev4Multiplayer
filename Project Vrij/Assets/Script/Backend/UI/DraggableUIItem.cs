using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public class PointerEventDataEvent : UnityEvent<PointerEventData>
{
}

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class DraggableUIItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler, IDropHandler
{
    public UnityEvent onGrabbed;
    public UnityEvent onReleased;
    public PointerEventDataEvent onBeginDrag;
    public PointerEventDataEvent onEndDrag;
    public PointerEventDataEvent onDrop;
    public bool isDragged;
    public bool isDraggable = true;

    private RectTransform rectTransform;
    private UnityEngine.UI.CanvasScaler canvasScaler;
    private float RealFactor
    {
        get
        {
            float realFactorWidth = Screen.width / canvasScaler.referenceResolution.x;
            float realFactorHeight = Screen.height / canvasScaler.referenceResolution.y;
            return Mathf.Max(realFactorWidth, realFactorHeight);
        }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasScaler = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<UnityEngine.UI.CanvasScaler>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragged)
        {
            rectTransform.anchoredPosition += eventData.delta / RealFactor;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onGrabbed.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //        Debug.Log("released on " + gameObject.name);
        onReleased.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (isDragged)
        {
            isDragged = false;
            onEndDrag.Invoke(eventData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (isDraggable)
        {
            isDragged = true;
            onBeginDrag.Invoke(eventData);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        onDrop.Invoke(eventData);
    }
}
