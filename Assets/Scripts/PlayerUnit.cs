using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class PlayerUnit : Unit
    {
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
       
        private void Start()
        {
            _unitNameUpdateChannel.RaiseUnitNameUpdateEvent("Player",_unitName);
        }
    }
}
