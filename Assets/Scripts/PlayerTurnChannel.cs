using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/Battle State Change Channel/Player Turn")]
public class PlayerTurnChannel : ScriptableObject
{
    public UnityEvent OnPlayerTurn;

    public void RaisePlayerTurnEvent()
    {
        if (OnPlayerTurn != null)
            OnPlayerTurn.Invoke();
    }
}