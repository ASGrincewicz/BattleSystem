using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Veganimus.BattleSystem
{
    public class BattleUIManager : MonoBehaviour
    {
        public static BattleUIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("Boss is NULL!");
                }
                return _instance;
            }
        }
        private static BattleUIManager _instance;
        [SerializeField] private Canvas _battleCanvas;
        [Header("Player UI")]
        [SerializeField] private Image _playerTurnIndicator;
        [SerializeField] private TMP_Text _playerUnitNameText;
        [SerializeField] private Slider _playerHitPointsSlider;
        [Header("Enemy UI")]
        [SerializeField] private Image _enemyTurnIndicator;
        [SerializeField] private TMP_Text _enemyUnitNameText;
        [SerializeField] private Slider _enemyHitPointsSlider;
        [Space]
        [SerializeField] private TMP_Text _actionText;
        [SerializeField] private TMP_Text _statUpdateText;
        private WaitForSeconds _displayTextDelay;

        [Header("Player Attack Move Buttons")]
        [SerializeField] private TMP_Text[] _playerAttackNames = new TMP_Text[0];
        [SerializeField] private Button[] _playerAttackButtons = new Button[4];

        [Header("Player Defense Move Buttons")]
        [SerializeField] private TMP_Text[] _playerDefenseNames = new TMP_Text[0];
        [SerializeField] private Button[] _playerDefenseButtons = new Button[4];

        [Header("Listening To")]
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] private UnitHitPointUpdate _unitHPUpdateChannel;
        [SerializeField] private UnitMoveNameUpdate _unitAttackMoveNameUpdateChannel;
        [SerializeField] private UnitMoveNameUpdate _UnitDefenseMoveNameUpdateChannel;
        [SerializeField] private DisplayActionChannel _displayActionTakenChannel;

        private void Awake() => _instance = this;

        private void OnEnable()
        {
            _unitNameUpdateChannel.OnUnitNameUpdated.AddListener(DisplayUnitName);
            _unitHPUpdateChannel.OnUnitHPUpdated.AddListener(DisplayCurrentUnitHP);
            _unitAttackMoveNameUpdateChannel.OnMoveNameUpdated.AddListener(DisplayCurrentAttackMoveNames);
            _UnitDefenseMoveNameUpdateChannel.OnMoveNameUpdated.AddListener(DisplayCurrentDefenseMoveNames);
            _displayActionTakenChannel.OnDisplayAction.AddListener(DisplayCurrentActionTaken);
        }
        private void OnDisable()
        {
            _unitNameUpdateChannel.OnUnitNameUpdated.RemoveListener(DisplayUnitName);
            _unitHPUpdateChannel.OnUnitHPUpdated.RemoveListener(DisplayCurrentUnitHP);
            _unitAttackMoveNameUpdateChannel.OnMoveNameUpdated.RemoveListener(DisplayCurrentAttackMoveNames);
            _UnitDefenseMoveNameUpdateChannel.OnMoveNameUpdated.RemoveListener(DisplayCurrentDefenseMoveNames);
            _displayActionTakenChannel.OnDisplayAction.RemoveListener(DisplayCurrentActionTaken);
        }
        private void Start() => _displayTextDelay = new WaitForSeconds(2f);

        public void ActivateButtons(bool isPlayerTurn)
        {
            if (isPlayerTurn == false)
            {
                foreach (var button in _playerAttackButtons)
                {
                    button.interactable = false;
                }
                foreach (var button in _playerDefenseButtons)
                {
                    button.interactable = false;
                }
            }
            else if (isPlayerTurn)
            {
                foreach (var button in _playerAttackButtons)
                {
                    button.interactable = true;
                }
                foreach (var button in _playerDefenseButtons)
                {
                    button.interactable = true;
                }
            }
        }

        private void DisplayUnitName(string unit, string unitName)
        {
            switch(unit)
            {
                case "Player":
                    _playerUnitNameText.text = unitName;
                    break;
                case "Enemy":
                    _enemyUnitNameText.text = unitName;
                    break;
            }
        }
        private void DisplayCurrentUnitHP(string unit,int maxUnitHP, int unitHP)
        {
            float sliderValue = (float)unitHP / maxUnitHP * 100;
            switch (unit)
            {
                case "Player":
                    _playerHitPointsSlider.value = sliderValue;
                    break;
                case "Enemy":
                    _enemyHitPointsSlider.value = sliderValue;
                    break;
            }
        }
       
        private void DisplayCurrentAttackMoveNames(string moveName, int moveSlot)
        {
            for (int i = _playerAttackNames.Length; i >= 0; i--)
            {
                _playerAttackNames[moveSlot].text = $"{moveName}";
                foreach (Button button in _playerAttackButtons)
                {
                    if (moveName == null)
                        button.gameObject.SetActive(false);
                }
            }
        }
        private void DisplayCurrentDefenseMoveNames(string moveName, int moveSlot)
        { 
            for (int i = _playerDefenseNames.Length; i >= 0; i--)
            {
                _playerDefenseNames[moveSlot].text = $"{moveName}";
            }
            foreach (Button button in _playerDefenseButtons)
            {
                if(_playerDefenseNames[moveSlot]== null)
                    button.interactable = false;
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
