using System.Collections.Generic;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Character _owner;

        [SerializeField] private List<Item> _inventory;

        [SerializeField] private int _capacity;

        private void Awake()
        {
            _inventory = new List<Item>(_capacity);
        }
        private void OnEnable()
        {
            _owner = GetComponent<Character>();
        }
        public bool AddItem(Item item)
        {
            if (_inventory.Count < _capacity)
            {
                item = ScriptableObject.CreateInstance<Item>();
                _inventory.Add(item);
                return true;
            }

            else
                return false;
        }
        public void RemoveItem(Item item)
        {
            _inventory.Remove(item);
        }
        public void UseItem(Item item)
        {
            item.UseItem();
        }
    }
}