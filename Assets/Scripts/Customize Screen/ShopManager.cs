using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Veganimus.BattleSystem;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private ShopInventory _shopInventory;
    [SerializeField] private List<Image> productImages = new List<Image>();
    [SerializeField] private Image _productImagePrefab;

    [ContextMenuItem("Get Items", "GetItems")]
    [SerializeField] private List<Item> _shopInventoryList;
    [SerializeField] private List<Item> _customerInventoryList;
    [SerializeField] private Transform _shopGrid, _inventoryGrid;

    [SerializeField] private CharacterStats _customer;
   
    [SerializeField] private List<CharacterStats> _characters;
    [SerializeField] private TMP_Dropdown _selectCharacter;
    [ContextMenuItem("Get Items", "GetItems")]
    public string getItems;
   
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
        }
        _shopInventoryList.Sort();
    }


}

