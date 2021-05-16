using UnityEngine;
using UnityEngine.Events;
using Veganimus.BattleSystem;

[CreateAssetMenu(menuName = "Channels/ Turn Completion")]
public class TurnCompleteChannel : ScriptableObject
{
    public UnityEvent<CharacterType,bool> OnTurnComplete;

    public void RaiseTurnCompleteEvent(CharacterType characterType,bool isComplete)
    {
        if (OnTurnComplete != null)
            OnTurnComplete.Invoke(characterType,isComplete);
    }
}
