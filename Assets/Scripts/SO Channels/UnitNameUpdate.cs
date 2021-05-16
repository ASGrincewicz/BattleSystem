using UnityEngine;
using UnityEngine.Events;
namespace Veganimus.BattleSystem
{
    [CreateAssetMenu(menuName ="Channels/Unit Name Update")]
    public class UnitNameUpdate : ScriptableObject
    {
        public UnityEvent<CharacterType, string> OnUnitNameUpdated;

        public void RaiseUnitNameUpdateEvent(CharacterType characterType,string unitName)
        {
            if(OnUnitNameUpdated != null)
            {
                OnUnitNameUpdated.Invoke(characterType, unitName);
            }
        }
    }
}
