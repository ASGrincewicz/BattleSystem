using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using Veganimus.BattleSystem;

[CreateAssetMenu(menuName = "Unit Moves/ Defense Move")]
public class UnitDefenseMove : UnitMove
{
    public int DefenseBuff { get => _defenseBuff; private set => value = _defenseBuff; }
    [SerializeField] private int _defenseBuff;
    public int DefenseStrength { get => _defeneseStrength; private set => value = _defeneseStrength; }
    [SerializeField] private int _defeneseStrength;

    public UnityEvent OnDefenseMoveUsed;

    public void RaiseDefenseMoveUsedEvent(string unitName)
    {
        PerformDefenseMove(unitName);
    }
    private void PerformDefenseMove(string unitName)
    {
        if (MoveName != "")
        {
            _displayActionChannel.RaiseDisplayActionEvent($"{unitName} used {MoveName}!");
        }

        else
            return;
    }
    public void CreateNewDefenseMove(string newMoveName)
    {
        UnitDefenseMove newDefenseMove = CreateInstance<UnitDefenseMove>();
        newDefenseMove.MoveName = newMoveName;
        AssetDatabase.CreateAsset(CreateInstance<UnitAttackMove>(), $"Assets/Scripts/Scriptable Objects/Moves/Attack Moves/{newDefenseMove.MoveName}.asset");
    }
}