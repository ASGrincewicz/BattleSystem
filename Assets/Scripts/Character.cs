using System.Collections.Generic;
using UnityEngine;

namespace Veganimus.BattleSystem
{
    public enum CharacterType { Player, Enemy, Ally, EnemyAlly }
    public class Character : MonoBehaviour
    {
        public CharacterType characterType;
        [SerializeField] private string _playerName;
        //Inventory will be assigned once created
        [SerializeField] private List<Unit> _party = new List<Unit>();
        public Unit activeUnit;
        [SerializeField] private int _turnCount;
    }
}