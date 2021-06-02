using UnityEngine;

namespace Veganimus.BattleSystem
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///@info: This Class is assigned to a Unit to allow it to get the IDamageable
    ///interface from the Unit within the collider.
    ///It can be used for other reasons such as healing an ally through the IHealable
    ///interface, etc..
    ///</summary>
    public class TargetFinder : MonoBehaviour
    {
        [SerializeField] private Transform _targetUnitPosition;
        [SerializeField] private Unit _targetUnit;
        public IDamageable targetIDamageable;
        public IBuffable targetIBuffable;
        public IHealable targetIHealable;
        private UnitInfo _targetStats;
        public UnitInfo TargetStats { get { return _targetStats; } set{ } }

        private void OnTriggerEnter(Collider other)
        {
            _targetUnit = other.gameObject.GetComponentInParent<Unit>();
            if (_targetUnit != null)
            {
                targetIDamageable = _targetUnit.GetComponent<IDamageable>();
                targetIBuffable = _targetUnit.GetComponent<IBuffable>();
                targetIHealable = _targetUnit.GetComponent<IHealable>();
                _targetStats = _targetUnit.RunTimeUnitInfo;
                Debug.Log($"Found Target Info: Defense:{_targetStats.defense},Speed:{_targetStats.speed}");
                
            }

        }
    }
}
