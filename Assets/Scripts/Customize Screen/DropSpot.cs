using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSpot : MonoBehaviour,IDropHandler, IPointerEnterHandler
{
    [SerializeField] private bool _isBattleInventory;
    [SerializeField] private int _itemCount;
    public int ItemCount { get { return _itemCount;} set { _itemCount = value; } }
    [SerializeField] private int _capacity;
    public int Capacity { get { return _capacity; } }
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
                
                if (eventData.pointerDrag.transform.parent != eventData.pointerDrag.GetComponent<DragItem>().originalPos)
                {
                    InventoryManager.Instance.AddToBattleInventory(eventData.pointerDrag.GetComponent<DragItem>().item);
                }
                else
                   InventoryManager.Instance.RefreshGrids();
            }
            else if (!_isBattleInventory && _itemCount < _capacity)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(this.transform);
              
                if (eventData.pointerDrag.transform.parent != eventData.pointerDrag.GetComponent<DragItem>().originalPos)
                {
                    InventoryManager.Instance.AddToInventory(eventData.pointerDrag.GetComponent<DragItem>().item);
                }
                else
                    InventoryManager.Instance.RefreshGrids();
            }
            else if(_itemCount >= _capacity)
            {
                var orig = eventData.pointerDrag.GetComponent<DragItem>().originalPos;
                eventData.pointerDrag.transform.SetParent(orig);
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = orig.GetComponent<RectTransform>().anchoredPosition;
                InventoryManager.Instance.RefreshGrids();
            }
            _itemCount = transform.childCount;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _itemCount = transform.childCount;
    }
}
