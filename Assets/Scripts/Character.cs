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
        [SerializeField] private CharacterType _thisCharacterType;
        public CharacterType ThisCharacterType { get { return _thisCharacterType; } }

        [SerializeField] private string _characterName;
        public string CharacterName { get { return _characterName; } }

        [Range(-1, 1)][SerializeField] private int _aiAgression;
        public int AIAgression { get { return _aiAgression; } }

        [SerializeField] private List<UnitStats> _party = new List<UnitStats>();
        public List<UnitStats> Party { get { return _party; } }

        private Inventory _inventory;
        public Unit activeUnit;
        public int turnCount;
        public bool isDefeated;
        [SerializeField] private Transform activeUnitSpot;
        [Header("Broadcasting on")]
        [SerializeField] private TurnCompleteChannel _turnCompleteChannel;
        public TurnCompleteChannel TurnCompleteChannel { get { return _turnCompleteChannel; } }

        [SerializeField] private DisplayActionChannel _displayActionChannel;
        [SerializeField] private UnitMoveNameUpdate _itemNameUpdateChannel;
        [Space]
        [Header("Listening to:")]
        [SerializeField] private CharacterTurnChannel characterTurnChannel;
        [Space]
        [SerializeField] private bool _isTurnComplete;
        public bool IsTurnComplete { get => _isTurnComplete; set => _isTurnComplete = value; }

        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;

        private void OnEnable() => characterTurnChannel.OnCharacterTurn.AddListener(InitiateCharacterTurn);

        private void OnDisable() => characterTurnChannel.OnCharacterTurn.RemoveListener(InitiateCharacterTurn);

        private void Start()
        {
            _inventory = GetComponent<Inventory>();
            GameObject activeUnitPrefab = Instantiate(_party[0].unitModelPrefab ,activeUnitSpot);
            activeUnit.unitStats = _party[0];
            activeUnitPrefab.transform.position = activeUnitSpot.position;
            activeUnitPrefab.transform.rotation = activeUnitSpot.rotation;
        }

        private void InitiateCharacterTurn(CharacterType characterType)
        {
            if (characterType == ThisCharacterType)
            {
                IsTurnComplete = false;
                TurnCompleteChannel.RaiseTurnCompleteEvent(characterType, IsTurnComplete);
                if (ThisCharacterType != CharacterType.Player && IsTurnComplete == false)
                {
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
                if(item.ThisItemType == Item.ItemType.Health)
                    BattleUIManager.Instance.DisplayMoveStats("health item", 0, 0, item.healAmount, i);
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
                IsTurnComplete = true;
                TurnCompleteChannel.RaiseTurnCompleteEvent(ThisCharacterType, IsTurnComplete);
            }
            else if (usesLeft <= 0 && itemName == "")
                return;
        }
    }
}