using UnityEngine;
using Veganimus.BattleSystem;
using System;

public enum ItemType { Health, Equipment, Boost, Refill, NULL }
public enum StatAffected { HP, Speed, Defense, Accuracy}
[CreateAssetMenu(menuName ="Item/Empty")]
public class Item : ScriptableObject, IComparable<Item>
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    ///

    public int itemCreditCost;
    public int ItemID => (int)itemType;
    public int SecondID => (int)statAffected;
    [Header("Item Settings")]
    public string itemName;
    public bool isConsumable;
    public int itemUses;
    public ItemType itemType;
    public StatAffected statAffected;
    public int effectAmount;

    public Item() { }

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

    public int CompareTo(Item other)
    {
        if (this.ItemID < other.ItemID)
            return -1;

        else if (this.ItemID > other.ItemID)
            return 1;

        else if (ItemID == other.ItemID)
        {
            if (SecondID < other.SecondID)
                return -1;

            else if (this.SecondID > other.SecondID)
                return 1;

            else if (this.SecondID == other.SecondID)
            {
                if (this.effectAmount < other.effectAmount)
                    return -1;

                else if (this.effectAmount > other.effectAmount)
                    return 1;

                else
                    return 0;

            }
            else
                return 0;
               
        }
        else
            return 0;
    }
}
