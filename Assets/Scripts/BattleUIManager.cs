using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Veganimus.BattleSystem
{
    public class BattleUIManager : MonoBehaviour
    {
        [SerializeField] private Canvas _battleCanvas;
        [SerializeField] private TMP_Text _playerUnitNameText;
        [SerializeField] private TMP_Text _enemyUnitNameText;
        [SerializeField] private Slider _playerHitPointsSlider;
        [SerializeField] private Slider _enemyHitPointsSlider;
        [Header("Player Attack Move Buttons")]
        [SerializeField] private TMP_Text[] _playerAttackNames = new TMP_Text[0];
        [SerializeField] private Button[] _playerAttackButtons = new Button[4];
        [Header("Player Defense Move Buttons")]
        [SerializeField] private TMP_Text[] _playerDefenseNames = new TMP_Text[0];
        [SerializeField] private Button[] _playerDefenseButtons = new Button[4];
        [Header("Listening To")]
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] private UnitHitPointUpdate _unitHPUpdateChannel;
        [SerializeField] private UnitMoveNameUpdate _unitMoveNameUpdateChannel;

        private void OnEnable()
        {
            _unitNameUpdateChannel.OnUnitNameUpdated.AddListener(DisplayUnitName);
            _unitHPUpdateChannel.OnUnitHPUpdated.AddListener(DisplayCurrentUnitHP);
            _unitMoveNameUpdateChannel.OnMoveNameUpdated.AddListener(DisplayCurrentMoveNames);
        }
        private void OnDisable()
        {
            _unitNameUpdateChannel.OnUnitNameUpdated.RemoveListener(DisplayUnitName);
            _unitHPUpdateChannel.OnUnitHPUpdated.RemoveListener(DisplayCurrentUnitHP);
            _unitMoveNameUpdateChannel.OnMoveNameUpdated.RemoveListener(DisplayCurrentMoveNames);
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
        //Need To Separate Attack and Defense Name Updates
        private void DisplayCurrentMoveNames(string moveName, int moveSlot)
        {
            for(int i = _playerAttackNames.Length; i>=0; i--)
            {
                _playerAttackNames[moveSlot].text = $"{moveName}";
                foreach(Button button in _playerAttackButtons)
                {
                    if (moveName == null)
                        button.gameObject.SetActive(false);
                }
            }
            for (int i = _playerDefenseNames.Length; i >= 0; i--)
            {
                _playerDefenseNames[moveSlot].text = $"{moveName}";
                foreach (Button button in _playerDefenseButtons)
                {
                    if (moveName == null)
                        button.gameObject.SetActive(false);
                }
            }
        }
    }
}
