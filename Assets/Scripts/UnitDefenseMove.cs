using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Unit Moves/ Defense Move")]
public class UnitDefenseMove : UnitMove
{
    public int DefenseBuff { get => defenseBuff; private set => value = defenseBuff; }
    public int defenseBuff;
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
}