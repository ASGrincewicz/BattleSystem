using UnityEngine;
using UnityEngine.Events;
namespace Veganimus.BattleSystem
{
    public class UnitAttackMove : UnitMove
    {
        public int damageAmount;
        public int attackSpeed;
        public enum AttackType { quickShot, rifle, missile, rockets, laser }
        [SerializeField] private AttackType _attackType;
        public AttackType MoveAttackType { get { return _attackType; } }

        public UnityEvent<int, Transform> OnAttackMoveUsed;

        public void RaiseAttackMoveUsedEvent(Unit unit, AttackType attackType)
        {
            //PerformAttackMove(unit, attackType);
        }

        private void PerformAttackMove(Unit unit, AttackType attackType)
        {
          
        }
    }
}