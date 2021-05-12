using UnityEngine;

[CreateAssetMenu(menuName ="Item/Consumable/Health Item")]
public class HealthItem : Item
{
    public HealthItem(string itemName, bool isConsumable, int itemUses) { }
    public int healAmount;
}
