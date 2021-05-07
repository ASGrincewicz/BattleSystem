using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class EnemyUnit : Unit, IDamageable
    {
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] private UnitHitPointUpdate _unitHPUpdateChannel;


        private void Start() => _unitNameUpdateChannel.RaiseUnitNameUpdateEvent("Enemy", _unitName);

        public void Damage(int amount)
        {
            _currentUnitHP -= amount;
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Enemy",_unitHitPoints, _currentUnitHP);
        }
    }
}
