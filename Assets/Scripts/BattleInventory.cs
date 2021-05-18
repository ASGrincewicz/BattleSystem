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
    public class BattleInventory : MonoBehaviour
    {
        [SerializeField] private Character _owner;
        
        public List<Item> battleInventory = new List<Item>();//runtime copies

        [SerializeField] private List<Item> _assetBattleInventory;

        [SerializeField] private int _capacity;

        [SerializeField] private Item _selectedItem;

        [SerializeField] private Item _emptySlotPlaceHolder;

       
        private void OnEnable()
        {
            _owner = GetComponent<Character>();
            PopulateAssetInventory();
            GenerateCopyItems();
        }
      
        public void UseItem(int itemSlot)
        {
            var item = battleInventory[itemSlot];
            item.UseItem(_owner.activeUnit);
        }
        private void PopulateAssetInventory()
        {
            foreach(var item in _owner.ThisCharacterStats.battleInventory)
            {
                _assetBattleInventory.Add(item);
            }
        }
        /// <summary>
        /// This method creates copies of assigned items to prevent changes to asset values.
        /// </summary>
        public void GenerateCopyItems()
        {
            if (_assetBattleInventory.Count > 0)
            {
                foreach (var item in _assetBattleInventory)
                {
                    var copy = Instantiate(item);
                    battleInventory.Add(copy);
                }
            }
        }
    }
}