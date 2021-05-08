using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class PlayerUnit : Unit, IDamageable, IHealable, IDefendable
    {
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] private UnitHitPointUpdate _unitHPUpdateChannel;
        [SerializeField] private UnitMoveNameUpdate _unitMoveNameUpdateChannel;

        private void Start() => _unitNameUpdateChannel.RaiseUnitNameUpdateEvent("Player", _unitName);

        public void Damage(int amount)
        {
            _currentUnitHP -= (amount- _unitDefense);
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Player", _unitHitPoints, _currentUnitHP);
        }
        public void Heal(int amount)
        {
            _currentUnitHP += amount;
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Player", _unitHitPoints, _currentUnitHP);
        }
        public void AdjustDefense(int amount) => _unitDefense = amount;

        public void UpdateMoveNames(string moveType)
        {
            if (moveType == "Attack")
            {
                for (int i = _unitAttacksMoveSet.Length - 1; i >= 0; i--)
                {
                    _unitMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(_unitAttacksMoveSet[i].moveName, i);
                }
            }
            else if (moveType == "Defense")
            {
                for (int i = _unitDefensesMoveSet.Length - 1; i >= 0; i--)
                {
                    _unitMoveNameUpdateChannel.RaiseMoveNameUpdateEvent(_unitDefensesMoveSet[i].moveName, i);
                }
            }
        }
    }
}
