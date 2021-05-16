using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Veganimus.BattleSystem
{
    public class BattleUIManager : Singleton<BattleUIManager>
    {
        [SerializeField] private Canvas _battleCanvas;
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
        [Space]
        [Header("Unit Stats")]
        [SerializeField] private TMP_Text _unitHPText;
        [SerializeField] private TMP_Text _unitSpeed;
        [SerializeField] private TMP_Text _unitDefense;
        [SerializeField] private TMP_Text _unitAccuracy;
        [Header("Player Attack Move Buttons")]
        [SerializeField] private TMP_Text[] _playerAttackNames = new TMP_Text[0];
        [SerializeField] private TMP_Text[] _attackMoveUses = new TMP_Text[0];
        [SerializeField] private TMP_Text[] _attackMoveAccuracy = new TMP_Text[0];
        [SerializeField] private TMP_Text[] _attackMoveDamage = new TMP_Text[0];
        [SerializeField] private Button[] _playerAttackButtons = new Button[4];

        [Header("Player Defense Move Buttons")]
        [SerializeField] private TMP_Text[] _playerDefenseNames = new TMP_Text[0];
        [SerializeField] private TMP_Text[] _defenseMoveUses = new TMP_Text[0];
        [SerializeField] private TMP_Text[] _defenseMoveBuff = new TMP_Text[0];
        [SerializeField] private Button[] _playerDefenseButtons = new Button[4];

        [Header("Player Item Buttons")]
        [SerializeField] private TMP_Text[] _playerItemNames = new TMP_Text[4];
        [SerializeField] private TMP_Text[] _playerItemUses = new TMP_Text[4];
        [SerializeField] private TMP_Text[] _playerItemEffects = new TMP_Text[4];
        [SerializeField] private Button[] _playerItemButtons = new Button[4];

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
        private void Start() => _displayTextDelay = new WaitForSeconds(2f);

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
            if (isPlayerTurn == false)
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
       
        private void DisplayCurrentAttackMoveNames(string moveName, int moveSlot)
        {
            for (int a = _playerAttackNames.Length; a >= 0; a--)
            {
                _playerAttackNames[moveSlot].text = $"{moveName}";
                foreach (Button button in _playerAttackButtons)
                {
                    if (moveName == "")
                        button.gameObject.SetActive(false);
                }
            }
        }
        private void DisplayCurrentDefenseMoveNames(string moveName, int moveSlot)
        { 
            for (int d = _playerDefenseNames.Length; d >= 0; d--)
            {
                _playerDefenseNames[moveSlot].text = $"{moveName}";
            }
        }
        private void DisplayCurrentItemNames(string itemName, int itemSlot)
        {
            for(int i = _playerItemNames.Length; i>=0; i--)
            {
                _playerItemNames[itemSlot].text = $"{itemName}";
            }
        }
        public void DisplayUnitStats(int hp, int maxHP,int speed, int defense, int accuracyMod)
        {
            _unitHPText.text = $"Current HP: {hp}/ {maxHP}";
            _unitSpeed.text = $"Speed: {speed}";
            _unitDefense.text = $"Defense: {defense}";
            _unitAccuracy.text = $"Accuracy Mod: {accuracyMod}";
        }
        public void DisplayCurrentMoveUsesLeft(string moveType, int uses, int moveSlot)
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
        public void DisplayMoveStats(string moveType,int damage, float accuracy,int buff, int moveSlot)
        {
            switch (moveType)
            {
                case "attack":
                    _attackMoveAccuracy[moveSlot].text = $"Accuracy: {accuracy}";
                    _attackMoveDamage[moveSlot].text = $"Damage: {damage}";
                    break;
                case "defense":
                    _defenseMoveBuff[moveSlot].text = $"Defense Buff: {buff}";
                    break;
                case "health item":
                    _playerItemEffects[moveSlot].text = $"Heal Amount:{buff}";
                    break;
                case "equipment":
                    _playerItemEffects[moveSlot].text = $"Equip Effect:{buff}";
                    break;

            }
        }
        public void DisplayCurrentActionTaken(string actionTaken)
        {
            _actionText.text = $"{actionTaken}";
            StartCoroutine(DisplayTextDelayRoutine(_actionText));
        }
        public void DisplayStatUpdateText(string statUpdate)
        {
            _statUpdateText.text = $"{statUpdate}";
            StartCoroutine(DisplayTextDelayRoutine(_statUpdateText));
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
        private IEnumerator DisplayTextDelayRoutine(TMP_Text text)
        {
            yield return _displayTextDelay;
            text.text = $"";
        }
    }
}
