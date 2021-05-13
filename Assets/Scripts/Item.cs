using UnityEngine;

public class Item : ScriptableObject
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    ///
    
    public string itemName;
   
    public bool isConsumable;
    
    public int itemUses;

    public virtual void UseItem()
    {
        itemUses--;
        if (itemUses <= 0)
            return;
    }
}
