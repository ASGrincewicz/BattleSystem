using UnityEngine;
using Veganimus.BattleSystem;

public enum ItemType { Health, Equipment, Boost, Refill, NULL }
public enum StatAffected { HP, Speed, Defense, Accuracy}
[CreateAssetMenu(menuName ="Item/Empty")]
public class Item : ScriptableObject
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    ///
    
    [Header("Item Settings")]
    public string itemName;
    public bool isConsumable;
    public int itemUses;
    public ItemType itemType;
    public StatAffected statAffected;
    public int effectAmount;

    public virtual void UseItem(Unit unit)
    {
        if (itemUses > 0)
        {
            itemUses--;
       
            if (itemType == ItemType.Health)
            {
                var iHeal = unit.GetComponent<IHealable>();
                iHeal.Heal(effectAmount);
            }
            else if (itemType == ItemType.Equipment)
            {
                var iBuff = unit.GetComponent<IBuffable>();
                iBuff.BuffStats(statAffected, effectAmount);
            }
        }
    }

    public void DeActivateItem(Unit unit)
    {
        if (itemType == ItemType.Equipment)
        {
            var iBuff = unit.GetComponent<IBuffable>();
            iBuff.BuffStats(statAffected, -effectAmount);
        }
    }
}
