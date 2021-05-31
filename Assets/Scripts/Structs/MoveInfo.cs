using UnityEngine;

namespace Veganimus.BattleSystem
{
    [System.Serializable]
    public struct MoveInfo
    {
        public string moveName;
        [Range(1, 10)] public uint uses;
        public int accuracy;
        public MoveType moveType;
        public ElementType elementType;
        public AudioClip moveSFX;

        public MoveInfo(string moveName, uint uses, int accuracy, MoveType moveType, ElementType elementType, AudioClip clip)
        {
            this.moveName = moveName;
            this.uses = uses;
            this.accuracy = accuracy;
            this.moveType = moveType;
            this.elementType = elementType;
            this.moveSFX = clip;
        }
        public MoveInfo(string moveName, uint uses)
        {
            this.moveName = moveName;
            this.uses = uses;
            this.accuracy = default;
            this.moveType = default;
            this.elementType = default;
            this.moveSFX = default;
        }
    }
}
