using UnityEngine;

public abstract class Item : ScriptableObject
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    ///
    
    public string itemName;
   
    public bool isConsumable;
    
    public int itemUses;

    public void UseItem()
    {
        itemUses--;
        if (itemUses <= 0)
            return;
    }
}
