using UnityEngine;
using UnityEngine.Events;
namespace Veganimus.BattleSystem
{
    [CreateAssetMenu(menuName ="Channels/Unit Name Update")]
    public class UnitNameUpdate : ScriptableObject
    {
        public UnityEvent<string, string> OnUnitNameUpdated;

        public void RaiseUnitNameUpdateEvent(string unit,string unitName)
        {
            if(OnUnitNameUpdated != null)
            {
                OnUnitNameUpdated.Invoke(unit, unitName);
            }
        }
    }
}
