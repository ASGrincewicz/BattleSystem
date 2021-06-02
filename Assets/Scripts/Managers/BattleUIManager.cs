using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Veganimus.BattleSystem
{
    public class BattleUIManager : Singleton<BattleUIManager>
    {
        //public Button attackButtonPrefab;
        //public Transform attackButtonPanel;
        [SerializeField] private Canvas _battleCanvas;
        [SerializeField] private GameObject _startSequence;
        [Header("Player UI")]
        [SerializeField] private GameObject _playerUI;
        [SerializeField] private Image _playerTurnIndicator;
        [SerializeField] private TMP_Text _playerCharacterNameText;
        [SerializeField] private TMP_Text _playerUnitNameText;
        [SerializeField] private Slider _playerHitPointsSlider;
        [Header("Enemy UI")]
        [SerializeField] private GameObject _enemyUI;
        [SerializeField] private Image _enemyTurnIndicator;
        [SerializeField] private TMP_Text _enemyCharacterNameText;
        [SerializeField] private TMP_Text _enemyUnitNameText;
        [SerializeField] private Slider _enemyHitPointsSlider;
        [Space]
        [SerializeField] private TMP_Text _actionText;
        [SerializeField] private TMP_Text _statUpdateText;
        [SerializeField] private TMP_Text _endBattleText;
        private WaitForSeconds _displayTextDelay;
        private WaitForSeconds _endBattleDelay;
        [Space]
        [Header("Unit Stats")]
        [SerializeField] private TMP_Text _unitHPText;
        [SerializeField] private TMP_Text _unitSpeed;
        [SerializeField] private TMP_Text _unitDefense;
        [SerializeField] private TMP_Text _unitAccuracy;
        [Header("Player Attack Move Buttons")]
        [SerializeField] private List<TMP_Text> _playerAttackNames = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> _attackMoveUses = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> _attackMoveAccuracy = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> _attackMoveDamage = new List<TMP_Text>();
        [SerializeField] private List<Button> _playerAttackButtons = new List<Button>();

        [Header("Player Defense Move Buttons")]
        [SerializeField] private List<TMP_Text> _playerDefenseNames = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> _defenseMoveUses = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> _defenseMoveBuff = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> _defenseMoveActiveTurns = new List<TMP_Text>();
        [SerializeField] private List<Button> _playerDefenseButtons = new List<Button>();

        [Header("Player Item Buttons")]
        [SerializeField] private List<TMP_Text> _playerItemNames = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> _playerItemUses = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> _playerItemEffects = new List<TMP_Text>();
        [SerializeField] private List<Button> _playerItemButtons = new List<Button>();

        [Header("Player Unit Buttons")]
        [SerializeField] private List<Button> _playerUnitButtons = new List<Button>();
        [SerializeField] private List<TMP_Text> _playerUnitNames = new List<TMP_Text>();

        [Header("Listening To")]
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] private UnitHitPointUpdate _unitHPUpdateChannel;
        [SerializeField] private UnitMoveNameUpdate _unitAttackMoveNameUpdateChannel;
        [SerializeField] private UnitMoveNameUpdate _UnitDefenseMoveNameUpdateChannel;
        [SerializeField] private UnitMoveNameUpdate _itemNameUpdateChannel;
        [SerializeField] private DisplayActionChannel _displayActionTakenChannel;
        [SerializeField] private BattleStateChannel _endBattleChannel;

        protected override void Awake()
        {
            base.Awake();
            ActivateButtons(false);
            ToggleTurnIndicators(BattleState.Start);
        }

        private void OnEnable()
        {
            _unitNameUpdateChannel.OnUnitNameUpdated.AddListener(DisplayUnitName);
            _unitHPUpdateChannel.OnUnitHPUpdated.AddListener(DisplayCurrentUnitHP);
           _unitAttackMoveNameUpdateChannel.OnMoveNameUpdated.AddListener(DisplayCurrentAttackMoveNames);
            _UnitDefenseMoveNameUpdateChannel.OnMoveNameUpdated.AddListener(DisplayCurrentDefenseMoveNames);
            _itemNameUpdateChannel.OnMoveNameUpdated.AddListener(DisplayCurrentItemNames);
            _displayActionTakenChannel.OnDisplayAction.AddListener(DisplayCurrentActionTaken);
            _endBattleChannel.OnBattleStateChanged.AddListener(EndBattleUIActivate);
        }
        private void OnDisable()
        {
            _unitNameUpdateChannel.OnUnitNameUpdated.RemoveListener(DisplayUnitName);
            _unitHPUpdateChannel.OnUnitHPUpdated.RemoveListener(DisplayCurrentUnitHP);
           _unitAttackMoveNameUpdateChannel.OnMoveNameUpdated.RemoveListener(DisplayCurrentAttackMoveNames);
            _UnitDefenseMoveNameUpdateChannel.OnMoveNameUpdated.RemoveListener(DisplayCurrentDefenseMoveNames);
            _itemNameUpdateChannel.OnMoveNameUpdated.RemoveListener(DisplayCurrentItemNames);
            _displayActionTakenChannel.OnDisplayAction.RemoveListener(DisplayCurrentActionTaken);
            _endBattleChannel.OnBattleStateChanged.RemoveListener(EndBattleUIActivate);
        }
        private void Start()
        {
            _displayTextDelay = new WaitForSeconds(2f);
            _endBattleDelay = new WaitForSeconds(5f);
            _startSequence.SetActive(true);
        }

        public void UpdateCharacterNames(CharacterType characterType, string characterName)
        {
            switch(characterType)
            {
                case CharacterType.Player:
                    _playerCharacterNameText.text = $"{characterName}";
                    break;
                case CharacterType.Enemy:
                    _enemyCharacterNameText.text = $"{characterName}";
                    break;
            }
        }

        public void ActivateButtons(bool isPlayerTurn)
        {
            if (!isPlayerTurn)
            {
                foreach (var attackButton in _playerAttackButtons)
                {
                    attackButton.interactable = false;
                }
                foreach (var defenseButton in _playerDefenseButtons)
                {
                    defenseButton.interactable = false;
                }
                foreach(var itemButton in _playerItemButtons)
                {
                    itemButton.interactable = false;
                }
                foreach(var unitButton in _playerUnitButtons)
                {
                    unitButton.interactable = false;
                }
            }
            else if (isPlayerTurn)
            {
                foreach (var attackButton in _playerAttackButtons)
                {
                    attackButton.interactable = true;
                }
                foreach (var defenseButton in _playerDefenseButtons)
                {
                    defenseButton.interactable = true;
                }
                foreach (var itemButton in _playerItemButtons)
                {
                    itemButton.interactable = true;
                }
                foreach (var unitButton in _playerUnitButtons)
                {
                    unitButton.interactable = true;
                }
            }
        }
        
        private void EndBattleUIActivate(BattleState battleState)
        {
            _playerUI.SetActive(false);
            _enemyUI.SetActive(false);
            _actionText.gameObject.SetActive(false);
            _statUpdateText.gameObject.SetActive(false);
            _endBattleText.gameObject.SetActive(true);

            if (battleState == BattleState.Win)
              _endBattleText.text = $"YOU WIN!";

            else if (battleState == BattleState.Lose)
             _endBattleText.text = $"YOU LOSE!";
            StartCoroutine(DisplayTextDelayRoutine(_endBattleDelay,_endBattleText));
        }

        private void DisplayUnitName(CharacterType characterType, string unitName)
        {
            switch(characterType)
            {
                case CharacterType.Player:
                    _playerUnitNameText.text = unitName;
                    break;
                case CharacterType.Enemy:
                    _enemyUnitNameText.text = unitName;
                    break;
                case CharacterType.Ally:
                    //Set up when ally is implemented.
                    break;
                case CharacterType.EnemyAlly:
                    break;
            }
        }
        private void DisplayCurrentUnitHP(CharacterType characterType,int maxUnitHP, int unitHP)
        {
            float sliderValue = (float)unitHP / maxUnitHP * 100;
            switch (characterType)
            {
                case CharacterType.Player:
                    _playerHitPointsSlider.value = sliderValue;
                    break;
                case CharacterType.Enemy:
                    _enemyHitPointsSlider.value = sliderValue;
                    break;
                case CharacterType.Ally:
                    //Set up when ally is implemented.
                    break;
                case CharacterType.EnemyAlly:
                    break;
            }
        }
        //public void PopulateAttackButtons(List<UnitAttackMove> moves)
        // {
        //     foreach(var move in moves)
        //     {
        //         var attackButton = Instantiate(attackButtonPrefab, attackButtonPanel);
        //         var info = attackButton.GetComponent<AttackButton>().attackButtonInfo;
        //         info.FillText(move.MoveName, move.MoveUses, move.MoveAccuracy, move.damageAmount);

        //     }
        // }
        public void DisplayCurrentAttackMoveNames(string moveName, int moveSlot)
        {
            for (int a = _playerAttackNames.Count; a >= 0; a--)
            {
                if(moveName != null)
                _playerAttackNames[moveSlot].text = $"{moveName}";
                else
                    _playerAttackNames[moveSlot].text = string.Empty;
            }
        }
        private void DisplayCurrentDefenseMoveNames(string moveName, int moveSlot)
        { 
            for (int d = _playerDefenseNames.Count; d >= 0; d--)
            {
                if (moveName != null)
                    _playerDefenseNames[moveSlot].text = $"{moveName}";
                else
                    _playerDefenseNames[moveSlot].text = string.Empty;
            }
        }
        private void DisplayCurrentItemNames(string itemName, int itemSlot)
        {
            for(int i = _playerItemNames.Count; i>=0; i--)
            {
                if (itemName != null)
                    _playerItemNames[itemSlot].text = $"{itemName}";
                else
                    _playerItemNames[itemSlot].text = string.Empty;
            }
        }
        public void DisplayPartyUnitNames(string unitName, int unitSlot)
        {
            for(int i = _playerUnitNames.Count; i>=0; i--)
            {
                if (unitName != string.Empty)
                    _playerUnitNames[unitSlot].text = $"{unitName}";
                else
                    _playerUnitNames[unitSlot].text = string.Empty;
            }
        }

        public void DisplayUnitStats(int hp, int maxHP,int speed, int defense, int accuracyMod)
        {
            _unitHPText.text = $"Current HP: {hp}/ {maxHP}";
            _unitSpeed.text = $"Speed: {speed}";
            _unitDefense.text = $"Defense: {defense}";
            _unitAccuracy.text = $"Accuracy Mod: {accuracyMod}";
        }
        public void DisplayCurrentMoveUsesLeft(string moveType, uint uses, int moveSlot)
        {
            switch(moveType)
            {
                case "attack":
                    _attackMoveUses[moveSlot].text = $"Uses Left: {uses}";
                    break;
                case "defense":
                    _defenseMoveUses[moveSlot].text = $"Uses Left: {uses}";
                    break;
                case "item":
                    _playerItemUses[moveSlot].text = $"Uses Left: {uses}";
                    break;
            }
        }
        public void DisplayMoveStats(string moveType,int damage, float accuracy,int buff,int activeTurns, int moveSlot)
        {
            switch (moveType)
            {
                case "attack":
                    _attackMoveAccuracy[moveSlot].text = $"Accuracy: {accuracy}";
                    _attackMoveDamage[moveSlot].text = $"Damage: {damage}";
                    break;
                case "defense":
                    _defenseMoveBuff[moveSlot].text = $"Defense Buff: {buff}";
                    _defenseMoveActiveTurns[moveSlot].text = $"Active for: {activeTurns} Turns";
                    break;
            }
        }
        public void DisplayItemEffects(ItemType type,StatAffected stat, int amount,int itemSlot)
        {
            switch(type)
            {
                case ItemType.Health:
                    _playerItemEffects[itemSlot].text = $"Heal Amount: {amount}";
                    break;
                case ItemType.Equipment:
                case ItemType.Boost:
                    _playerItemEffects[itemSlot].text = $"{stat}: +{ amount}";
                    break;
                case ItemType.Refill:
                    _playerItemEffects[itemSlot].text = $"Move Uses +{amount}";
                    break;
            }
        }
        public void DisplayCurrentActionTaken(string actionTaken)
        {
            _actionText.text = $"{actionTaken}";
            StartCoroutine(DisplayTextDelayRoutine(_displayTextDelay,_actionText));
        }
        public void DisplayStatUpdateText(string statUpdate)
        {
            _statUpdateText.text = $"{statUpdate}";
            StartCoroutine(DisplayTextDelayRoutine(_displayTextDelay,_statUpdateText));
        }
        public void ToggleTurnIndicators(BattleState battleState)
        {
            switch(battleState)
            {
                case BattleState.PlayerTurn:
                    _playerTurnIndicator.gameObject.SetActive(true);
                    _enemyTurnIndicator.gameObject.SetActive(false);
                    break;
                case BattleState.EnemyTurn:
                    _playerTurnIndicator.gameObject.SetActive(false);
                    _enemyTurnIndicator.gameObject.SetActive(true);
                    break;
                default:
                    _playerTurnIndicator.gameObject.SetActive(false);
                    _enemyTurnIndicator.gameObject.SetActive(false);
                    break;

            }
        }
        private IEnumerator DisplayTextDelayRoutine(WaitForSeconds delay ,TMP_Text text)
        {
            yield return delay;
            if (delay == _endBattleDelay)
                GameManager.Instance.LoadMainMenu();
            else if (delay == _displayTextDelay)
                text.text = string.Empty;
        }
    }
}
