using System.Collections.Generic;
using UnityEngine;

namespace Veganimus.BattleSystem
{
    /// <summary>
    /// Determines if the Character is a User or AI
    /// </summary>
    public enum CharacterType { Player, Enemy, Ally, EnemyAlly }
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///@info: Represents either the User as "Player" or an AI.
    ///</summary>
    public class Character : MonoBehaviour
    {
        public CharacterType thisCharacterType;
        [SerializeField] private string _characterName;
        [Range(-1, 1)] public int aIAggression;
        [SerializeField] private List<Unit> _party = new List<Unit>();
        private Inventory _inventory;
        public Unit activeUnit;
        public int turnCount;
        public bool isDefeated;
        [Header("Broadcasting on")]
        public TurnCompleteChannel turnCompleteChannel;
        [SerializeField] private DisplayActionChannel _displayActionChannel;
        [SerializeField] private UnitMoveNameUpdate _itemNameUpdateChannel;
        [Space]
        [Header("Listening to:")]
        public CharacterTurnChannel characterTurnChannel;
        [Space]
        public bool isTurnComplete;

        private void OnEnable() => characterTurnChannel.OnCharacterTurn.AddListener(InitiateCharacterTurn);

        private void OnDisable() => characterTurnChannel.OnCharacterTurn.RemoveListener(InitiateCharacterTurn);

        private void Start() => _inventory = GetComponent<Inventory>();

        private void InitiateCharacterTurn(CharacterType characterType)
        {
            if (characterType == thisCharacterType)
            {
                isTurnComplete = false;
                turnCompleteChannel.RaiseTurnCompleteEvent(characterType, isTurnComplete);
                if (thisCharacterType != CharacterType.Player && isTurnComplete == false)
                {
                    //activeUnit.GenerateMoveSet();
                    StartCoroutine(activeUnit.TurnDelayRoutine());
                }
            }
        }
        public void UpdateItemNames()
        {
            UpdateItemUseUI();
            for (int i = _inventory.battleInventory.Count - 1; i >= 0; i--)
            {
                var item = _inventory.battleInventory[i];
                _itemNameUpdateChannel.RaiseMoveNameUpdateEvent(item.itemName, i);
            }
        }
        private void UpdateItemUseUI()
        {
           
            for (int i = _inventory.battleInventory.Count - 1; i >= 0; i--)
            {
                var item = _inventory.battleInventory[i];
                int usesLeft = _inventory.battleInventory[i].itemUses;
                if (usesLeft >= 0)
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("item", usesLeft, i);
                else
                    return;
            }
        }
        public void UseItemSlot(int slotNumber)
        {
            var itemName = _inventory.battleInventory[slotNumber].itemName;
            int usesLeft = _inventory.battleInventory[slotNumber].itemUses;
           
            if (usesLeft > 0 && itemName != "")
            {
                _inventory.UseItem(slotNumber);
                BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("item", usesLeft, slotNumber);
                BattleUIManager.Instance.ActivateButtons(false);
                _displayActionChannel.RaiseDisplayActionEvent($"{_characterName} used {_inventory.battleInventory[slotNumber].itemName}!");
                isTurnComplete = true;
                turnCompleteChannel.RaiseTurnCompleteEvent(thisCharacterType, isTurnComplete);
            }
            else if (usesLeft <= 0 && itemName == "")
                return;
        }
    }
}