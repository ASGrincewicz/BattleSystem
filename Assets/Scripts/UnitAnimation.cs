using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class UnitAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private List<AnimationClip> _animationClips = new List<AnimationClip>();

        private void Start() => _animator = GetComponent<Animator>();

        public void SetInteger(string parameter, int integer)
        {
            _animator.SetInteger(parameter, integer);
            StartCoroutine(ResetInteger(parameter));
        }

        public void PlayClip(string statename) => _animator.Play(statename);

        private IEnumerator ResetInteger(string parameter)
        {
            yield return new WaitForSeconds(4);
            _animator.SetInteger(parameter, 0);
            //PlayClip("Idle");
        }
    }
}
