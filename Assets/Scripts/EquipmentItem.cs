using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable/Equipment Item")]
public class EquipmentItem : Item
{
    public EquipmentItem(string itemName, bool isConsumable, int itemUses) { }
    public int statEffect;
}