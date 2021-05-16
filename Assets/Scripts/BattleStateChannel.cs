using UnityEngine;

namespace Veganimus.BattleSystem
{
    [CreateAssetMenu(menuName = "Channels/ Battle State Change")]
    public class BattleStateChannel : ScriptableObject
    {
        public UnityEngine.Events.UnityEvent<BattleState> OnBattleStateChanged;

        public void RaiseBattleStateChangeEvent(BattleState battleState)
        {
            if (OnBattleStateChanged != null)
                OnBattleStateChanged.Invoke(battleState);
        }
    }
}
