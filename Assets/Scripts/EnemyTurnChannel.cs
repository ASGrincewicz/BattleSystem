using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/Battle State Change Channel/Enemy Turn")]
public class EnemyTurnChannel : ScriptableObject
{
    public UnityEvent OnEnemyTurn;

    public void RaiseEnemyTurnEvent()
    {
        if (OnEnemyTurn != null)
            OnEnemyTurn.Invoke();
    }
}