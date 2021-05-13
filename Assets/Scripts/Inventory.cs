using System.Collections.Generic;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Character _owner;

        public List<Item> battleInventory;

        [SerializeField] private List<Item> _assetInventory;

        [SerializeField] private int _capacity;

        [SerializeField] private Item _selectedItem;

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
            battleInventory.Remove(item);
        }
        public void UseItem(int itemSlot)
        {
            var item = battleInventory[itemSlot];
            item.UseItem();
            if(item.itemUses <=0)
            battleInventory.Remove(item);
        }
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