using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AnimatedUIMovement : MonoBehaviour
{
    public Vector2 TargetPosition
    {
        get
        {
            DraggableUIItem draggableUIItem = GetComponent<DraggableUIItem>();
            if (draggableUIItem != null)
            {
                if (draggableUIItem.isDragged)
                {
                    return rectTransform.anchoredPosition;
                }
            }
            return targetPosition;
        }
    }

    public float maxSpeed = 400;
    public float maxAnimationTime = 0.3f;
    private float requiredSpeed;
    private RectTransform rectTransform;
    private Vector2 targetPosition;
    private bool onTarget;

    private void Awake()
    {
        onTarget = true;
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetPositionImmediate(Vector2 position)
    {
        targetPosition = position;
        rectTransform.anchoredPosition = position;
        onTarget = true;
    }

    public void SetPosition(Vector2 position)
    {
        targetPosition = position;
        requiredSpeed = Vector2.Distance(rectTransform.anchoredPosition, targetPosition) / maxAnimationTime;
        onTarget = false;
    }

    private void Update()
    {
        if (!onTarget)
        {
            Vector2 direction = targetPosition - rectTransform.anchoredPosition;
            Vector2 newPosition = rectTransform.anchoredPosition + direction.normalized * Time.deltaTime * Mathf.Min(maxSpeed, requiredSpeed);
            if (Vector2.Distance(rectTransform.anchoredPosition, newPosition) < Vector2.Distance(rectTransform.anchoredPosition, targetPosition))
            {
                Vector2 diff = rectTransform.anchoredPosition - newPosition;
                for (int i = 0; i < transform.childCount; i++)
                {
                    DraggableUIItem draggable = transform.GetChild(i).GetComponent<DraggableUIItem>();
                    if (draggable != null)
                    {
                        if (draggable.isDragged)
                        {
                            draggable.GetComponent<RectTransform>().anchoredPosition += diff;
                        }
                    }
                }
                rectTransform.anchoredPosition = newPosition;
            }
            else
            {
                Vector2 diff = rectTransform.anchoredPosition - targetPosition;
                for (int i = 0; i < transform.childCount; i++)
                {
                    DraggableUIItem draggable = transform.GetChild(i).GetComponent<DraggableUIItem>();
                    if (draggable != null)
                    {
                        if (draggable.isDragged)
                        {
                            draggable.GetComponent<RectTransform>().anchoredPosition += diff;
                        }
                    }
                }
                rectTransform.anchoredPosition = targetPosition;
                onTarget = true;
            }
        }
    }
}
