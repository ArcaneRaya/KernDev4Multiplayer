using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VisualProgrammingActionUI : MonoBehaviour
{
    public VisualProgrammingAction action;

#if UNITY_EDITOR
    private int spawnCounter;
#endif

    private void Start()
    {
        DraggableUIItem draggableUIItem = GetComponent<DraggableUIItem>();
        draggableUIItem.onBeginDrag.AddListener(CreateDragCopy);
    }

    public void Setup(VisualProgrammingAction action)
    {
        this.action = action;
    }

    public void CreateDragCopy(PointerEventData eventData)
    {
        GameObject obj = Instantiate(action.UIRepresentation, transform.parent);
#if UNITY_EDITOR
        obj.name += spawnCounter;
        spawnCounter++;
#endif
        obj.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        obj.AddComponent<VisualProgrammingActionRepresentation>().Setup(action);
        eventData.pointerDrag = obj;
        obj.GetComponent<DraggableUIItem>().isDragged = true;
        obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
