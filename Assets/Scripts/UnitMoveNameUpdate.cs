using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/ Move Name Update")]
public class UnitMoveNameUpdate : ScriptableObject
{

    public UnityEvent<string, int> OnMoveNameUpdated;

    public void RaiseMoveNameUpdateEvent(string moveName, int moveSlot)
    {
        if (OnMoveNameUpdated != null)
            OnMoveNameUpdated.Invoke(moveName, moveSlot);
    }
}
