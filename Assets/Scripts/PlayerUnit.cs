using UnityEngine;
using System;
using System.Collections;

namespace Veganimus.BattleSystem
{
    public class PlayerUnit : Unit, IDamageable, IHealable, IDefendable
    { 
        [Header("Broadcasting on")]
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] private UnitHitPointUpdate _unitHPUpdateChannel;
        [SerializeField] private TurnCompleteChannel _playerTurnCompleteChannel;
        [Header("Listening to:")]
        [SerializeField] private PlayerTurnChannel _playerTurnChannel;
        [Space]
        private bool _isPlayerTurnComplete;

        private void OnEnable() => _playerTurnChannel.OnPlayerTurn.AddListener(InitiatePlayerTurn);

        private void OnDisable() => _playerTurnChannel.OnPlayerTurn.RemoveListener(InitiatePlayerTurn);

        private void Start() => _unitNameUpdateChannel.RaiseUnitNameUpdateEvent("Player", _unitName);

        public void Damage(int amount)
        {
            _currentUnitHP -= (amount- _unitDefense);
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Player", _unitHitPoints, _currentUnitHP);
        }
        public void Heal(int amount)
        {
            _currentUnitHP += amount;
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Player", _unitHitPoints, _currentUnitHP);
        }
        public void AdjustDefense(int amount) => _unitDefense += amount;

        private void InitiatePlayerTurn()
        {
            _isPlayerTurnComplete = false;
            _playerTurnCompleteChannel.RaiseTurnCompleteEvent(_isPlayerTurnComplete);

        }
        public void UseAttackMoveSlot(int slotNumber)
        {
            _unitAttacksMoveSet[slotNumber].RaiseAttackMoveUsedEvent(_unitName, slotNumber);
            AcquireTarget(_unitAttacksMoveSet[slotNumber].damageAmount);
            _isPlayerTurnComplete = true;
            _playerTurnCompleteChannel.RaiseTurnCompleteEvent(_isPlayerTurnComplete);
        }

        public void UseDefenseMoveSlot(int slotNumber)
        {
            _unitDefensesMoveSet[slotNumber].RaiseDefenseMoveUsedEvent(_unitName);
            AdjustDefense(_unitDefensesMoveSet[slotNumber].defenseBuff);
            _isPlayerTurnComplete = true;
            _playerTurnCompleteChannel.RaiseTurnCompleteEvent(_isPlayerTurnComplete);
        }
    }
}
