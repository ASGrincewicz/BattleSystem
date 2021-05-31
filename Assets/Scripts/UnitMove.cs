using UnityEngine;

namespace Veganimus.BattleSystem
{
    public enum MoveType
    {
        Physical, Special, Buff, DeBuff
    }
    [System.Serializable]
    public abstract class UnitMove : ScriptableObject
    {
        [SerializeField] private MoveInfo _moveInfo = new MoveInfo();
        public string MoveName { get { return _moveInfo.moveName; } }
        public uint MoveUses { get { return _moveInfo.uses; } set { } }
        public float MoveAccuracy { get { return _moveInfo.accuracy; } }
        public MoveType MoveType { get { return _moveInfo.moveType; } }
        public ElementType MoveElementType { get { return _moveInfo.elementType; } }
        public AudioClip MoveSoundEffect { get { return _moveInfo.moveSFX; } }
        public string animationTrigger;
        public uint runtimeUses;
        [SerializeField] protected Transform _assignedUnit;
        [SerializeField] private DisplayActionChannel _displayActionChannel;
        [SerializeField] private AudioClipChannel _audioClipChannel;
        //private DieRoll dieRoll;

        private void OnEnable() => runtimeUses = MoveUses;


        public bool RollForMoveAccuracy(int accuracyModifier)
        {
            var dieRoll = new DieRoll();
            var result =  dieRoll.Roll(MoveAccuracy, accuracyModifier);
            Debug.Log($"{MoveName}:{dieRoll.GetResult()}");
            return result;
        }
        protected void PlayMoveSoundEffect(AudioClip clip) => _audioClipChannel.RaisePlayClipEvent(clip);
    }
}
