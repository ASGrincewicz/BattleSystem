using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class EnemyUnit : Unit
    {
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;

        private void Start()
        {
            _unitNameUpdateChannel.RaiseUnitNameUpdateEvent("Enemy",_unitName);
        }
    }
}
