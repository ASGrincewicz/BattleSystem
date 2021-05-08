using UnityEngine;
using Veganimus.BattleSystem;

public abstract class UnitMove : ScriptableObject
{
    public string moveName;
    public int moveCost;
    public enum MoveType
    {
        Physical, Special, Buff, DeBuff
    }
    [SerializeField] private MoveType _moveType;

    [SerializeField] private ElementType _elementType;
    public Unit _assignedUnit;
}
