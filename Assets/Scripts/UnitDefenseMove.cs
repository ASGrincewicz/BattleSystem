using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class UnitDefenseMove : UnitMove
{
    public int defenseBuff;
    [SerializeField] private int _defeneseStrength;

    public UnityEvent OnDefenseMoveUsed;

    public void RaiseDefenseMoveUsedEvent(string unitName)
    {
        PerformDefenseMove(unitName);
    }
    private void PerformDefenseMove(string unitName)
    {
        if (moveName != "")
        {
            displayActionChannel.RaiseDisplayActionEvent($"{unitName} used {moveName}!");
        }

        else
            return;
    }
}