using System.Collections.Generic;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///@info: this class is meant to be assigned to a Character GameObject. Assigned Item assets will be Instantiated
    ///into a separate list. This prevents the values on the assets from changing.
    ///</summary>
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Character _owner;
        public List<Item> battleInventory;

        [SerializeField] private List<Item> _assetInventory;

        [SerializeField] private int _capacity;

        [SerializeField] private Item _selectedItem;

        [SerializeField] private Item _emptySlotPlaceHolder;

        private void Awake()
        {
            battleInventory = new List<Item>(_capacity);
        }
        private void OnEnable()
        {
            _owner = GetComponent<Character>();
            GenerateCopyItems();
        }
        public bool AddItem(Item item)
        {
            if (battleInventory.Count < _capacity)
            {
                item = ScriptableObject.CreateInstance<Item>();
                battleInventory.Add(item);
                return true;
            }

            else
                return false;
        }
        public void RemoveItem(Item item)
        {
            if (item.itemUses <= 0)
            {
                battleInventory.Remove(item);
                var empty = Instantiate(_emptySlotPlaceHolder);
                battleInventory.Add(empty);
            }
        }
        public void UseItem(int itemSlot)
        {
            var item = battleInventory[itemSlot];
            item.UseItem(_owner.activeUnit);
        }
        /// <summary>
        /// This method creates copies of assigned items to prevent changes to asset values.
        /// </summary>
        public void GenerateCopyItems()
        {
            foreach(var item in _assetInventory)
            {
                var copy = Instantiate(item);
                battleInventory.Add(copy);
            }
        }
    }
}