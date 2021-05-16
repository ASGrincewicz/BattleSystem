using UnityEngine;
using UnityEngine.Events;
using Veganimus.BattleSystem;

public class UnitDefenseMove : UnitMove
{
    public int defenseBuff;
    [SerializeField] private int _defeneseStrength;

    public enum DefenseType { EnergyShield, Barrier, Cloak, Armor}
    [SerializeField] private DefenseType _defenseType;
    public DefenseType MoveDefenseType { get { return _defenseType; } }
    
    public UnityEvent OnDefenseMoveUsed;

    public void RaiseDefenseMoveUsedEvent(Unit unit, DefenseType moveDefenseType)
    {
        ActivateDefenseEffect(unit, moveDefenseType);
    }
    private void ActivateDefenseEffect(Unit unit, DefenseType defenseType)
    {
       switch(defenseType)
        {
            case DefenseType.EnergyShield:
                if (unit.UnitEnergyShield.activeInHierarchy == false)
                {
                    unit.UnitEnergyShield.SetActive(true);
                }
                else
                    return;
                break;
            case DefenseType.Barrier:
                if (unit.UnitBarrier.activeInHierarchy == false)
                    unit.UnitBarrier.SetActive(true);
                else
                    return;
                break;
            case DefenseType.Cloak:
                if (unit.UnitCloak.activeInHierarchy == false)
                    unit.UnitCloak.SetActive(true);
                else
                    return;
                break;
            case DefenseType.Armor:

                break;
        }
    }
}