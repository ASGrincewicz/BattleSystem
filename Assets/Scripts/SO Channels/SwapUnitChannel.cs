// Aaron Grincewicz Veganimus@icloud.com 5/31/2021
using UnityEngine;
using UnityEngine.Events;

namespace Veganimus.BattleSystem
{
    [CreateAssetMenu(menuName="Swap Unit Channel")]
    public class SwapUnitChannel : ScriptableObject
    {
        public UnityEvent OnUnitSwap;

        public void RaiseUnitSwapEvent()
        {
            if (OnUnitSwap != null)
                OnUnitSwap.Invoke();
        }
    }
}
