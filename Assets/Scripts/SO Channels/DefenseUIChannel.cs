using UnityEngine;
using UnityEngine.Events;

namespace Veganimus.BattleSystem
{
    [CreateAssetMenu(menuName = "Defense UI Channel")]
    public class DefenseUIChannel : ScriptableObject
    {
        public UnityEvent<int> OnDefenseChange;

        public void RaiseDefenseUIChange(int defense)
        {
            if (OnDefenseChange != null)
                OnDefenseChange.Invoke(defense);
        }
    }
}
