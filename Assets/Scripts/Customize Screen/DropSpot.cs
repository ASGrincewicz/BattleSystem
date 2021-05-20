using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSpot : MonoBehaviour,IDropHandler, IPointerEnterHandler
{
    [SerializeField] private bool _isBattleInventory;
    [SerializeField] private int _itemCount;
    public int ItemCount { get { return _itemCount;} set { _itemCount = value; } }
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
            if (_isBattleInventory && _itemCount < _capacity)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(this.transform);
                InventoryManager.Instance.AddToBattleInventory(eventData.pointerDrag.GetComponent<DragItem>().item);
            }
            else if (!_isBattleInventory && _itemCount < _capacity)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(this.transform);
                InventoryManager.Instance.AddToInventory(eventData.pointerDrag.GetComponent<DragItem>().item);
            }
            else
            {
                eventData.pointerDrag.transform.SetParent(eventData.pointerDrag.GetComponent<DragItem>().originalPos);
            }
            _itemCount = transform.childCount;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _itemCount = transform.childCount;
    }
}
