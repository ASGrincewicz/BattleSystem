using UnityEngine;

namespace Veganimus.BattleSystem
{
    [System.Serializable]
    public struct MoveInfo
    {
        public string moveName;
        [Range(1, 10)] public int uses;
        public int accuracy;
        public MoveType moveType;
        public ElementType elementType;

        public MoveInfo(string moveName, int uses, int accuracy, MoveType moveType, ElementType elementType)
        {
            this.moveName = moveName;
            this.uses = uses;
            this.accuracy = accuracy;
            this.moveType = moveType;
            this.elementType = elementType;
        }
        public MoveInfo(string moveName, int uses)
        {
            this.moveName = moveName;
            this.uses = uses;
            this.accuracy = default;
            this.moveType = default;
            this.elementType = default;
        }



    }
}
