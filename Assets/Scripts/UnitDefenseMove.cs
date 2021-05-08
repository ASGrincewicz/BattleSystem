using UnityEngine;
using UnityEngine.Events;
using Veganimus.BattleSystem;

[CreateAssetMenu(menuName = "Unit Moves/ Defense Move")]
public class UnitDefenseMove : UnitMove
{
    [SerializeField] private int _defenseBuff;
    [SerializeField] private int _defenseStrength;

    public UnityEvent OnDefenseMoveUsed;

    public void RaiseDefenseMoveUsedEvent(Unit assignedUnit)
    {
        PerformDefenseMove(assignedUnit);
    }
    private void PerformDefenseMove(Unit assignedUnit)
    {
        Debug.Log($"{assignedUnit} is Defending!");
    }
}