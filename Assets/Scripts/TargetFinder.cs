using UnityEngine;

namespace Veganimus.BattleSystem
{
    public class TargetFinder : MonoBehaviour
    {
        [SerializeField] protected Transform _targetUnitPosition;
        [SerializeField] protected Unit _targetUnit;
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
