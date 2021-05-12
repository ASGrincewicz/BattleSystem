using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Unit Moves/ Attack Move")]
public class UnitAttackMove : UnitMove
{
    public int DamageAmount { get => damageAmount; private set => value = damageAmount; }
    public int damageAmount;
    public int AttackSpeed { get => attackSpeed; private set => value = attackSpeed; }
    public int attackSpeed;
    

    public UnityEvent<int, Transform> OnAttackMoveUsed;

    public  void RaiseAttackMoveUsedEvent(string unitName,Transform unit,int moveSlot)
    {
        _assignedUnit = unit;
        PerformAttackMove(unitName, moveSlot);
    }
    
    private void PerformAttackMove(string unitName, int moveSlot)
    {
        if (MoveName != "")
        {
            _displayActionChannel.RaiseDisplayActionEvent($"{unitName} used {MoveName}!");
        }
        else
            return;
    }
}
