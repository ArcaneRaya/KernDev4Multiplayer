using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeginPointUI : MonoBehaviour
{
    public int id;
    public bool linked;
    private Transform spawnParent;
    private Transform parent;
    private Vector2 spawnPosition;
    private Vector2 snapPosition;
    private RectTransform rectTransform;
    private AnimatedUIMovement animatedUIMovement;

    // Use this for initialization
    void OnEnable()
    {
        linked = false;
        spawnParent = transform.parent;
        parent = spawnParent;
        rectTransform = GetComponent<RectTransform>();
        animatedUIMovement = GetComponent<AnimatedUIMovement>();
        spawnPosition = rectTransform.anchoredPosition;
        snapPosition = spawnPosition;
        GetComponent<DraggableUIItem>().onEndDrag.AddListener(OnEndDrag);
    }

    public void SetSnap(Transform parent, Vector2 position)
    {
        //        Debug.Log("set snap");
        linked = true;
        this.parent = parent;
        snapPosition = position;
        //    animatedUIMovement.SetPosition(position);
    }

    public void ResetSnap()
    {
        linked = false;
        parent = spawnParent;
        snapPosition = spawnPosition;
        animatedUIMovement.SetPosition(snapPosition);
    }

    private void OnDisable()
    {
        GetComponent<DraggableUIItem>().onEndDrag.RemoveListener(OnEndDrag);
    }

    private void OnEndDrag(PointerEventData evenData)
    {
        //        Debug.Log("dropped");
        transform.SetParent(parent);
        if (rectTransform.anchoredPosition != snapPosition)
        {
            animatedUIMovement.SetPosition(snapPosition);
        }
    }
}
