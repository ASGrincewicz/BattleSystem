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
        [Header("Listening To")]
        [SerializeField] private UnitNameUpdate _unitNameUpdateChannel;
        [SerializeField] private UnitHitPointUpdate _unitHPUpdateChannel;

        private void OnEnable()
        {
            _unitNameUpdateChannel.OnUnitNameUpdated.AddListener(DisplayUnitName);
            _unitHPUpdateChannel.OnUnitHPUpdated.AddListener(DisplayCurrentUnitHP);
        }
        private void OnDisable()
        {
            _unitNameUpdateChannel.OnUnitNameUpdated.RemoveListener(DisplayUnitName);
            _unitHPUpdateChannel.OnUnitHPUpdated.RemoveListener(DisplayCurrentUnitHP);
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
    }
}
