using UnityEngine;
using Veganimus.BattleSystem;

public abstract class UnitMove : ScriptableObject
{
    public string MoveName { get => _moveName; protected set => _moveName = value; }
    [SerializeField] protected string _moveName;
    public int MoveUses { get => _moveUses; protected set => _moveUses = value; }
    [Range(0, 10)] [SerializeField] protected int _moveUses;
    public float MoveAccuracy { get => _moveAccuracy; private set => value = _moveAccuracy; }
    [SerializeField] private float _moveAccuracy;
    public enum MoveType
    {
        Physical, Special, Buff, DeBuff
    }
    [SerializeField] protected MoveType _moveType;
    [SerializeField] protected ElementType _elementType;
    [SerializeField] protected Transform _assignedUnit;
    [SerializeField] protected DisplayActionChannel _displayActionChannel;

    public bool RollForMoveAccuracy(int accuracyModifier)
    {
        int dieRoll = Random.Range(1, 6);
        int secondDieRoll = Random.Range(1, 6);
        float result = (dieRoll + secondDieRoll) * _moveAccuracy;
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