using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Veganimus.BattleSystem
{
    public class DefenseBar : MonoBehaviour
    {
        [SerializeField] private List<Image> _defensePointImage = new List<Image>(5);
        [SerializeField] private Unit _activeUnit;
        [SerializeField] private DefenseUIChannel _defenseUIChannel;

        public void OnEnable()
        {
            _defenseUIChannel.OnDefenseChange.AddListener(ShowDefense);
        }
        public void OnDisable()
        {
            _defenseUIChannel.OnDefenseChange.RemoveListener(ShowDefense);
        }
        public void ClearDefense()
        {
            foreach(var image in _defensePointImage)
                image.gameObject.SetActive(false);
        }
        public void ShowDefense(int defense)
        {
            ClearDefense();
            for(int i = 0; i < defense; i++)
                 _defensePointImage[i].gameObject.SetActive(true);
        }
    }
}
