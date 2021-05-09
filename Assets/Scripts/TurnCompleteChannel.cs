using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/ Turn Completion")]
public class TurnCompleteChannel : ScriptableObject
{
    public UnityEvent<bool> OnTurnComplete;

    public void RaiseTurnCompleteEvent(bool isComplete)
    {
        if (OnTurnComplete != null)
            OnTurnComplete.Invoke(isComplete);
    }
}
