using UnityEngine;
using UnityEngine.EventSystems;
public class DropSpot : MonoBehaviour,IDropHandler
{
    [SerializeField] private bool isBattleInventory;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.transform.SetParent(this.transform);
            if(isBattleInventory)
            {
                InventoryManager.Instance.AddToBattleInventory(eventData.pointerDrag.GetComponent<DragItem>().item);
            }
            else if(!isBattleInventory)
            {
                InventoryManager.Instance.AddToInventory(eventData.pointerDrag.GetComponent<DragItem>().item);
            }
        }
    }
}
