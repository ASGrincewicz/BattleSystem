using UnityEngine;
using UnityEngine.Events;
namespace Veganimus.BattleSystem
{
    [CreateAssetMenu(menuName = "Channels/Unit HP Update")]
    public class UnitHitPointUpdate : ScriptableObject
    {
        public UnityEvent<string,int, int> OnUnitHPUpdated;

        public void RaiseUnitHPUpdateEvent(string unit, int maxUnitHP, int unitHP)
        {
            if (OnUnitHPUpdated != null)
            {
                OnUnitHPUpdated.Invoke(unit, maxUnitHP, unitHP);
            }
        }
    }
}
