using System;
using UnityEngine;

namespace Veganimus.BattleSystem
{
    public enum ItemType { Health, Equipment, Boost, Refill, NULL }
    public enum StatAffected { HP, Speed, Defense, Accuracy }
    [CreateAssetMenu(menuName = "Item/Empty")]
    public class Item : ScriptableObject, IComparable<Item>
    {
        ///<summary>
        ///@author
        ///Aaron Grincewicz
        ///</summary>
        ///
        private int _ItemID => (int)ItemType;
        private int _SecondID => (int)StatAffected;
        [Header("Item Settings")]
        [SerializeField] private string _itemName;
        public string ItemName { get { return _itemName; } }

        [SerializeField] private bool _isConsumable;
        public bool IsConsumable { get { return _isConsumable; } }

        [SerializeField] private uint _itemUses;
        public uint ItemUses { get { return _itemUses; } }

        [SerializeField] private ItemType _itemType;
        public ItemType ItemType { get { return _itemType; } }

        [SerializeField] private StatAffected _statAffected;
        public StatAffected StatAffected { get { return _statAffected; } }

        [SerializeField] private int _effectAmount;
        public int EffectAmount { get { return _effectAmount; } }

        [SerializeField] private Sprite _itemIcon;
        public Sprite ItemIcon { get { return _itemIcon; } }

        [SerializeField] private uint _itemCreditCost;
        public uint ItemCreditCost { get { return _itemCreditCost; } }

        [SerializeField] private AudioClip _itemSound;
        [SerializeField] private AudioClipChannel _audioClipChannel;

        public Item() { }

        public void UseItem(Unit unit)
        {
            if (ItemUses > 0)
            {
                _itemUses--;

                if (ItemType == ItemType.Health)
                {
                    var iHeal = unit.GetComponent<IHealable>();
                    iHeal.Heal(EffectAmount);
                }
                else if (ItemType == ItemType.Equipment)
                {
                    var iBuff = unit.GetComponent<IBuffable>();
                    iBuff.BuffStats(StatAffected, EffectAmount);
                }
                if(_itemSound != null)
                 _audioClipChannel.RaisePlayClipEvent(_itemSound);
            }
        }

        public void DeActivateItem(Unit unit)
        {
            if (ItemType == ItemType.Equipment)
            {
                var iBuff = unit.GetComponent<IBuffable>();
                iBuff.BuffStats(StatAffected, -EffectAmount);
            }
        }

        public int CompareTo(Item other)
        {
            if (this._ItemID < other._ItemID)
                return -1;

            else if (this._ItemID > other._ItemID)
                return 1;

            else if (_ItemID == other._ItemID)
            {
                if (_SecondID < other._SecondID)
                    return -1;

                else if (this._SecondID > other._SecondID)
                    return 1;

                else if (this._SecondID == other._SecondID)
                {
                    if (this.EffectAmount < other.EffectAmount)
                        return -1;

                    else if (this.EffectAmount > other.EffectAmount)
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
}