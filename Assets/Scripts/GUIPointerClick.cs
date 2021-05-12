using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Veganimus.BattleSystem
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    public class GUIPointerClick : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent onLeft;
        public UnityEvent onRight;
        public GameObject gridOne;
        public GameObject gridTwo;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                onLeft.Invoke();

            else if (eventData.button == PointerEventData.InputButton.Right)
                onRight.Invoke();
        }
        public void RightClick()
        {
            gridOne.SetActive(false);
            gridTwo.SetActive(true);
            StartCoroutine(GridReset());
        }
        private IEnumerator GridReset()
        {
            yield return new WaitForSeconds(2f);
            gridOne.SetActive(true);
            gridTwo.SetActive(false);
        }
    }
}

