using System.Collections.Generic;
using UnityEngine;
namespace Veganimus.BattleSystem
{
    [CreateAssetMenu(menuName = "Move Set")]
    public class MoveSet : ScriptableObject
    {
        [SerializeField] private UnitMove[] _moves = new UnitMove[4];
    }
}
