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
        [SerializeField] protected ElementType _unitType;
        [SerializeField] protected string _unitName;
        [SerializeField] protected int _unitHitPoints;
        [SerializeField] protected int _currentUnitHP;
        [SerializeField] protected int _unitSpeed;
        [SerializeField] protected int _unitDefense;
        [SerializeField] protected UnitAttackMove[] _unitAttacksMoveSet = new UnitAttackMove[4];
        [SerializeField] protected UnitDefenseMove[] _unitDefensesMoveSet = new UnitDefenseMove[2];
        [SerializeField] protected UnitAttackMove _emptyAttackPlaceholder;
        [SerializeField] protected UnitDefenseMove _emptyDefensePlaceholder;

        [SerializeField] protected UnitMoveNameUpdate _unitAttackMoveNameUpdateChannel;
        [SerializeField] protected UnitMoveNameUpdate _unitDefenseMoveNameUpdateChannel;

        [SerializeField] protected LayerMask _target;

        //SO broadcast channel for turn completion
        //SO listener channel for BattleState change

        private void Awake() => _currentUnitHP = _unitHitPoints;

        public void UpdateMoveNames(string moveType)
        {
            if (moveType == "Attack")
            {
                for (int i = _unitAttacksMoveSet.Length - 1; i >= 0; i--)
                {
                    _unitAttackMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(_unitAttacksMoveSet[i].moveName, i);
                }
            }
            else if (moveType == "Defense")
            {
                for (int i = _unitDefensesMoveSet.Length - 1; i >= 0; i--)
                {
                    _unitDefenseMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(_unitDefensesMoveSet[i].moveName, i);
                }
            }
        }
        protected void AcquireTarget(int amount)
        {
            RaycastHit hitInfo;
            
            if (Physics.SphereCast(transform.position,2f,Vector3.forward, out hitInfo, Mathf.Infinity,_target))
            {
                if(hitInfo.collider != null)
                {
                    Debug.Log($"{hitInfo.collider.name} was targeted!");
                   var damage = hitInfo.collider.GetComponentInChildren<IDamageable>();
                    damage.Damage(amount);
                }
            }
        }
    }
}
