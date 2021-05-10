using System.Collections;
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
            StartCoroutine(TurnDelayRoutine());
        }

        private void DetermineAction()
        {
            var dieRoll = Random.Range(1, 6);
            var attackToUse = Random.Range(0, _unitAttacksMoveSet.Length);
            var defenseToUse = Random.Range(0, _unitDefensesMoveSet.Length);
            //var itemToUse = Random.Range(0, _unitItems.Length);
            if (dieRoll >= 3)
                UseAttackMoveSlot(attackToUse);
            
            else if (dieRoll < 3)
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
            int damageAmount = _unitAttacksMoveSet[slotNumber].damageAmount;
            _targetUnit.targetIDamageable.Damage(damageAmount);
            _unitAttacksMoveSet[slotNumber].RaiseAttackMoveUsedEvent(_unitName, this.transform, slotNumber);
            _isEnemyTurnComplete = true;
            _enemyTurnCompleteChannel.RaiseTurnCompleteEvent(_isEnemyTurnComplete);
        }

        public void UseDefenseMoveSlot(int slotNumber)
        {
            _unitDefensesMoveSet[slotNumber].RaiseDefenseMoveUsedEvent(_unitName);
            AdjustDefense(_unitDefensesMoveSet[slotNumber].defenseBuff);
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
