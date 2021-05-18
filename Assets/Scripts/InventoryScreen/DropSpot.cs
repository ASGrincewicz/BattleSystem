using UnityEngine;
using UnityEngine.EventSystems;

public class DropSpot : MonoBehaviour,IDropHandler
{
    [SerializeField] private bool _isBattleInventory;
    [SerializeField] private int _itemCount;
    [SerializeField] private int _capacity;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.transform.SetParent(this.transform);

            if (_isBattleInventory && _itemCount < _capacity)
            {
                InventoryManager.Instance.AddToBattleInventory(eventData.pointerDrag.GetComponent<DragItem>().item);
            }
            else if (!_isBattleInventory && _itemCount < _capacity)
            {
                InventoryManager.Instance.AddToInventory(eventData.pointerDrag.GetComponent<DragItem>().item);
            }
            else
            {
                eventData.pointerDrag.transform.SetParent(eventData.pointerDrag.GetComponent<DragItem>().originalPos);
            }
            _itemCount = transform.childCount;
        }
    }
}
