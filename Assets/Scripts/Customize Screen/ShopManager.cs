using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Veganimus.BattleSystem;

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
   
    [SerializeField] private List<CharacterStats> _characters;
    [SerializeField] private TMP_Dropdown _selectCharacter;
    [SerializeField] private TMP_Text _characterCredits, _shopCredits;
    [SerializeField] private int _shopCreditsAmount;
    public int ShopCreditsAmount { get { return _shopCreditsAmount; } }
    [ContextMenuItem("Get Items", "GetItems")]
    public string getItems;

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
    public void GetCharacters()
    {
        var characters = Resources.FindObjectsOfTypeAll<CharacterStats>();
        foreach(var character in characters)
        {
            _characters.Add(character);
        }
    }
    public void GetItems()
    {
        var items = Resources.FindObjectsOfTypeAll<Item>();
        foreach(var item in items)
        {
            _shopInventoryList.Add(item);
            _shopCreditsAmount += item.itemCreditCost;
        }
        _shopInventoryList.Sort();
    }
    public void AddDropdownOptions()
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
            newProductImage.GetComponentInChildren<TMP_Text>().text = item.itemName;
            _productImages.Add(newProductImage);
           
        }
        foreach (var item in _customerInventoryList)
        {
            var newItemImage = Instantiate(_productImagePrefab, _inventoryGrid);
            newItemImage.GetComponent<ShopDragItem>().item = item;
            newItemImage.GetComponentInChildren<TMP_Text>().text = item.itemName;
            _productImages.Add(newItemImage);
        }
    }
    public void ChangeCharacter(int index)
    {
        index = _selectCharacter.value;
        _customer = _characters[index];
        _productImages.Clear();
        ClearChildObjects(_shopGrid, _inventoryGrid);
        _customerInventoryList = _customer.characterInventory;
        SortInventory(_shopInventoryList, _customerInventoryList);
        PopulateInventoryGrid();
    }
    public void BuyItem(Item item)
    {
        if (_customerInventoryList.Count == 20) return;
        if (_customer.characterCredits >= item.itemCreditCost)
        {
            _customer.characterCredits -= item.itemCreditCost;
            _shopCreditsAmount += item.itemCreditCost;
            UpdateCreditsText(_customer.characterCredits, _shopCreditsAmount);
            _customerInventoryList.Add(item);
            _shopInventoryList.Remove(item);
            SortInventory(_shopInventoryList, _customerInventoryList);
        }
    }
    public void SellItem(Item item)
    {
        if (_shopCreditsAmount >= item.itemCreditCost)
        {
            _customer.characterCredits += item.itemCreditCost;
            _shopCreditsAmount -= item.itemCreditCost;
            UpdateCreditsText(_customer.characterCredits, _shopCreditsAmount);
            _customerInventoryList.Remove(item);
            _shopInventoryList.Add(item);
            SortInventory(_shopInventoryList, _customerInventoryList);
        }
    }
    private void UpdateCreditsText(int customerAmount, int shopAmount)
    {
        _characterCredits.text = $"Character's $: {customerAmount}";
        _shopCredits.text = $"Shop $: {shopAmount}";
    }
    public void SortInventory(List<Item> inventoryToSort, [Optional] List<Item> secondInventory)

    {
        inventoryToSort.Sort();
        secondInventory.Sort();
    }
    public void ClearChildObjects(Transform transform, [Optional]Transform secondTransform)
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
}

