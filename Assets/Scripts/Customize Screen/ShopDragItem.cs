using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopDragItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;
    public Item item;
    public Transform originalPos;
    public ShopDropSpot previousSpot, newSpot;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = transform.parent;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
        previousSpot = GetComponentInParent<ShopDropSpot>();
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
        newSpot = GetComponentInParent<ShopDropSpot>();
        newSpot.ItemCount++;
    }
}
