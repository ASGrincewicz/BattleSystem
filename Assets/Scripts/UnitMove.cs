using UnityEngine;
using UnityEngine.Events;
using Veganimus.BattleSystem;

public abstract class UnitMove : ScriptableObject
{
    public string moveName;
    public int moveCost;
    public enum MoveType
    {
        Physical, Special, Buff, DeBuff
    }
    [SerializeField] protected MoveType _moveType;

    [SerializeField] protected ElementType _elementType;
    public Transform _assignedUnit;

    public UnityEvent<UnitMove> OnMoveQueued;

    public void RaiseMoveQueuedEvent(UnitMove unitMove)
    {
        if (OnMoveQueued != null)
            OnMoveQueued.Invoke(this);
    }
}
