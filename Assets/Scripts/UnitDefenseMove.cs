using UnityEngine;
using UnityEngine.Events;
using Veganimus.BattleSystem;

public class UnitDefenseMove : UnitMove
{
    public int defenseBuff;
    public int turnsActive;

    public enum DefenseType { EnergyShield, Barrier, Cloak, Armor}
    [SerializeField] private DefenseType _defenseType;
    public DefenseType MoveDefenseType { get { return _defenseType; } }
    public GameObject effectObject;
    public UnityEvent OnDefenseMoveUsed;

    public void RaiseDefenseMoveUsedEvent(Unit unit, DefenseType moveDefenseType)
    {
        ActivateDefenseEffect(unit, moveDefenseType);
    }
    private void ActivateDefenseEffect(Unit unit, DefenseType defenseType)
    {
        switch (defenseType)
        {
            case DefenseType.EnergyShield:
                if (unit.UnitEnergyShield.activeInHierarchy == false)
                {
                    unit.ActiveEffect = unit.UnitEnergyShield;
                    effectObject = unit.UnitEnergyShield;
                    effectObject.SetActive(true);
                }
                else
                    return;
                break;
            case DefenseType.Barrier:
                if (unit.UnitBarrier.activeInHierarchy == false)
                {
                    unit.ActiveEffect = unit.UnitBarrier;
                    effectObject = unit.UnitBarrier;
                    effectObject.SetActive(true);
                }
                else
                    return;
                break;
            case DefenseType.Cloak:
                if (unit.UnitCloak.activeInHierarchy == false)
                {
                    unit.ActiveEffect = unit.UnitCloak;
                    effectObject = unit.UnitCloak;
                    effectObject.SetActive(true);
                    unit.UnitBaseModel.SetActive(false);
                    unit.TargetUnit.targetIBuffable.BuffStats(StatAffected.Accuracy, -10);
                }
                else
                    return;
                break;
            case DefenseType.Armor:

                break;
        }
        effectObject.GetComponent<MoveEffect>().turnsActive = turnsActive;
        effectObject.GetComponent<MoveEffect>().activatedOnTurn = unit.Owner.TurnCount;
    }
}