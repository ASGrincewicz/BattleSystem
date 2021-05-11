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
    public class Unit : MonoBehaviour
    {
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

        [SerializeField] protected UnitMoveNameUpdate _unitAttackMoveNameUpdateChannel;
        [SerializeField] protected UnitMoveNameUpdate _unitDefenseMoveNameUpdateChannel;
        [SerializeField] protected DisplayActionChannel _displayAttackActionChannel;
        [SerializeField] protected BattleStateChannel _endBattleChannel;
       
        private void Awake() => _currentUnitHP = _unitHitPoints;

        private void Start() => _animator = GetComponent<Animator>();

        public void SetMoveUses()
        {
            if (_attackMoveUses.Count > 0 || _defenseMoveUses.Count > 0)
                return;
            else
            {
                for (int i = 0; i < _unitAttacksMoveSet.Length; i++)
                {
                    _attackMoveUses.Add(_unitAttacksMoveSet[i].MoveUses);
                }
                for (int i = 0; i < _unitDefensesMoveSet.Length; i++)
                {
                    _defenseMoveUses.Add(_unitDefensesMoveSet[i].MoveUses);
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
                    _unitAttackMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(_unitAttacksMoveSet[i].MoveName, i);
                }
            }
            else if (moveType == "Defense")
            {
                for (int i = _unitDefensesMoveSet.Length - 1; i >= 0; i--)
                {
                    _unitDefenseMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(_unitDefensesMoveSet[i].MoveName, i);
                }
            }
        }
        public void UpdateMoveUseUI()
        {
            if (_attackMoveUses.Count > 0 || _defenseMoveUses.Count > 0)
                return;
            for (int i = _unitAttacksMoveSet.Length-1; i >= 0; i--)
            {
                if(_unitAttacksMoveSet[i].MoveUses >0)
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("attack", _unitAttacksMoveSet[i].MoveUses, i);
            }
            for (int i = _unitDefensesMoveSet.Length-1; i >= 0; i--)
            {
                if(_unitDefensesMoveSet[i].MoveUses > 0)
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("defense", _unitDefensesMoveSet[i].MoveUses, i);
            }
        }
        protected IEnumerator StatUpdateDelayRoutine(string actionTakenText)
        {
            yield return new WaitForSeconds(2f);
            BattleUIManager.Instance.DisplayStatUpdateText(actionTakenText);
        }
    }
}
