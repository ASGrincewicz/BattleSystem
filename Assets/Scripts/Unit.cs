using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
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
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///@info:Assigned to a Unit Prefab to determine Stats, Moves, etc...
    ///</summary>
    public class Unit : MonoBehaviour, IDamageable, IHealable,IDefendable
    {
        [SerializeField] protected Character _owner;
        [SerializeField] protected CharacterType _characterType;
        [SerializeField] private UnitStats _unitStats;
        protected int _unitLevel;
        [SerializeField] protected TargetFinder _targetUnit;
        [SerializeField] protected ElementType _unitType;
        [SerializeField] protected string _unitName => _unitStats.unitName;
        [SerializeField] protected int _unitHitPoints => _unitStats.unitHitPoints;
        [SerializeField] protected int _currentUnitHP;
        [SerializeField] protected int _unitSpeed => _unitStats.unitSpeed;
        [SerializeField]
        protected int UnitDefense
        {
            get => _unitStats.unitDefense; private set { }
        }
        [SerializeField] protected int _accuracyModifier => _unitStats.accuracyModifier;
        [Header("Runtime Assets")]
        [SerializeField] private List<UnitAttackMove> _attackMoveSet = new List<UnitAttackMove>();
        [SerializeField] private List<UnitDefenseMove> _defenseMoveSet = new List<UnitDefenseMove>();
       [Space]
        protected Animator _animator;
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] protected UnitMoveNameUpdate _unitAttackMoveNameUpdateChannel;
        [SerializeField] protected UnitMoveNameUpdate _unitDefenseMoveNameUpdateChannel;
        [SerializeField] protected UnitHitPointUpdate _unitHPUpdateChannel;
        [SerializeField] protected DisplayActionChannel _displayAttackActionChannel;
        [SerializeField] protected BattleStateChannel _endBattleChannel;
        [SerializeField] private PlayableDirector _director;
        [SerializeField] private List<GameObject> _moveCutScenes = new List<GameObject>();
        [Header("Asset Assignments")]
        [SerializeField] protected UnitAttackMove[] _unitAttacksMoveSet = new UnitAttackMove[4];
        [SerializeField] protected UnitDefenseMove[] _unitDefensesMoveSet = new UnitDefenseMove[2];


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
            GenerateMoveSet();
        }
        public void GenerateMoveSet()
        {
            foreach(var attackMove in _unitStats.unitAttackMoves)
            {
                var attackCopy = Instantiate(attackMove);
                _attackMoveSet.Add(attackCopy);
            }
            foreach(var defenseMove in _unitStats.unitDefenseMoves)
            {
                var defenseCopy = Instantiate(defenseMove);
                _defenseMoveSet.Add(defenseCopy);
            }
        }
        public void DisplayUnitStats() => BattleUIManager.Instance.DisplayUnitStats(_currentUnitHP,
                                                                                    _unitHitPoints,
                                                                                    _unitSpeed,
                                                                                    UnitDefense,
                                                                                    _accuracyModifier);
       
        public void UpdateMoveNames(string moveType)
        {
            UpdateMoveUseUI();
            
            if (moveType == "Attack")
            {
                for (int a = _attackMoveSet.Count - 1; a >= 0; a--)
                {
                    var move = _attackMoveSet[a];
                    _unitAttackMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(move.moveName, a);
                    BattleUIManager.Instance.DisplayMoveStats("attack", move.damageAmount, move.moveAccuracy, 0, a);
                }
            }
            else if (moveType == "Defense")
            {
                for (int d = _defenseMoveSet.Count - 1; d >= 0; d--)
                {
                    var move = _defenseMoveSet[d];
                    _unitDefenseMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(move.moveName, d);
                    BattleUIManager.Instance.DisplayMoveStats("defense", 0, 0, move.defenseBuff, d);
                }
            }
        }
        public void UpdateMoveUseUI()
        {
            //if (_attackMoveUses.Count > 0 || _defenseMoveUses.Count > 0)
            //    return;
            for (int a = _attackMoveSet.Count-1; a >= 0; a--)
            {
                if(_attackMoveSet[a].moveUses >0)
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("attack", _attackMoveSet[a].moveUses, a);
            }
            for (int d = _defenseMoveSet.Count-1; d >= 0; d--)
            {
                if(_defenseMoveSet[d].moveUses > 0)
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("defense", _defenseMoveSet[d].moveUses, d);
            }
        }
        public void Damage(int amount)
        {
            var damage = amount -= UnitDefense;
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
            if (amount + _currentUnitHP > _unitHitPoints)
                amount = _unitHitPoints - _currentUnitHP;

            if (_currentUnitHP <= _unitHitPoints)
            {
                _currentUnitHP += amount;
                if (_currentUnitHP > _unitHitPoints)
                    _currentUnitHP = _unitHitPoints;

                _unitHPUpdateChannel.RaiseUnitHPUpdateEvent(_characterType, _unitHitPoints, _currentUnitHP);
                StartCoroutine(StatUpdateDelayRoutine($"{_unitName} healed {amount} HP!"));
            }
            else
                return;
        }
        public void AdjustDefense(int amount)
        {
            UnitDefense += amount;
            StartCoroutine(StatUpdateDelayRoutine(($"{_unitName} raised Defense by {amount}.")));
        }
        public void UseAttackMoveSlot(int slotNumber)
        {
            var move = _attackMoveSet[slotNumber];
            if (_attackMoveSet[slotNumber].moveUses > 0)
            {
                _attackMoveSet[slotNumber].moveUses--;
                if (_owner.thisCharacterType == CharacterType.Player)
                {
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("attack", _attackMoveSet[slotNumber].moveUses, slotNumber);
                    BattleUIManager.Instance.ActivateButtons(false);
                }
                bool didMoveHit = move.RollForMoveAccuracy(_accuracyModifier);
                if (didMoveHit == true)
                {
                    int damageAmount = move.damageAmount;
                    _targetUnit.targetIDamageable.Damage(damageAmount);
                    move.RaiseAttackMoveUsedEvent(_unitName, this.transform, slotNumber);
                }
                else if (didMoveHit == false)
                {
                    _displayAttackActionChannel.RaiseDisplayActionEvent($"{_unitName} used {move.moveName}!");
                    StartCoroutine(StatUpdateDelayRoutine($"{_unitName} Missed!"));
                }
                _owner.isTurnComplete = true;
                _owner.turnCompleteChannel.RaiseTurnCompleteEvent(_characterType, _owner.isTurnComplete);
            }
            else if (_attackMoveSet[slotNumber].moveUses <= 0)
            {
                BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("attack", _attackMoveSet[slotNumber].moveUses, slotNumber);
                if (_characterType != CharacterType.Player)
                    DetermineAction();
                else
                    return;
            }
        }

        public void UseDefenseMoveSlot(int slotNumber)
        {
            int usesLeft = _defenseMoveSet[slotNumber].moveUses;
            var move = _defenseMoveSet[slotNumber];
            if (usesLeft > 0)
            {
                _defenseMoveSet[slotNumber].moveUses--;
                if (_owner.thisCharacterType == CharacterType.Player)
                {
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("defense", _defenseMoveSet[slotNumber].moveUses, slotNumber);
                    BattleUIManager.Instance.ActivateButtons(false);
                }
                move.RaiseDefenseMoveUsedEvent(_unitName);
                AdjustDefense(move.defenseBuff);
                _owner.isTurnComplete = true;
                _owner.turnCompleteChannel.RaiseTurnCompleteEvent(_characterType,_owner.isTurnComplete);
            }
            else if (_defenseMoveSet[slotNumber].moveUses <= 0)
            {
                if(_owner.thisCharacterType == CharacterType.Player)
                BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("defense", _defenseMoveSet[slotNumber].moveUses, slotNumber);

                if (_characterType != CharacterType.Player)
                    DetermineAction();
                else
                    return;
            }
                
        }
        ///<summary>
        ///Uses a dice roll system to determine what Action an AI character will take.
        ///</summary>
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
        //private IEnumerator MoveAnimationRoutine()
        //{
           
        //}
    }
}
