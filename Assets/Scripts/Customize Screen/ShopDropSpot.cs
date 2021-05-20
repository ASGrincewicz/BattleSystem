using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopDropSpot : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    [SerializeField] private bool _isCustomerInventory;
    [SerializeField] private int _itemCount;
    public int ItemCount { get { return _itemCount; } set { _itemCount = value; } }
    [SerializeField] private int _capacity;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        _itemCount = transform.childCount;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
           
            var item = eventData.pointerDrag.GetComponent<ShopDragItem>().item;

            if (_isCustomerInventory && _itemCount < _capacity && ShopManager.Instance.Customer.characterCredits >= item.itemCreditCost)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(this.transform);
                ShopManager.Instance.BuyItem(eventData.pointerDrag.GetComponent<ShopDragItem>().item);
            }
            else if (!_isCustomerInventory && _itemCount < _capacity && ShopManager.Instance.ShopCreditsAmount >= item.itemCreditCost)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(this.transform);
                ShopManager.Instance.SellItem(eventData.pointerDrag.GetComponent<ShopDragItem>().item);
            }
            else
            {
                eventData.pointerDrag.transform.SetParent(eventData.pointerDrag.GetComponent<ShopDragItem>().originalPos);
            }
            _itemCount = transform.childCount;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _itemCount = transform.childCount;
    }
}
