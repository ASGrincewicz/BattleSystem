using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopDropSpot : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    [SerializeField] private bool _isCustomerInventory;
    [SerializeField] private int _itemCount;
    public int ItemCount { get { return _itemCount; } set { _itemCount = value; } }
    [SerializeField] private int _capacity;
    public int Capacity { get { return _capacity; } }
    private ShopManager ShopManager;
    private CharacterStats Customer;

    private IEnumerator Start()
    {
        ShopManager = ShopManager.Instance;
        Customer = ShopManager.Instance.Customer;
        yield return new WaitForSeconds(2f);
        _itemCount = transform.childCount;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Customer = ShopManager.Instance.Customer;
            var item = eventData.pointerDrag.GetComponent<ShopDragItem>().item;

            if (_isCustomerInventory && _itemCount < _capacity && Customer.characterCredits >= item.itemCreditCost)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(this.transform);
                if (eventData.pointerDrag.transform.parent != eventData.pointerDrag.GetComponent<ShopDragItem>().originalPos)
                {
                    ShopManager.BuyItem(eventData.pointerDrag.GetComponent<ShopDragItem>().item);
                }
                else
                   ShopManager.RefreshGrids();
            }
            else if (!_isCustomerInventory && _itemCount < _capacity && ShopManager.ShopCreditsAmount >= item.itemCreditCost)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(this.transform);
                if (eventData.pointerDrag.transform.parent != eventData.pointerDrag.GetComponent<ShopDragItem>().originalPos)
                {
                    ShopManager.SellItem(eventData.pointerDrag.GetComponent<ShopDragItem>().item);
                }
                else
                    ShopManager.RefreshGrids();
            }
            else if (_isCustomerInventory && _itemCount >= _capacity || item.itemCreditCost > Customer.characterCredits)
            {
                var orig = eventData.pointerDrag.GetComponent<ShopDragItem>().originalPos;
                eventData.pointerDrag.transform.SetParent(orig);
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = orig.GetComponent<RectTransform>().anchoredPosition;
                ShopManager.RefreshGrids();
            }
            else if(!_isCustomerInventory && item.itemCreditCost > ShopManager.ShopCreditsAmount)
            {
                var orig = eventData.pointerDrag.GetComponent<ShopDragItem>().originalPos;
                eventData.pointerDrag.transform.SetParent(orig);
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = orig.GetComponent<RectTransform>().anchoredPosition;
                ShopManager.RefreshGrids();
            }
            _itemCount = transform.childCount;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _itemCount = transform.childCount;
    }
}
