using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop Inventory")]
public class ShopInventory : ScriptableObject
{
    public List<Item> shopInventory = new List<Item>();

    public ShopInventory() { }
}