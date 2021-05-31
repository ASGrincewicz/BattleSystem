using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Veganimus.BattleSystem;

namespace Veganimus
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        public List<Image> itemImage = new List<Image>();
        public CharacterStats owner;
        [SerializeField] private List<Item> inventory;
        [SerializeField] private List<Item> battleInventory;
        [SerializeField] private Image itemImagePrefab;
        [SerializeField] private Transform itemGrid;
        [SerializeField] private Transform battleItemGrid;
        [SerializeField] private List<CharacterStats> _characters;
        [SerializeField] private TMP_Dropdown _selectCharacter;
        private Image newItemImage;
        [Header("Audio")]
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private AudioClip _addToBInvSound;
        [SerializeField] private AudioClip _addToInvSound;

        private void Start()
        {
            if (_selectCharacter.options.Count > 0)
                _selectCharacter.ClearOptions();

            AddDropdownOptions();
            owner = _characters[0];
            inventory = owner.characterInventory;
            battleInventory = owner.battleInventory;
            RefreshGrids();
        }
        public void ChangeCharacter(int index)
        {
            index = _selectCharacter.value;
            owner = _characters[index];
            itemImage.Clear();
            inventory = owner.characterInventory;
            battleInventory = owner.battleInventory;
            RefreshGrids();
        }
        private void AddDropdownOptions()
        {
            var characterNames = new List<string>();
            foreach (var character in _characters)
            {
                var characterName = character.CharacterName;
                characterNames.Add(characterName);

                if (characterName.Contains("Manager"))
                    characterNames.Remove(characterName);
            }
            _selectCharacter.AddOptions(characterNames);
            _selectCharacter.RefreshShownValue();
        }
        private void PopulateInventoryGrid()
        {

            foreach (var item in owner.characterInventory)
            {
                newItemImage = Instantiate(itemImagePrefab, itemGrid);
                newItemImage.GetComponent<DragItem>().item = item;
                newItemImage.GetComponentInChildren<TMP_Text>().text = item.ItemName;
                newItemImage.GetComponentInChildren<DragItem>().itemIcon.sprite = item.ItemIcon;
                itemImage.Add(newItemImage);
            }
            foreach (var item in owner.battleInventory)
            {
                newItemImage = Instantiate(itemImagePrefab, battleItemGrid);
                newItemImage.GetComponent<DragItem>().item = item;
                newItemImage.GetComponentInChildren<TMP_Text>().text = item.ItemName;
                newItemImage.GetComponentInChildren<DragItem>().itemIcon.sprite = item.ItemIcon;
                itemImage.Add(newItemImage);

            }
        }
        public void AddToBattleInventory(Item item)
        {
            if (battleInventory.Count == 4) return;
            battleInventory.Add(item);
            inventory.Remove(item);
            _audioManager.PlaySFX(_addToBInvSound);
            RefreshGrids();
        }
        public void AddToInventory(Item item)
        {
            battleInventory.Remove(item);
            inventory.Add(item);
            _audioManager.PlaySFX(_addToInvSound);
            RefreshGrids();
        }
        private void SortInventory(List<Item> inventoryToSort, [Optional] List<Item> secondInventory)
        {
            inventoryToSort.Sort();
            secondInventory.Sort();
        }
        private void ClearChildObjects(Transform transform, [Optional] Transform secondTransform)
        {
            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    Destroy(child.gameObject);
                }
            }
            if (secondTransform != null && secondTransform.childCount > 0)
            {
                for (int i = 0; i < secondTransform.childCount; i++)
                {
                    var child = secondTransform.GetChild(i);
                    Destroy(child.gameObject);
                }
            }
        }
        public void RefreshGrids()
        {
            ClearChildObjects(battleItemGrid, itemGrid);
            SortInventory(inventory, battleInventory);
            itemImage.Clear();
            PopulateInventoryGrid();
        }
    }
}
