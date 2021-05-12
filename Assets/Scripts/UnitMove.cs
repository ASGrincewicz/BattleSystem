using UnityEngine;
using Veganimus.BattleSystem;

public enum MoveType
{
    Physical, Special, Buff, DeBuff
}
public abstract class UnitMove : ScriptableObject
{
    public string MoveName { get => moveName; protected set => moveName = value; }
    public string moveName;
    public int MoveUses { get => moveUses; protected set => moveUses = value; }
    [Range(0, 10)] public int moveUses;
    public float MoveAccuracy { get => moveAccuracy; private set => value = moveAccuracy; }
    public float moveAccuracy;
    public MoveType moveType;
    public ElementType elementType;
    [SerializeField] protected Transform _assignedUnit;
    [SerializeField] protected DisplayActionChannel _displayActionChannel;

    public bool RollForMoveAccuracy(int accuracyModifier)
    {
        int dieRoll = Random.Range(1, 6);
        int secondDieRoll = Random.Range(1, 6);
        float result = (dieRoll + secondDieRoll) * moveAccuracy;
        float modifiedResult = Mathf.Round(result + accuracyModifier);
        float finalResult = modifiedResult / 100f;
        
        if (finalResult > 3)
            return true;
        else
            return false;
    }
    // die roll int random between 1 and 6
    // multiply die roll result by accuracy
    //add accuracy modifiers
    // round result
    // divide above result by 100
    // example = 4
    // result above 3 is successful
}