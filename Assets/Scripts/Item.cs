using UnityEngine;
using Veganimus.BattleSystem;

[CreateAssetMenu(menuName ="Item/Empty")]
public class Item : ScriptableObject
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    ///
    
    public string itemName;
   
    public bool isConsumable;
    
    public int itemUses;

    public enum ItemType { Health, Equipment, Boost, Refill, NULL}
    [SerializeField] private ItemType _thisItemType;
    public ItemType ThisItemType { get { return _thisItemType; } }

    public int healAmount;
    public virtual void UseItem(Unit unit)
    {
        itemUses--;
        if (itemUses <= 0)
            return;
    }
}
