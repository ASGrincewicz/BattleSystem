using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class EnemyUnit : Unit, IDamageable, IHealable, IDefendable
    {
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] private UnitHitPointUpdate _unitHPUpdateChannel;
       
        private void Start() => _unitNameUpdateChannel.RaiseUnitNameUpdateEvent("Enemy", _unitName);

        public void Damage(int amount)
        {
            _currentUnitHP -= amount - _unitDefense;
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Enemy",_unitHitPoints, _currentUnitHP);
        }
        public void Heal(int amount)
        {
            _currentUnitHP += amount;
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Enemy", _unitHitPoints, _currentUnitHP);
        }
        public void AdjustDefense(int amount) => _unitDefense = amount;
    }
}
