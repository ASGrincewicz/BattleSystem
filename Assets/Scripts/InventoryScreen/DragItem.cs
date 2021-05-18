using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour,IBeginDragHandler,IEndDragHandler, IDragHandler
{
    public Canvas canvas;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;
    public Item item;
    public Transform originalPos;
    public DropSpot previousSpot, newSpot;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }
   
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta/ canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = transform.parent;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
        previousSpot = GetComponentInParent<DropSpot>();
        previousSpot.ItemCount--;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        StartCoroutine(AddToCountDelayRoutine());
    }
    private IEnumerator AddToCountDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        newSpot = GetComponentInParent<DropSpot>();
        newSpot.ItemCount++;
    }
}
