using System.Collections.Generic;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class UnitAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private List<AnimationClip> _animationClips = new List<AnimationClip>();

        private void Start() => _animator = GetComponent<Animator>();

        public void SetInteger(string parameter, int integer) => _animator.SetInteger(parameter, integer);

        public void PlayClip(string statename) => _animator.Play("Death");
    }
}
