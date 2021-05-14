using UnityEngine;
using Veganimus.BattleSystem;

public class HealthItem : Item
{
    public HealthItem(string itemName, bool isConsumable, int itemUses) { }
    public int healAmount;

    public override void UseItem(Unit unit)
    {
        base.UseItem(unit);
        var iHeal = unit.GetComponent<IHealable>();
        iHeal.Heal(healAmount);
    }
}
