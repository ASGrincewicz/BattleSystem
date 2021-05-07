using System;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class Unit : MonoBehaviour
    {
       [Flags]
       public enum Type
        {
            Fire = 2,
            Water = 4,
            Earth = 8,
            Air = 16,
            Spirit = 32
        }
        [SerializeField] protected Type _unitType;
        [SerializeField] protected string _unitName;
        [SerializeField] protected int _unitHitPoints;
        [SerializeField] protected int _currentUnitHP;
        [SerializeField] protected int _unitSpeed;
        [SerializeField] protected int _unitDefense;

        private void Awake() => _currentUnitHP = _unitHitPoints;
    }
}
