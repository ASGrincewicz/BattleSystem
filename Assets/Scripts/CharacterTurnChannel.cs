using UnityEngine;
using UnityEngine.Events;
using Veganimus.BattleSystem;
[CreateAssetMenu(menuName = "Channels/Battle State Change Channel/Character Turn")]
public class CharacterTurnChannel : ScriptableObject
{
    public UnityEvent<CharacterType> OnCharacterTurn;

    public void RaiseCharacterTurnEvent(CharacterType characterType)
    {
        if (OnCharacterTurn != null)
            OnCharacterTurn.Invoke(characterType);
    }
}
