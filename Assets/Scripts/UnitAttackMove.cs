using System;
using UnityEngine;
using UnityEngine.Events;
namespace Veganimus.BattleSystem
{
    public class UnitAttackMove : UnitMove, IComparable<UnitAttackMove>
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
        #region Compare
        public int Id => damageAmount;
        public int SecondID => (int)MoveAccuracy;
        public int ThirdID => (int)_attackType;

        public int CompareTo(UnitAttackMove other)
        {
            if (this.Id < other.Id)
                return -1;

            else if (this.Id > other.Id)
                return 1;

            else if (Id == other.Id)
            {
                if (SecondID < other.SecondID)
                    return -1;

                else if (this.SecondID > other.SecondID)
                    return 1;

                else if (this.SecondID == other.SecondID)
                {
                    if (this.ThirdID < other.ThirdID)
                        return -1;

                    else if (this.ThirdID > other.ThirdID)
                        return 1;

                    else
                        return 0;

                }
                else
                    return 0;

            }
            else
                return 0;
        }
        #endregion
    }

}