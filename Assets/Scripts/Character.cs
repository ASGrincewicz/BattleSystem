using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Veganimus.BattleSystem
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///@info: Represents either the User as "Player" or an AI.
    ///</summary>
    public class Character : MonoBehaviour
    {
        public CharacterStats ThisCharacterStats;
        [SerializeField] private CharacterType _thisCharacterType;
        public CharacterType ThisCharacterType { get { return _thisCharacterType; } }

        [SerializeField] private string _characterName;
        public string CharacterName { get { return _characterName; } }

        [Range(-1, 1)][SerializeField] private int _aiAgression;
        public int AIAgression { get { return _aiAgression; } }

        [SerializeField] private List<UnitStats> _party = new List<UnitStats>();
        public List<UnitStats> Party { get { return _party; } }
        public List<MoveEffect> effects = new List<MoveEffect>();
        private BattleInventory _inventory;
        public BattleInventory ThisInventory { get { return _inventory; } }
        public Unit activeUnit;
       [SerializeField] private int _activeUnitSlotNumber;
        public GameObject activeUnitPrefab;
        public bool isDefeated;
        [SerializeField] private Transform activeUnitSpot;
        [Header("Broadcasting on")]
        [SerializeField] private TurnCompleteChannel _turnCompleteChannel;
        public TurnCompleteChannel TurnCompleteChannel { get { return _turnCompleteChannel; } }

        [SerializeField] private DisplayActionChannel _displayActionChannel;
        [SerializeField] private UnitMoveNameUpdate _itemNameUpdateChannel;
        [SerializeField] private SwapUnitChannel _swapUnitChannel;
        [Space]
        [Header("Listening to:")]
        [SerializeField] private CharacterTurnChannel characterTurnChannel;
        [Space]
        [SerializeField] private int _turnCount;
        public int TurnCount { get { return _turnCount; } }
        [SerializeField] private bool _isTurnComplete;
        public bool IsTurnComplete { get => _isTurnComplete; set => _isTurnComplete = value; }

        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        //private object _statUpdateDelay;

        private void OnEnable() => characterTurnChannel.OnCharacterTurn.AddListener(InitiateCharacterTurn);

        private void OnDisable() => characterTurnChannel.OnCharacterTurn.RemoveListener(InitiateCharacterTurn);

        private void Start()
        {
            _characterName = ThisCharacterStats.CharacterName;
            _inventory = GetComponent<BattleInventory>();
            activeUnitPrefab = Instantiate(_party[0].UnitModelPrefab ,activeUnitSpot);
            
            activeUnit.unitStats = _party[0];
            _activeUnitSlotNumber = _party.IndexOf(activeUnit.unitStats);
            Debug.Log($"Active Unit Slot:{_activeUnitSlotNumber}");
            activeUnitPrefab.transform.position = new Vector3(activeUnit.transform.position.x, 15, activeUnit.transform.position.z);
            activeUnitPrefab.transform.rotation = activeUnitSpot.rotation;
            
            UpdateCharacterNames();
        }

        private void InitiateCharacterTurn(CharacterType characterType)
        {
            if (characterType == ThisCharacterType)
            {
                _turnCount++;
                IsTurnComplete = false;
                TurnCompleteChannel.RaiseTurnCompleteEvent(characterType, IsTurnComplete);
                DeActivateEffects();
                if (ThisCharacterType != CharacterType.Player && IsTurnComplete == false)
                {
                    StartCoroutine(activeUnit.TurnDelayRoutine());
                }
            }
        }
        private void DeActivateEffects()
        {
            var efffect = GetComponentsInChildren<MoveEffect>();
            foreach(var obj in activeUnit.GetComponentsInChildren<MoveEffect>())
            {
                effects.Add(obj);
            }
            for (int i = effects.Count - 1; i > 0; i--)
            {
                if (TurnCount - effects[i].activatedOnTurn > effects[i].turnsActive)
                {
                    effects[i].gameObject.SetActive(false);
                    activeUnit.ResetDefense();
                    effects.Remove(effects[i]); 
                }
            }
        }
        private void UpdateCharacterNames()
        {
            BattleUIManager.Instance.UpdateCharacterNames(ThisCharacterType, CharacterName);
        }
        public void UpdateItemNames()
        {
            UpdateItemUseUI();
            for (int i = _inventory.battleInventory.Count - 1; i >= 0; i--)
            {
                var item = _inventory.battleInventory[i];
                var type = _inventory.battleInventory[i].ItemType;
                _itemNameUpdateChannel.RaiseMoveNameUpdateEvent(item.ItemName, i);
                switch (type)
                {
                    case ItemType.Health:
                        BattleUIManager.Instance.DisplayItemEffects(type, item.StatAffected, item.EffectAmount, i);
                        break;
                    case ItemType.Equipment:
                        BattleUIManager.Instance.DisplayItemEffects(type, item.StatAffected, item.EffectAmount, i);
                        break;
                }
            }
        }
        private void UpdateItemUseUI()
        {
           
            for (int i = _inventory.battleInventory.Count - 1; i >= 0; i--)
            {
                var item = _inventory.battleInventory[i];
                uint usesLeft = _inventory.battleInventory[i].ItemUses;
                if (usesLeft >= 0)
                    BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("item", usesLeft, i);
                else
                    return;
            }
        }
        public void UseItemSlot(int slotNumber)
        {
            var itemName = _inventory.battleInventory[slotNumber].ItemName;
            uint usesLeft = _inventory.battleInventory[slotNumber].ItemUses;

            if (usesLeft > 0 && itemName != "")
            {
                _inventory.UseItem(slotNumber);
                BattleUIManager.Instance.DisplayCurrentMoveUsesLeft("item", usesLeft, slotNumber);
                BattleUIManager.Instance.ActivateButtons(false);
                _displayActionChannel.RaiseDisplayActionEvent($"{_characterName} used {_inventory.battleInventory[slotNumber].ItemName}!");
                IsTurnComplete = true;
                TurnCompleteChannel.RaiseTurnCompleteEvent(ThisCharacterType, IsTurnComplete);
            }
            else if (usesLeft <= 0|| itemName == "")
            {
                if (ThisCharacterType != CharacterType.Player)
                    activeUnit.DetermineAction();
                else
                    return;
            }
        }
        public void SwapUnit(int slotNumber)
        {
            if (slotNumber != _activeUnitSlotNumber)
            {
                var unitName = _party[slotNumber].UnitName;
                Destroy(activeUnitPrefab);
                activeUnitPrefab = Instantiate(_party[slotNumber].UnitModelPrefab, activeUnitSpot);
                activeUnit.unitStats = _party[slotNumber];
                _activeUnitSlotNumber = _party.IndexOf(activeUnit.unitStats);
                _swapUnitChannel.RaiseUnitSwapEvent();
                activeUnitPrefab.transform.position = new Vector3(activeUnit.transform.position.x, 15, activeUnit.transform.position.z);
                activeUnitPrefab.transform.rotation = activeUnitSpot.rotation;
                BattleUIManager.Instance.ActivateButtons(false);
                _displayActionChannel.RaiseDisplayActionEvent($"{_characterName} swapped in {unitName}!");
                IsTurnComplete = true;
                TurnCompleteChannel.RaiseTurnCompleteEvent(ThisCharacterType, IsTurnComplete);
            }
            else
            {
                if (ThisCharacterType != CharacterType.Player)
                    activeUnit.DetermineAction();
                else
                    return;
            }
        }
    }
}