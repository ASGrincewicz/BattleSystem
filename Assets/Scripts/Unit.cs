using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    [Flags]
    public enum ElementType
    {
        Normal = 0,
        Power = 2,
        Wave = 4,
        Plasma = 8,
        Light = 16,
        Dark = 32
    }
    public class Unit : MonoBehaviour, IDamageable, IHealable,IDefendable
    {
        [SerializeField] protected Character _owner;
        [SerializeField] protected CharacterType _characterType;
        protected int _unitLevel;
        [SerializeField] protected TargetFinder _targetUnit;
        [SerializeField] protected ElementType _unitType;
        [SerializeField] protected string _unitName;
        [SerializeField] protected int _unitHitPoints;
        [SerializeField] protected int _currentUnitHP;
        [SerializeField] protected int _unitSpeed;
        [SerializeField] protected int _unitDefense;
        [SerializeField] protected int _accuracyModifier = 0;
        [SerializeField] protected UnitAttackMove[] _unitAttacksMoveSet = new UnitAttackMove[4];
        [SerializeField] protected List<int> _attackMoveUses = new List<int>();
        [SerializeField] protected UnitDefenseMove[] _unitDefensesMoveSet = new UnitDefenseMove[2];
        [SerializeField] protected List<int> _defenseMoveUses = new List<int>();
        [SerializeField] protected UnitAttackMove _emptyAttackPlaceholder;
        [SerializeField] protected UnitDefenseMove _emptyDefensePlaceholder;
        protected Animator _animator;
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] protected UnitMoveNameUpdate _unitAttackMoveNameUpdateChannel;
        [SerializeField] protected UnitMoveNameUpdate _unitDefenseMoveNameUpdateChannel;
        [SerializeField] protected UnitHitPointUpdate _unitHPUpdateChannel;
        [SerializeField] protected DisplayActionChannel _displayAttackActionChannel;
        [SerializeField] protected BattleStateChannel _endBattleChannel;

        private void Awake()
        {
            _owner = GetComponentInParent<Character>();
            _currentUnitHP = _unitHitPoints;
        }

        private void Start()
        {
            _owner.activeUnit = this;
            _characterType = _owner.thisCharacterType;
            _animator = GetComponent<Animator>();
            _unitNameUpdateChannel.RaiseUnitNameUpdateEvent(_characterType, _unitName);
        }

        public void DisplayUnitStats() => BattleUIManager.Instance.DisplayUnitStats(_currentUnitHP,
                                                                                    _unitHitPoints,
                                                                                    _unitSpeed,
                                                                                    _unitDefense,
                                                                                    _accuracyModifier);

        public void SetMoveUses()
        {
            if (_attackMoveUses.Count > 0 || _defenseMoveUses.Count > 0)
                return;
            else
            {
                for (int i = 0; i < _unitAttacksMoveSet.Length; i++)
                {
                    _attackMoveUses.Add(_unitAttacksMoveSet[i].moveUses);
                }
                for (int i = 0; i < _unitDefensesMoveSet.Length; i++)
                {
                    _defenseMoveUses.Add(_unitDefensesMoveSet[i].moveUses);
                }
            }

        }
        public void UpdateMoveNames(string moveType)
        {
            UpdateMoveUseUI();
            SetMoveUses();
            
            if (moveType == "Attack")
            {
                for (int i = _unitAttacksMoveSet.Length - 1; i >= 0; i--)
                {
                    var move = _unitAttacksMoveSet[i];
                    _unitAttackMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(move.moveName, i);
                    BattleUIManager.Instance.DisplayMoveStats("attack", move.damageAmount, move.moveAccuracy, 0, i);
                }
            }
            else if (moveType == "Defense")
            {
                for (int i = _unitDefensesMoveSet.Length - 1; i >= 0; i--)
                {
                    var move = _unitDefensesMoveSet[i];
                    _unitDefenseMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(move.moveName, i);
                    BattleUIManager.Instance.DisplayMoveStats("defense", 0, 0, move.defenseBuff, i);
                }
            }
        }
        public void UpdateMoveUseUI()
        {
            if (_attackMoveUses.Count > 0 || _defenseMoveUses.Count > 0)
                return;
            for (int i = _unitAttacksMoveSet.Length-1; i >= 0; i--)
            {
                if(_unitAttacksMoveSet[i].moveUses >0)
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("attack", _unitAttacksMoveSet[i].moveUses, i);
            }
            for (int i = _unitDefensesMoveSet.Length-1; i >= 0; i--)
            {
                if(_unitDefensesMoveSet[i].moveUses > 0)
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("defense", _unitDefensesMoveSet[i].moveUses, i);
            }
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
                if (_characterType == CharacterType.Player)
                    _endBattleChannel.RaiseBattleStateChangeEvent(BattleState.Lose);
                else
                    _endBattleChannel.RaiseBattleStateChangeEvent(BattleState.Win);
            }
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent(_characterType, _unitHitPoints, _currentUnitHP);
            StartCoroutine(StatUpdateDelayRoutine($"{_unitName} took {damage} damage!"));
        }
        public void Heal(int amount)
        {
            _currentUnitHP += amount;
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent(_characterType, _unitHitPoints, _currentUnitHP);
            StartCoroutine(StatUpdateDelayRoutine($"{_unitName} healed {amount} HP!"));
        }
        public void AdjustDefense(int amount)
        {
            _unitDefense += amount;
            StartCoroutine(StatUpdateDelayRoutine(($"{_unitName} raised Defense by {amount}.")));
        }
        public void UseAttackMoveSlot(int slotNumber)
        {
            int usesLeft = _attackMoveUses[slotNumber];
            if (usesLeft > 0)
            {
                _attackMoveUses[slotNumber]--;
                BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("attack", _attackMoveUses[slotNumber], slotNumber);
                BattleUIManager.Instance.ActivateButtons(false);
                bool didMoveHit = _unitAttacksMoveSet[slotNumber].RollForMoveAccuracy(_accuracyModifier);
                if (didMoveHit == true)
                {
                    int damageAmount = _unitAttacksMoveSet[slotNumber].damageAmount;
                    _targetUnit.targetIDamageable.Damage(damageAmount);
                    _unitAttacksMoveSet[slotNumber].RaiseAttackMoveUsedEvent(_unitName, this.transform, slotNumber);
                }
                else if (didMoveHit == false)
                {
                    _displayAttackActionChannel.RaiseDisplayActionEvent($"{_unitName} used {_unitAttacksMoveSet[slotNumber].moveName}!");
                    StartCoroutine(StatUpdateDelayRoutine($"{_unitName} Missed!"));
                }
                _owner.isTurnComplete = true;
                _owner.turnCompleteChannel.RaiseTurnCompleteEvent(_characterType, _owner.isTurnComplete);
            }
            else if (usesLeft <= 0)
                return;
        }

        public void UseDefenseMoveSlot(int slotNumber)
        {
            int usesLeft = _defenseMoveUses[slotNumber];
            if (usesLeft > 0)
            {
                _defenseMoveUses[slotNumber]--;
                BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("defense", _defenseMoveUses[slotNumber], slotNumber);
                BattleUIManager.Instance.ActivateButtons(false);
                _unitDefensesMoveSet[slotNumber].RaiseDefenseMoveUsedEvent(_unitName);
                AdjustDefense(_unitDefensesMoveSet[slotNumber].defenseBuff);
                _owner.isTurnComplete = true;
                _owner.turnCompleteChannel.RaiseTurnCompleteEvent(_characterType,_owner.isTurnComplete);
            }
            else if (usesLeft <= 0)
            {
                if (_characterType != CharacterType.Player)
                    DetermineAction();
                else
                    return;
            }
                
        }
        private void DetermineAction()
        {
            var dieRoll = UnityEngine.Random.Range(1, 6);
            var attackToUse = UnityEngine.Random.Range(0, _unitAttacksMoveSet.Length);
            var defenseToUse = UnityEngine.Random.Range(0, _unitDefensesMoveSet.Length);
            //var itemToUse = Random.Range(0, _unitItems.Length);
            if (dieRoll + _owner.aIAggression >= 3)
                UseAttackMoveSlot(attackToUse);
            
            else if (dieRoll + _owner.aIAggression < 3)
                UseDefenseMoveSlot(defenseToUse);
        }
        protected IEnumerator StatUpdateDelayRoutine(string actionTakenText)
        {
            yield return new WaitForSeconds(2f);
            BattleUIManager.Instance.DisplayStatUpdateText(actionTakenText);
        }
        public IEnumerator TurnDelayRoutine()
        {
            yield return new WaitForSeconds(5f);
            DetermineAction();
        }
    }
}
