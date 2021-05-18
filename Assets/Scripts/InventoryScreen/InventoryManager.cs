using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Veganimus.BattleSystem;

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
    
   
    private void Start()
    {
        AddDropDownOptions();
        owner = _characters[0];
        inventory = owner.characterInventory;
        battleInventory = owner.battleInventory;
        PopulateInventoryGrid();
    }
    public void ChangeCharacter(int index)
    {
        index = _selectCharacter.value -1;
        owner = _characters[index];
        itemImage.Clear();

        ClearChildObjects(itemGrid);
        ClearChildObjects(battleItemGrid);
        inventory = owner.characterInventory;
        battleInventory = owner.battleInventory;
        PopulateInventoryGrid();
    }
    public void AddDropDownOptions()
    {
        var characterNames = new List<string>();
        foreach(var character in _characters)
        {
            var characterName = character.CharacterName;
            characterNames.Add(characterName);
        }
        _selectCharacter.AddOptions(characterNames);
    }
    private void PopulateInventoryGrid()
    {
        foreach(var item in owner.characterInventory)
        {
            var newItemImage = Instantiate(itemImagePrefab, itemGrid);
            newItemImage.GetComponent<DragItem>().item = item;
            newItemImage.GetComponentInChildren<TMP_Text>().text = item.itemName;
            itemImage.Add(newItemImage);
        }
        foreach (var item in owner.battleInventory)
        {
            var newItemImage = Instantiate(itemImagePrefab, battleItemGrid);
            newItemImage.GetComponent<DragItem>().item = item;
            newItemImage.GetComponentInChildren<TMP_Text>().text = item.itemName;
            itemImage.Add(newItemImage);
        }
    }
    public void AddToBattleInventory(Item item)
    {
        if (battleInventory.Count == 4) return;
        battleInventory.Add(item);
        inventory.Remove(item);
    }
    public void AddToInventory(Item item)
    {
        battleInventory.Remove(item);
        inventory.Add(item);
    }
    public void ClearChildObjects(Transform transform)
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }
}
