using UnityEngine;
using UnityEngine.Events;
using Veganimus.BattleSystem;

[CreateAssetMenu(menuName = "Unit Moves/ Attack Move")]
public class UnitAttackMove : UnitMove
{
    [SerializeField] private int _damageAmount;
    [SerializeField] private float _attackSpeed;

    public  void RaiseAttackMoveUsedEvent(Unit assignedUnit)
    {
        PerformAttackMove(assignedUnit);
    }

    private void PerformAttackMove(Unit assignedUnit)
    {
        Debug.Log($"{assignedUnit} is Attacking");
    }
}
