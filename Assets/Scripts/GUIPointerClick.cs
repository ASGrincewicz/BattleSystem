using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        public GameObject textGridOne;
        public GameObject textGridTwo;
        public string baseText = "Button";

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                onLeft.Invoke();

            else if (eventData.button == PointerEventData.InputButton.Right)
                onRight.Invoke();
        }
        public void RightClick()
        {
            textGridOne.SetActive(false);
            textGridTwo.SetActive(true);
            StartCoroutine(TextReset());
        }
        private IEnumerator TextReset()
        {
            yield return new WaitForSeconds(2f);
            textGridOne.SetActive(true);
            textGridTwo.SetActive(false);
        }
    }
}

