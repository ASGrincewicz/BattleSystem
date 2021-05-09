using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class EnemyUnit : Unit, IDamageable, IHealable, IDefendable
    {
        [Header("Broadcasting On:")]
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] private UnitHitPointUpdate _unitHPUpdateChannel;
        [SerializeField] private TurnCompleteChannel _enemyTurnCompleteChannel;
        [Header("Listening To:")]
        [SerializeField] private EnemyTurnChannel _enemyTurnChannel;
        [Space]
        private bool _isEnemyTurnComplete;

        private void OnEnable()
        {
            _enemyTurnChannel.OnEnemyTurn.AddListener(InitiateEnemyTurn);
        }
        private void OnDisable()
        {
            _enemyTurnChannel.OnEnemyTurn.RemoveListener(InitiateEnemyTurn);
        }
        private void Start() => _unitNameUpdateChannel.RaiseUnitNameUpdateEvent("Enemy", _unitName);

        private void InitiateEnemyTurn()
        {
            _isEnemyTurnComplete = false;
            _enemyTurnCompleteChannel.RaiseTurnCompleteEvent(_isEnemyTurnComplete);
            StartCoroutine(TurnDelayRoutine());
        }

        private void DetermineAction()
        {
            var dieRoll = Random.Range(1, 6);
            if (dieRoll >= 3)
                UseAttackMoveSlot(0);
            //end turn
            else if (dieRoll < 3)
                UseDefenseMoveSlot(0);
            //end turn
        }

        public void Damage(int amount)
        {
            _currentUnitHP -= amount - _unitDefense;
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Enemy",_unitHitPoints, _currentUnitHP);
        }
        public void Heal(int amount)
        {
            _currentUnitHP += amount;
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Enemy", _unitHitPoints, _currentUnitHP);
        }
        public void AdjustDefense(int amount) => _unitDefense = amount;

        public void UseAttackMoveSlot(int slotNumber)
        {
            _unitAttacksMoveSet[slotNumber].RaiseAttackMoveUsedEvent(_unitName, slotNumber);
            _isEnemyTurnComplete = true;
            _enemyTurnCompleteChannel.RaiseTurnCompleteEvent(true);
        }

        public void UseDefenseMoveSlot(int slotNumber)
        {
            _unitDefensesMoveSet[slotNumber].RaiseDefenseMoveUsedEvent(_unitName);
            _isEnemyTurnComplete = true;
            _enemyTurnCompleteChannel.RaiseTurnCompleteEvent(true);
        }
        private IEnumerator TurnDelayRoutine()
        {
            yield return new WaitForSeconds(5f);
            DetermineAction();
        }
    }
}
