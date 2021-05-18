using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Character")]
public class CharacterStats : ScriptableObject
{
    [SerializeField] private string _characterName;
    public string CharacterName { get { return _characterName; } }

    public List<Item> characterInventory = new List<Item>();

    public List<Item> battleInventory = new List<Item>();
}
