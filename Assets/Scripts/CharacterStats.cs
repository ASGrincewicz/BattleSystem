using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Character")]
public class CharacterStats : ScriptableObject, IComparable<CharacterStats>
{
    [SerializeField] private int id;
    [SerializeField] private string _characterName;
    public string CharacterName { get { return _characterName; } }

    public List<Item> characterInventory = new List<Item>();

    public List<Item> battleInventory = new List<Item>();

    public int CompareTo(CharacterStats other)
    {
        if (this.id < other.id)
            return 1;

        else if (this.id > other.id)
            return -1;

        else
            return 0;
    }
}
