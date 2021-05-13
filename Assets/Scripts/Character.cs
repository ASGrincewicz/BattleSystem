using System.Collections.Generic;
using UnityEngine;

namespace Veganimus.BattleSystem
{
    public enum CharacterType { Player, Enemy, Ally, EnemyAlly }
    public class Character : MonoBehaviour
    {
        public CharacterType thisCharacterType;
        [SerializeField] private string _characterName;
        [Range(-1, 1)] public int aIAggression;
        [SerializeField] private List<Unit> _party = new List<Unit>();
        public Unit activeUnit;
        public int turnCount;
        public bool isDefeated;
        [Header("Broadcasting on")]
        public TurnCompleteChannel turnCompleteChannel;
        [Space]
        [Header("Listening to:")]
        public CharacterTurnChannel characterTurnChannel;
        [Space]
        public bool isTurnComplete;

        private void OnEnable() => characterTurnChannel.OnCharacterTurn.AddListener(InitiateCharacterTurn);

        private void OnDisable() => characterTurnChannel.OnCharacterTurn.RemoveListener(InitiateCharacterTurn);

        private void InitiateCharacterTurn(CharacterType characterType)
        {
            if (characterType == thisCharacterType)
            {
                isTurnComplete = false;
                turnCompleteChannel.RaiseTurnCompleteEvent(characterType, isTurnComplete);
                if (thisCharacterType != CharacterType.Player && isTurnComplete == false)
                {
                    activeUnit.SetMoveUses();
                    StartCoroutine(activeUnit.TurnDelayRoutine());
                }
            }
        }
    }
}