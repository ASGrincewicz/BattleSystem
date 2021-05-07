using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class PlayerUnit : Unit, IDamageable
    {
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] private UnitHitPointUpdate _unitHPUpdateChannel;

        private void Start() => _unitNameUpdateChannel.RaiseUnitNameUpdateEvent("Player", _unitName);

        public void Damage(int amount)
        {
            _currentUnitHP -= amount;
            _unitHPUpdateChannel.RaiseUnitHPUpdateEvent("Player", _unitHitPoints, _currentUnitHP);
        }
    }
}
