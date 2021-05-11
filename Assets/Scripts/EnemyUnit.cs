using System.Collections;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class EnemyUnit : Unit, IDamageable, IHealable, IDefendable
    {
        public int EnemyAggression { get => _enemyAggression; private set => _enemyAggression = value; }
        [Range(-1, 1)] [SerializeField] private int _enemyAggression;

        [Header("Broadcasting On:")]
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] private UnitHitPointUpdate _unitHPUpdateChannel;
        [SerializeField] private TurnCompleteChannel _enemyTurnCompleteChannel;
        [Header("Listening To:")]
        [SerializeField] private EnemyTurnChannel _enemyTurnChannel;
        [Space]
        private bool _isEnemyTurnComplete;

        private void OnEnable() => _enemyTurnChannel.OnEnemyTurn.AddListener(InitiateEnemyTurn);

        private void OnDisable() => _enemyTurnChannel.OnEnemyTurn.RemoveListener(InitiateEnemyTurn);

        private void Start()
        {
            _unitNameUpdateChannel.RaiseUnitNameUpdateEvent("Enemy", _unitName);
            _animator = GetComponent<Animator>();
        }

        private void InitiateEnemyTurn()
        {
            _isEnemyTurnComplete = false;
            _enemyTurnCompleteChannel.RaiseTurnCompleteEvent(_isEnemyTurnComplete);
            SetMoveUses();
            StartCoroutine(TurnDelayRoutine());
        }

        private void DetermineAction()
        {
            var dieRoll = Random.Range(1, 6);
            var attackToUse = Random.Range(0, _unitAttacksMoveSet.Length);
            var defenseToUse = Random.Range(0, _unitDefensesMoveSet.Length);
            //var itemToUse = Random.Range(0, _unitItems.Length);
            if (dieRoll + _enemyAggression >= 3 )
                UseAttackMoveSlot(attackToUse);
            
            else if (dieRoll + _enemyAggression < 3)
                UseDefenseMoveSlot(defenseToUse);
        }

        public void Damage(int amount)
        {
            var damage = amount -= _unitDefense;
            if (damage <= 0)
                damage = 0;

            _currentUnitHP -= damage;
            if (_currentUnitHP <= 0)
            {
                _currentUnitHP = 0;
                _animator.SetInteger("hitPoints", 0);
                _endBattleChannel.RaiseBattleStateChangeEvent(BattleState.Win);
            }
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Enemy", _unitHitPoints, _currentUnitHP);
            StartCoroutine(StatUpdateDelayRoutine($"{_unitName} took {damage} damage!"));
        }
       
        public void Heal(int amount)
        {
            _currentUnitHP += amount;
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Enemy", _unitHitPoints, _currentUnitHP);
            StartCoroutine(StatUpdateDelayRoutine($"{_unitName} healed {amount} HP!"));
        }
        public void AdjustDefense(int amount)
        {
            _unitDefense += amount;
            StartCoroutine(StatUpdateDelayRoutine($"{_unitName} raised Defense by {amount}."));
        }

        public void UseAttackMoveSlot(int slotNumber)
        {
            int usesLeft = _attackMoveUses[slotNumber];
            if (usesLeft > 0)
            {
                _attackMoveUses[slotNumber]--;
                bool didMoveHit = _unitAttacksMoveSet[slotNumber].RollForMoveAccuracy(_accuracyModifier);
                if (didMoveHit == true)
                {
                    int damageAmount = _unitAttacksMoveSet[slotNumber].DamageAmount;
                    _targetUnit.targetIDamageable.Damage(damageAmount);
                    _unitAttacksMoveSet[slotNumber].RaiseAttackMoveUsedEvent(_unitName, this.transform, slotNumber);
                }
                else if (didMoveHit == false)
                {
                    _displayAttackActionChannel.RaiseDisplayActionEvent($"{_unitName} used {_unitAttacksMoveSet[slotNumber].MoveName}!");
                    StartCoroutine(StatUpdateDelayRoutine($"{_unitName} Missed!"));
                }
                _isEnemyTurnComplete = true;
                _enemyTurnCompleteChannel.RaiseTurnCompleteEvent(_isEnemyTurnComplete);
            }
            else if (usesLeft <= 0)
                DetermineAction();
        }

        public void UseDefenseMoveSlot(int slotNumber)
        {
            int usesLeft = _defenseMoveUses[slotNumber];
            if (usesLeft > 0)
            {
                _defenseMoveUses[slotNumber]--;
                _unitDefensesMoveSet[slotNumber].RaiseDefenseMoveUsedEvent(_unitName);
                AdjustDefense(_unitDefensesMoveSet[slotNumber].DefenseBuff);
                _isEnemyTurnComplete = true;
                _enemyTurnCompleteChannel.RaiseTurnCompleteEvent(true);
            }
            else if (usesLeft <= 0)
                DetermineAction();
        }
        private IEnumerator TurnDelayRoutine()
        {
            yield return new WaitForSeconds(5f);
            DetermineAction();
        }
    }
}
