using UnityEngine;
using UnityEngine.Events;

namespace Veganimus.BattleSystem
{
    public class UseButton : MonoBehaviour
    {
        [SerializeField] private Character _user;

        private void Start() => _user = GetComponent<Character>();

        public void UseAttack(int slotNumber) => _user.activeUnit.UseAttackMoveSlot(slotNumber);

        public void UseDefend(int slotNumber) => _user.activeUnit.UseDefenseMoveSlot(slotNumber);

        public void UseItem(int slotNumber) => _user.UseItemSlot(slotNumber);

        public void DisplayUnitStats() => _user.activeUnit.DisplayUnitStats();
    }
}