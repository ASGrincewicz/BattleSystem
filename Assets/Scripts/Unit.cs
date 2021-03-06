using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///@info:Assigned to a Unit Prefab to determine Stats, Moves, etc...
    ///</summary>
    public class Unit : MonoBehaviour, IDamageable, IHealable, IDefendable, IBuffable
    {
        [SerializeField] private List<uint> _runTimeMoveUses = new List<uint>();
        [SerializeField] protected Character _owner;
        public Character Owner { get { return _owner; } }
        [SerializeField] protected CharacterType _characterType;
        public UnitStats unitStats;
        //private int _unitLevel;
        [SerializeField] protected TargetFinder _targetUnit;
        public TargetFinder TargetUnit { get { return _targetUnit; } }

        [Header("Runtime Unit Stats")]
        [SerializeField] private UnitInfo _runTimeUnitInfo;
        public UnitInfo RunTimeUnitInfo { get { return _runTimeUnitInfo; } }
        [SerializeField] private int _currentUnitHP;
        public int CurrentUnitHP { get { return _currentUnitHP; } }

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
        public List<UnitAttackMove> AttackMoveSet { get { return _attackMoveSet; } }
        [SerializeField] private List<UnitDefenseMove> _defenseMoveSet = new List<UnitDefenseMove>();
        public List<UnitDefenseMove> DefenseMoveSet { get { return _defenseMoveSet; } }
        [Space]
        [SerializeField] protected UnitAnimation _unitAnimation;
        [Header("Broadcasting On:")]
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] protected UnitMoveNameUpdate _unitAttackMoveNameUpdateChannel;
        [SerializeField] protected UnitMoveNameUpdate _unitDefenseMoveNameUpdateChannel;
        [SerializeField] protected UnitHitPointUpdate _unitHPUpdateChannel;
        [SerializeField] protected DisplayActionChannel _displayActionChannel;
        [SerializeField] protected BattleStateChannel _endBattleChannel;
        [SerializeField] private CameraShakeChannel _cameraShakeChannel;
        [SerializeField] private DefenseUIChannel _defenseUIChannel;
        [Header("Listening To:")]
        [SerializeField] private SwapUnitChannel _swapUnitChannel;

        private WaitForSeconds _statUpdateDelay;
        private WaitForSeconds _endBattleDelay;
        private bool _isEffectActive;
        private string _actionAnnouncementAbbrev;

        private void Awake()
        {
            _owner = GetComponentInParent<Character>();
            _statUpdateDelay = new WaitForSeconds(2f);
            _endBattleDelay = new WaitForSeconds(1.5f);
        }

        private void OnEnable()
        {
            if (_swapUnitChannel == null)
                _swapUnitChannel = ScriptableObject.CreateInstance<SwapUnitChannel>();

            _swapUnitChannel.OnUnitSwap.AddListener(Start);
        }
        private void OnDisable() => _swapUnitChannel.OnUnitSwap.RemoveListener(Start);

        private void Start() => StartCoroutine(BootUp());

        private IEnumerator BootUp()
        {
            _owner.activeUnit = this;
            _characterType = _owner.ThisCharacterType;
            yield return new WaitForSeconds(2f);
            PopulateRuntimeStats();
            GenerateMoveSet();
            UpdateMoveNames();
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent(_characterType, _runTimeUnitInfo.hitPoints, _currentUnitHP);
            yield return new WaitForSeconds(2.0f);

        }
        private void PopulateRuntimeStats()
        {
            _runTimeUnitInfo = new UnitInfo();
            _unitPrefab = _owner.activeUnitPrefab;
            _runTimeUnitInfo.unitName = unitStats.UnitName;
            _runTimeUnitInfo.hitPoints = unitStats.UnitHitPoints;
            _currentUnitHP = _runTimeUnitInfo.hitPoints;
            _runTimeUnitInfo.speed = unitStats.UnitSpeed;
            _runTimeUnitInfo.defense = unitStats.UnitDefense;
            _runTimeUnitInfo.accuracyMod = unitStats.UnitAccuracyModifier;
            _actionAnnouncementAbbrev = $"{_owner.CharacterName} {_runTimeUnitInfo.unitName}";
            CheckPrefabs();
            _unitAnimation = GetComponentInChildren<UnitAnimation>();
            _defenseUIChannel.RaiseDefenseUIChange(_runTimeUnitInfo.defense);
            _unitNameUpdateChannel.RaiseUnitNameUpdateEvent(_characterType, _runTimeUnitInfo.unitName);

        }
        private void CheckPrefabs()
        {
            var prefab = _unitPrefab.GetComponent<UnitPrefab>();
            _unitBaseModel = prefab.baseModel;
            _unitEnergyShield = prefab.shield;
            if (_unitEnergyShield)
                _unitEnergyShield.SetActive(false);

            _unitBarrier = prefab.barrier;
            if (_unitBarrier)
                _unitBarrier.SetActive(false);

            _unitCloak = prefab.cloak;
            if (_unitCloak)
                _unitCloak.SetActive(false);
        }
        public void GenerateMoveSet()
        {
            _attackMoveSet.Clear();
            _defenseMoveSet.Clear();
            _runTimeMoveUses.Clear();
            foreach (var attackMove in unitStats.UnitAttackMoves)
            {
                var attackCopy = Instantiate(attackMove);
                _attackMoveSet.Add(attackCopy);
                _attackMoveSet.Sort();
                _runTimeMoveUses.Add(attackCopy.runtimeUses);
            }
            foreach (var defenseMove in unitStats.UnitDefenseMoves)
            {
                var defenseCopy = Instantiate(defenseMove);
                _defenseMoveSet.Add(defenseCopy);
                _runTimeMoveUses.Add(defenseCopy.runtimeUses);
            }
        }

        public void DisplayUnitStats() => BattleUIManager.Instance.DisplayUnitStats(_currentUnitHP,
                                                                                    _runTimeUnitInfo.hitPoints,
                                                                                    _runTimeUnitInfo.speed,
                                                                                    _runTimeUnitInfo.defense,
                                                                                   _runTimeUnitInfo.accuracyMod);

        public void UpdateMoveNames()
        {
            for (int a = _attackMoveSet.Count - 1; a >= 0; a--)
            {
                var move = _attackMoveSet[a];
                _unitAttackMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(move.MoveName, a);
                BattleUIManager.Instance.DisplayMoveStats("attack", move.damageAmount, move.MoveAccuracy, 0, 0, a);
                if (_attackMoveSet[a].runtimeUses != 0)
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("attack", _attackMoveSet[a].runtimeUses, a);
            }
            for (int d = _defenseMoveSet.Count - 1; d >= 0; d--)
            {
                var move = _defenseMoveSet[d];
                _unitDefenseMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(move.MoveName, d);
                BattleUIManager.Instance.DisplayMoveStats("defense", 0, 0, move.defenseBuff, move.turnsActive, d);
                if (_defenseMoveSet[d].runtimeUses > 0)
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("defense", _defenseMoveSet[d].runtimeUses, d);
            }
        }
        public void Damage(int amount)
        {
            int damage = amount -= _runTimeUnitInfo.defense;
            if (damage <= 0)
                damage = 0;
            if (_owner.ThisCharacterType == CharacterType.Player)
            {
                var magnitude = damage / 100f;
                _cameraShakeChannel.RaiseCameraShakeEvent(magnitude);
            }

            _currentUnitHP -= damage;
            if (_currentUnitHP <= 0)
            {
                _currentUnitHP = 0;
                if (ActiveEffect != null)
                {
                    ActiveEffect.SetActive(false);
                    UnitBaseModel.SetActive(true);
                }

                _unitAnimation.SetInteger("hitPoints", 0);
                _unitHPUpdateChannel.RaiseUnitHPUpdateEvent(_characterType, _runTimeUnitInfo.hitPoints, _currentUnitHP);
                StartCoroutine(StatUpdateDelayRoutine($"{_actionAnnouncementAbbrev} took {damage} damage!"));
                StartCoroutine(EndBattleDelayRoutine());
            }
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent(_characterType, _runTimeUnitInfo.hitPoints, _currentUnitHP);
            StartCoroutine(StatUpdateDelayRoutine($"{_actionAnnouncementAbbrev} took {damage} damage!"));
        }
        public void Heal(int amount)
        {
            if (amount + _currentUnitHP > _runTimeUnitInfo.hitPoints)
                amount = _runTimeUnitInfo.hitPoints - _currentUnitHP;

            if (_currentUnitHP <= _runTimeUnitInfo.hitPoints)
            {
                _currentUnitHP += amount;
                if (_currentUnitHP > _runTimeUnitInfo.hitPoints)
                    _currentUnitHP = _runTimeUnitInfo.hitPoints;

                _unitHPUpdateChannel.RaiseUnitHPUpdateEvent(_characterType, _runTimeUnitInfo.hitPoints, _currentUnitHP);
                StartCoroutine(StatUpdateDelayRoutine($"{_actionAnnouncementAbbrev} healed {amount} HP!"));
            }
            else
                return;
        }
        public void AdjustDefense(int amount)
        {
            _runTimeUnitInfo.defense += amount;
            _defenseUIChannel.RaiseDefenseUIChange(_runTimeUnitInfo.defense);
            StartCoroutine(StatUpdateDelayRoutine(($"{_actionAnnouncementAbbrev} raised Defense by {amount}.")));
        }
        public void ResetDefense()
        {
            _isEffectActive = false;
            if (!_unitBaseModel.activeInHierarchy)
            {
                _unitBaseModel.SetActive(true);
                ActiveEffect = null;
            }
            _runTimeUnitInfo.defense = unitStats.UnitDefense;
            _defenseUIChannel.RaiseDefenseUIChange(_runTimeUnitInfo.defense);
            StartCoroutine(ResetStatDelayRoutine(2));
        }
        public void BuffStats(StatAffected statAffected, int amount)
        {
            switch (statAffected)
            {
                case StatAffected.Speed:
                    _runTimeUnitInfo.speed += amount;
                    break;
                case StatAffected.Defense:
                    _runTimeUnitInfo.defense += amount;
                    break;
                case StatAffected.Accuracy:
                    _runTimeUnitInfo.accuracyMod += amount;
                    break;
            }
            StartCoroutine(StatUpdateDelayRoutine($"{_actionAnnouncementAbbrev} raised {statAffected} by {amount}."));
        }

        public void UseAttackMoveSlot(int slotNumber)
        {
            var move = _attackMoveSet[slotNumber];
            if (_attackMoveSet[slotNumber].runtimeUses > 0)
            {
                _displayActionChannel.RaiseDisplayActionEvent($"{_actionAnnouncementAbbrev} used {move.MoveName}!");
                _attackMoveSet[slotNumber].runtimeUses--;
                _runTimeMoveUses[slotNumber]--;
                if (_unitAnimation != null)
                    _unitAnimation.SetInteger(move.MoveAttackType.ToString(), 1);

                if (_owner.ThisCharacterType == CharacterType.Player)
                {
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("attack", _attackMoveSet[slotNumber].runtimeUses, slotNumber);
                    BattleUIManager.Instance.ActivateButtons(false);
                }
                bool didMoveHit = move.RollForMoveAccuracy(_runTimeUnitInfo.accuracyMod);
                if (didMoveHit)
                {
                    int damageAmount = move.damageAmount;
                    _targetUnit.targetIDamageable.Damage(damageAmount);
                    move.RaiseAttackMoveUsedEvent(this, move.MoveAttackType);
                }
                else if (!didMoveHit)
                    StartCoroutine(StatUpdateDelayRoutine($"{_actionAnnouncementAbbrev} Missed!"));

                _owner.IsTurnComplete = true;
                _owner.TurnCompleteChannel.RaiseTurnCompleteEvent(_characterType, _owner.IsTurnComplete);
            }
            else if (_attackMoveSet[slotNumber].runtimeUses <= 0)
            {
                BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("attack", _attackMoveSet[slotNumber].runtimeUses, slotNumber);
                if (_characterType != CharacterType.Player)
                    _owner.DetermineAction();
                else
                    return;
            }
        }

        public void UseDefenseMoveSlot(int slotNumber)
        {
            uint usesLeft = _defenseMoveSet[slotNumber].runtimeUses;
            var move = _defenseMoveSet[slotNumber];

            if (CheckIfEffectActive() && usesLeft > 0)
            {
                _displayActionChannel.RaiseDisplayActionEvent($"{_actionAnnouncementAbbrev} used {move.MoveName}!");
                _defenseMoveSet[slotNumber].runtimeUses--;

                if (_owner.ThisCharacterType == CharacterType.Player)
                {
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("defense", _defenseMoveSet[slotNumber].runtimeUses, slotNumber);
                    BattleUIManager.Instance.ActivateButtons(false);
                }
                _isEffectActive = true;
                move.RaiseDefenseMoveUsedEvent(this, move.MoveDefenseType);
                if (move.MoveDefenseType != UnitDefenseMove.DefenseType.Cloak)
                    AdjustDefense(move.defenseBuff);
                else
                    StartCoroutine(StatUpdateDelayRoutine($"{_actionAnnouncementAbbrev} is harder to hit!"));

                _owner.IsTurnComplete = true;
                _owner.TurnCompleteChannel.RaiseTurnCompleteEvent(_characterType, _owner.IsTurnComplete);
            }
            else if (_defenseMoveSet[slotNumber].runtimeUses <= 0 || CheckIfEffectActive() == false)
            {
                if (_owner.ThisCharacterType == CharacterType.Player)
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("defense", _defenseMoveSet[slotNumber].runtimeUses, slotNumber);

                if (_characterType != CharacterType.Player)
                    _owner.DetermineAction();
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
        private IEnumerator ResetStatDelayRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
        private IEnumerator StatUpdateDelayRoutine(string actionTakenText)
        {
            yield return _statUpdateDelay;
            BattleUIManager.Instance.DisplayStatUpdateText(actionTakenText);
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