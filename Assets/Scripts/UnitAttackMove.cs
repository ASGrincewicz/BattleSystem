using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using Veganimus.BattleSystem;

[CreateAssetMenu(menuName = "Unit Moves/ Attack Move")]
public class UnitAttackMove : UnitMove
{
    public int DamageAmount { get => _damageAmount; private set => value = _damageAmount; }
    [SerializeField] private int _damageAmount;
    public float AttackSpeed { get => _attackSpeed; private set => value = _attackSpeed; }
    [SerializeField] private float _attackSpeed;
    

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
    
    public void CreateNewAttackMove(string newMoveName)
    {
        UnitAttackMove newAttackMove = CreateInstance<UnitAttackMove>();
        newAttackMove.MoveName = newMoveName;
        AssetDatabase.CreateAsset(CreateInstance<UnitAttackMove>(), $"Assets/Scripts/Scriptable Objects/Moves/Attack Moves/{newAttackMove.MoveName}.asset");
    }
}
