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

        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                _targetUnit = other.gameObject.GetComponent<Unit>();
                if (_targetUnit != null)
                    targetIDamageable = _targetUnit.gameObject.GetComponent<IDamageable>();
            }
        }
    }
}
