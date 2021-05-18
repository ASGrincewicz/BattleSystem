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
    public class Unit : MonoBehaviour, IDamageable, IHealable, IDefendable, IBuffable
    {
        [SerializeField] protected Character _owner;
        public Character Owner { get { return _owner; } }
        [SerializeField] protected CharacterType _characterType;
        public UnitStats unitStats;
        protected int _unitLevel;
        [SerializeField] protected TargetFinder _targetUnit;
        public TargetFinder TargetUnit { get { return _targetUnit; } }
        [SerializeField] protected ElementType _unitType;
        [SerializeField] protected string _unitName;
        [SerializeField] protected int _unitHitPoints;
        [SerializeField] private int _currentUnitHP;
        [SerializeField] protected int _unitSpeed;
        [SerializeField] protected int _unitDefense;
        [SerializeField] protected int _accuracyModifier;
        [Header("Defense Effect Prefabs")]
        [SerializeField] private GameObject _unitBaseModel;
        public GameObject UnitBaseModel { get { return _unitBaseModel; } private set { _unitBaseModel = value; } }
        [SerializeField] private GameObject _unitEnergyShield;
        public GameObject UnitEnergyShield { get { return _unitEnergyShield; } private set { _unitEnergyShield = value; } }

        [SerializeField] private GameObject _unitBarrier;
        public GameObject UnitBarrier { get { return _unitBarrier; } private set { _unitBarrier = value; } }

        [SerializeField] private GameObject _unitCloak;
        public GameObject UnitCloak { get { return _unitCloak; } private set { _unitCloak = value; } }
        public GameObject ActiveEffect { get; set; }
        [Header("Runtime Assets")]
        [SerializeField] private GameObject _unitPrefab;
        public GameObject UnitPrefab { get { return _unitPrefab; } set { } }
        [SerializeField] private List<UnitAttackMove> _attackMoveSet = new List<UnitAttackMove>();
        [SerializeField] private List<UnitDefenseMove> _defenseMoveSet = new List<UnitDefenseMove>();
        [Space]
        [SerializeField] protected UnitAnimation _unitAnimation;
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] protected UnitMoveNameUpdate _unitAttackMoveNameUpdateChannel;
        [SerializeField] protected UnitMoveNameUpdate _unitDefenseMoveNameUpdateChannel;
        [SerializeField] protected UnitHitPointUpdate _unitHPUpdateChannel;
        [SerializeField] protected DisplayActionChannel _displayActionChannel;
        [SerializeField] protected BattleStateChannel _endBattleChannel;
        private WaitForSeconds _turnDelay;
        private WaitForSeconds _statUpdateDelay;
        private WaitForSeconds _endBattleDelay;
        private bool _isEffectActive;
        private void Awake()
        {
            _owner = GetComponentInParent<Character>();
            _turnDelay = new WaitForSeconds(5f);
            _statUpdateDelay = new WaitForSeconds(2f);
            _endBattleDelay = new WaitForSeconds(1.5f);
        }

        private IEnumerator Start()
        {
            _owner.activeUnit = this;
            _characterType = _owner.ThisCharacterType;
            yield return new WaitForSeconds(2f);
            PopulateRuntimeStats();
            GenerateMoveSet();
        }
        private void PopulateRuntimeStats()
        {
            _unitPrefab = _owner.activeUnitPrefab;
            _unitName = unitStats.UnitName;
            _unitHitPoints = unitStats.UnitHitPoints;
            _currentUnitHP = _unitHitPoints;
            _unitSpeed = unitStats.UnitSpeed;
            _unitDefense = unitStats.UnitDefense;
            _accuracyModifier = unitStats.UnitAccuracyModifier;
            var prefab = _unitPrefab.GetComponent<UnitPrefab>();
            _unitBaseModel = prefab.baseModel;
            _unitEnergyShield = prefab.shield;
            _unitEnergyShield.SetActive(false);
            _unitBarrier = prefab.barrier;
            _unitBarrier.SetActive(false);
            _unitCloak = prefab.cloak;
            _unitCloak.SetActive(false);
            _unitAnimation = GetComponentInChildren<UnitAnimation>();
            _unitNameUpdateChannel.RaiseUnitNameUpdateEvent(_characterType, _unitName);

        }
        public void GenerateMoveSet()
        {
            foreach(var attackMove in unitStats.UnitAttackMoves)
            {
                var attackCopy = Instantiate(attackMove);
                _attackMoveSet.Add(attackCopy);
            }
            foreach(var defenseMove in unitStats.UnitDefenseMoves)
            {
                var defenseCopy = Instantiate(defenseMove);
                _defenseMoveSet.Add(defenseCopy);
            }
        }
       
        public void DisplayUnitStats() => BattleUIManager.Instance.DisplayUnitStats(_currentUnitHP,
                                                                                    _unitHitPoints,
                                                                                    _unitSpeed,
                                                                                    _unitDefense,
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
                    BattleUIManager.Instance.DisplayMoveStats("attack", move.damageAmount, move.moveAccuracy,0, 0, a);
                }
            }
            else if (moveType == "Defense")
            {
                for (int d = _defenseMoveSet.Count - 1; d >= 0; d--)
                {
                    var move = _defenseMoveSet[d];
                    _unitDefenseMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(move.moveName, d);
                    BattleUIManager.Instance.DisplayMoveStats("defense", 0, 0, move.defenseBuff,move.turnsActive, d);
                }
            }
        }
        public void UpdateMoveUseUI()
        {
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
            int damage = amount -= _unitDefense;
            if (damage <= 0)
                damage = 0;

            _currentUnitHP -= damage;
            if (_currentUnitHP <= 0)
            {
                _currentUnitHP = 0;
                if (ActiveEffect != null)
                {
                    ActiveEffect.SetActive(false);
                    UnitBaseModel.SetActive(true);
                }

                _unitAnimation.PlayClip("Death");
                _unitHPUpdateChannel.RaiseUnitHPUpdateEvent(_characterType, _unitHitPoints, _currentUnitHP);
                StartCoroutine(StatUpdateDelayRoutine($"{_owner.CharacterName} {_unitName} took {damage} damage!"));
                StartCoroutine(EndBattleDelayRoutine());
            }
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent(_characterType, _unitHitPoints, _currentUnitHP);
            StartCoroutine(StatUpdateDelayRoutine($"{_owner.CharacterName} {_unitName} took {damage} damage!"));
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
                StartCoroutine(StatUpdateDelayRoutine($"{_owner.CharacterName} {_unitName} healed {amount} HP!"));
            }
            else
                return;
        }
        public void AdjustDefense(int amount)
        {
            _unitDefense += amount;
            StartCoroutine(StatUpdateDelayRoutine(($"{_owner.CharacterName} {_unitName} raised Defense by {amount}.")));
        }
        public void ResetDefense()
        {
            _isEffectActive = false;
            if(!_unitBaseModel.activeInHierarchy)
            {
                _unitBaseModel.SetActive(true);
                ActiveEffect = null;
            }
            _unitDefense = unitStats.UnitDefense;
            TargetUnit.targetIBuffable.BuffStats(StatAffected.Accuracy, 10);// Need to change this so it's not hard coded.
            StartCoroutine(StatUpdateDelayRoutine($"{_owner.CharacterName} {_unitName} Defense was reset."));
            StartCoroutine(ResetStatDelayRoutine(2));
            
        }
        public void BuffStats(StatAffected statAffected, int amount)
        {
            switch(statAffected)
            {
                case StatAffected.Speed:
                    _unitSpeed += amount;
                    break;
                case StatAffected.Defense:
                    _unitDefense += amount;
                    break;
                case StatAffected.Accuracy:
                    _accuracyModifier += amount;
                    break;

            }
            StartCoroutine(StatUpdateDelayRoutine($"{_owner.CharacterName} {_unitName} raised {statAffected} by {amount}."));
        }

        public void UseAttackMoveSlot(int slotNumber)
        {
            var move = _attackMoveSet[slotNumber];
            if (_attackMoveSet[slotNumber].moveUses > 0)
            {
                _displayActionChannel.RaiseDisplayActionEvent($"{_owner.CharacterName} {_unitName} used {move.moveName}!");
                _attackMoveSet[slotNumber].moveUses--;
                _unitAnimation.SetInteger(move.MoveAttackType.ToString(), 1);
                if (_owner.ThisCharacterType == CharacterType.Player)
                {
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("attack", _attackMoveSet[slotNumber].moveUses, slotNumber);
                    BattleUIManager.Instance.ActivateButtons(false);
                }
                bool didMoveHit = move.RollForMoveAccuracy(_accuracyModifier);
                if (didMoveHit == true)
                {
                    Debug.Log(move.MoveAttackType.ToString());
                    int damageAmount = move.damageAmount;
                    _targetUnit.targetIDamageable.Damage(damageAmount);
                  
                   // move.RaiseAttackMoveUsedEvent(this,move.MoveAttackType);
                }
                else if (didMoveHit == false)
                {
                    StartCoroutine(StatUpdateDelayRoutine($"{_owner.CharacterName} {_unitName} Missed!"));
                }
                _owner.IsTurnComplete = true;
                _owner.TurnCompleteChannel.RaiseTurnCompleteEvent(_characterType, _owner.IsTurnComplete);
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

            if (CheckIfEffectActive() && usesLeft > 0)
                {
                    _displayActionChannel.RaiseDisplayActionEvent($"{_owner.CharacterName} {_unitName} used {move.moveName}!");
                    _defenseMoveSet[slotNumber].moveUses--;
                    if (_owner.ThisCharacterType == CharacterType.Player)
                    {
                        BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("defense", _defenseMoveSet[slotNumber].moveUses, slotNumber);
                        BattleUIManager.Instance.ActivateButtons(false);
                    }
                _isEffectActive = true;
                    move.RaiseDefenseMoveUsedEvent(this, move.MoveDefenseType);
                if (move.MoveDefenseType != UnitDefenseMove.DefenseType.Cloak)
                    AdjustDefense(move.defenseBuff);
                else
                    StartCoroutine(StatUpdateDelayRoutine($"{_owner.CharacterName} {_unitName} is harder to hit!"));

                    _owner.IsTurnComplete = true;
                    _owner.TurnCompleteChannel.RaiseTurnCompleteEvent(_characterType, _owner.IsTurnComplete);
                }
                else if (_defenseMoveSet[slotNumber].moveUses <= 0 || CheckIfEffectActive()==false)
                {
                    if (_owner.ThisCharacterType == CharacterType.Player)
                        BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("defense", _defenseMoveSet[slotNumber].moveUses, slotNumber);

                    if (_characterType != CharacterType.Player)
                        DetermineAction();
                    else
                        return;
                }
            
        }
        private bool CheckIfEffectActive()
        {
            if (!_isEffectActive)
            {
                if (!_unitEnergyShield.activeInHierarchy)
                    return true;

                else if (!_unitBarrier.activeInHierarchy)
                    return true;

                else if (!_unitCloak.activeInHierarchy)
                    return true;
                else
                    return false;
            }
            else
                return false;

        }
        ///<summary>
        ///Uses a dice roll system to determine what Action an AI character will take.
        ///</summary>
        public void DetermineAction()//Move to character
        {
            var dieRoll = UnityEngine.Random.Range(0, 6);
            var attackToUse = UnityEngine.Random.Range(0, _attackMoveSet.Count);
            var defenseToUse = UnityEngine.Random.Range(0, _defenseMoveSet.Count);
            var itemToUse = UnityEngine.Random.Range(0, _owner.ThisInventory.battleInventory.Count);
            //var itemToUse = Random.Range(0, _unitItems.Length);
            if (dieRoll + _owner.AIAgression >= 3)
                UseAttackMoveSlot(attackToUse);

            else if (dieRoll + _owner.AIAgression < 3 && dieRoll + _owner.AIAgression > 0)
                UseDefenseMoveSlot(defenseToUse);

            else if (dieRoll + _owner.AIAgression <= 0)
                _owner.UseItemSlot(itemToUse);
        }
        private IEnumerator ResetStatDelayRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
        private IEnumerator StatUpdateDelayRoutine(string actionTakenText)
        {
            yield return _statUpdateDelay;
            BattleUIManager.Instance.DisplayStatUpdateText(actionTakenText);
        }
        public IEnumerator TurnDelayRoutine()
        {
            yield return _turnDelay;
            DetermineAction();
        }
        private IEnumerator EndBattleDelayRoutine()
        {
            yield return _endBattleDelay;
            if (_characterType == CharacterType.Player)
                _endBattleChannel.RaiseBattleStateChangeEvent(BattleState.Lose);
            else
                _endBattleChannel.RaiseBattleStateChangeEvent(BattleState.Win);
        }
    }
}