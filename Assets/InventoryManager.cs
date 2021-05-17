using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Veganimus.BattleSystem;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<Button> itemButtons = new List<Button>();
    public List<Button> battleItemButtons = new List<Button>();
    //public List<TMP_Text> itemImageText = new List<TMP_Text>();
    public List<TMP_Text> battleItemButtonText = new List<TMP_Text>();
    public Character owner;
    public List<Item> inventory = new List<Item>();
    public List<Item> battleInventory = new List<Item>();

    public Button itemButtonPrefab;
    public Button battleItemButtonPrefab;
    //public TMP_Text itemTextPrefab;
    //public TMP_Text battleItemTextPrefab;

    public Transform itemGrid;
    public Transform battleItemGrid;

    // Start is called before the first frame update
    void Start()
    {
        PopulateInventoryGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PopulateInventoryGrid()
    {
        foreach(var item in inventory)
        {
            var newItemButton = Instantiate(itemButtonPrefab, itemGrid);
            
            itemButtons.Add(newItemButton);

            var itemButtonText = newItemButton.GetComponentInChildren<TMP_Text>();
            itemButtonText.text = $"{item.itemName}";
        }
    }
    public void AddToBattleInventory(int item)
    {
         var itemToAdd = inventory[item];
        battleInventory.Add(itemToAdd);
    }

}
