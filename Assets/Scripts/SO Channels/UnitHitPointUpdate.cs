using UnityEngine;
using UnityEngine.Events;
namespace Veganimus.BattleSystem
{
    [CreateAssetMenu(menuName = "Channels/Unit HP Update")]
    public class UnitHitPointUpdate : ScriptableObject
    {
        public UnityEvent<CharacterType,int, int> OnUnitHPUpdated;

        public void RaiseUnitHPUpdateEvent(CharacterType characterType, int maxUnitHP, int unitHP)
        {
            if (OnUnitHPUpdated != null)
            {
                OnUnitHPUpdated.Invoke(characterType, maxUnitHP, unitHP);
            }
        }
    }
}
