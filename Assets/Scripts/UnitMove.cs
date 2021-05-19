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

    private void OnEnable()
    {
        runtimeUses = MoveUses;
        Debug.Log($"{MoveName}: {runtimeUses}");
    }
    public bool RollForMoveAccuracy(int accuracyModifier)
    {
        dieRoll = new DieRoll();
        if (dieRoll.Roll(MoveAccuracy, accuracyModifier))
        {
            return true;
        }
        else
            return false;



        //int dieRoll = Random.Range(1, 6);
        //int secondDieRoll = Random.Range(1, 6);
        //float result = (dieRoll + secondDieRoll) * MoveAccuracy;
        //float modifiedResult = Mathf.Round(result + accuracyModifier);
        //float finalResult = modifiedResult / 100f;
        
        //if (finalResult > 3)
        //    return true;
        //else
        //    return false;
        
    }
    // die roll int random between 1 and 6
    // multiply die roll result by accuracy
    //add accuracy modifiers
    // round result
    // divide above result by 100
    // example = 4
    // result above 3 is successful
}
