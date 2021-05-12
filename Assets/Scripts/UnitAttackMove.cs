using UnityEngine;
using UnityEngine.Events;

public class UnitAttackMove : UnitMove
{
    public int damageAmount;
    public int attackSpeed;

    public UnityEvent<int, Transform> OnAttackMoveUsed;

    public  void RaiseAttackMoveUsedEvent(string unitName,Transform unit,int moveSlot)
    {
        _assignedUnit = unit;
        PerformAttackMove(unitName, moveSlot);
    }
    
    private void PerformAttackMove(string unitName, int moveSlot)
    {
        if (moveName != "")
        {
            displayActionChannel.RaiseDisplayActionEvent($"{unitName} used {moveName}!");
        }
        else
            return;
    }
}
