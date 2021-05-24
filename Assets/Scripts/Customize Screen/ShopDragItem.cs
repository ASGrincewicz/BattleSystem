using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopDragItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public TMP_Text itemNameText;
    public Image itemIcon;
    [HideInInspector]
    public Item item;
    [HideInInspector]
    public Transform originalPos;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private ShopDropSpot _previousSpot, _newSpot;
   

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = transform.parent;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.5f;
        _previousSpot = GetComponentInParent<ShopDropSpot>();
        _previousSpot.ItemCount--;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
        _newSpot = GetComponentInParent<ShopDropSpot>();
        StartCoroutine(AddToCountDelayRoutine());
    }
    private IEnumerator AddToCountDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        if (_newSpot.ItemCount > _newSpot.Capacity)
        {
            transform.SetParent(originalPos);
        }
        else
        {
            _newSpot.ItemCount++;
        }
    }
}
