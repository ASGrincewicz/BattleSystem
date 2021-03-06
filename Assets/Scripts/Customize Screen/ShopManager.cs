using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Veganimus.BattleSystem;

namespace Veganimus
{
    public class ShopManager : Singleton<ShopManager>
    {
        [SerializeField] private ShopInventory _shopInventory;
        [SerializeField] private List<Image> _productImages = new List<Image>();
        [SerializeField] private Image _productImagePrefab;

        [SerializeField] private List<Item> _shopInventoryList;
        [SerializeField] private List<Item> _customerInventoryList;
        [SerializeField] private Transform _shopGrid, _inventoryGrid;

        [SerializeField] private CharacterStats _customer;
        public CharacterStats Customer { get { return _customer; } }
        [SerializeField] private CharacterStats _shopManager;
        public CharacterStats ThisShopManager { get { return _shopManager; } }

        [SerializeField] private List<CharacterStats> _characters;
        [SerializeField] private TMP_Dropdown _selectCharacter;
        [SerializeField] private TMP_Text _characterCredits, _shopCredits;
        [SerializeField] private uint _shopCreditsAmount;
        public uint ShopCreditsAmount { get { return _shopCreditsAmount; } }

        [Header("Audio")]
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private AudioClip _buySound;
        [SerializeField] private AudioClip _sellSound;

        private IEnumerator Start()
        {
            if (_selectCharacter.options.Count > 0)
                _selectCharacter.ClearOptions();

            GetCharacters();
            AddDropdownOptions();
            _customer = _characters[0];
            GetItems();
            _customerInventoryList = _customer.characterInventory;
            _customerInventoryList.Sort();
            PopulateInventoryGrid();
            yield return new WaitForSeconds(2f);
            UpdateCreditsText(_customer.characterCredits, _shopCreditsAmount);
        }
        private void GetCharacters()
        {
            foreach (var character in _characters)
            {
                if (character.CharacterName.Contains("Manager"))
                    _characters.Remove(character);
            }
        }
        private void GetItems()
        {
            foreach (var item in _shopManager.characterInventory)
            {
                _shopInventoryList.Add(item);
                _shopCreditsAmount += item.ItemCreditCost;
            }
            _shopInventoryList.Sort();
        }
        private void AddDropdownOptions()
        {
            var characterNames = new List<string>();
            foreach (var character in _characters)
            {
                var characterName = character.CharacterName;
                characterNames.Add(characterName);

            }
            _selectCharacter.AddOptions(characterNames);
            _selectCharacter.RefreshShownValue();
        }
        private void PopulateInventoryGrid()
        {
            foreach (var item in _shopInventoryList)
            {
                var newProductImage = Instantiate(_productImagePrefab, _shopGrid);
                newProductImage.GetComponent<ShopDragItem>().item = item;
                newProductImage.GetComponentInChildren<ShopDragItem>().itemNameText.text = item.ItemName;
                newProductImage.GetComponentInChildren<ShopDragItem>().itemIcon.sprite = item.ItemIcon;

                _productImages.Add(newProductImage);
            }
            foreach (var item in _customerInventoryList)
            {
                var newItemImage = Instantiate(_productImagePrefab, _inventoryGrid);
                newItemImage.GetComponent<ShopDragItem>().item = item;
                newItemImage.GetComponentInChildren<ShopDragItem>().itemIcon.sprite = item.ItemIcon;
                newItemImage.GetComponentInChildren<TMP_Text>().text = item.ItemName;
                _productImages.Add(newItemImage);
            }
        }
        public void ChangeCharacter(int index)
        {
            index = _selectCharacter.value;
            _customer = _characters[index];
            _productImages.Clear();
            _customerInventoryList = _customer.characterInventory;
            RefreshGrids();
        }
        public void BuyItem(Item item)
        {
            if (_customerInventoryList.Count == 20) return;
            if (_customer.characterCredits >= item.ItemCreditCost)
            {
                _customer.characterCredits -= item.ItemCreditCost;
                _shopCreditsAmount += item.ItemCreditCost;
                _customerInventoryList.Add(item);
                _shopInventoryList.Remove(item);
                _audioManager.PlaySFX(_buySound);
                RefreshGrids();
            }
        }
        public void SellItem(Item item)
        {
            if (_shopCreditsAmount >= item.ItemCreditCost)
            {
                _customer.characterCredits += item.ItemCreditCost;
                _shopCreditsAmount -= item.ItemCreditCost;
                _customerInventoryList.Remove(item);
                _shopInventoryList.Add(item);
                _audioManager.PlaySFX(_sellSound);
                RefreshGrids();
            }
        }
        private void UpdateCreditsText(uint customerAmount, uint shopAmount)
        {
            _characterCredits.text = $"{Customer.CharacterName}'s $: {customerAmount}";
            _shopCredits.text = $"Shop $: {shopAmount}";
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
            if (secondTransform.childCount > 0)
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
            ClearChildObjects(_shopGrid, _inventoryGrid);
            SortInventory(_shopInventoryList, _customerInventoryList);
            _productImages.Clear();
            PopulateInventoryGrid();
            UpdateCreditsText(_customer.characterCredits, _shopCreditsAmount);
        }
    }
}
