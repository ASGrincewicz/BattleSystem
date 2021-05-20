using UnityEngine;
using Veganimus.BattleSystem;

public enum MoveType
{
    Physical, Special, Buff, DeBuff
}
[System.Serializable]
public abstract class UnitMove : ScriptableObject
{
    [SerializeField] private MoveInfo _moveInfo = new MoveInfo();
    public string MoveName { get { return _moveInfo.moveName; } }
    public int MoveUses { get { return _moveInfo.uses; } set { } }
    public float MoveAccuracy { get { return _moveInfo.accuracy; } }
    public MoveType MoveType { get { return _moveInfo.moveType; } }
    public ElementType MoveElementType { get { return _moveInfo.elementType; } }
    public string animationTrigger;
    [SerializeField] protected Transform _assignedUnit;
    public DisplayActionChannel displayActionChannel;
    private DieRoll dieRoll;
    public int runtimeUses;

    private void OnEnable() => runtimeUses = MoveUses;

    public bool RollForMoveAccuracy(int accuracyModifier)
    {
        dieRoll = new DieRoll();
        return dieRoll.Roll(MoveAccuracy, accuracyModifier);
    }
}
