// Aaron Grincewicz Veganimus@icloud.com 6/2/2021
using UnityEngine;

namespace Veganimus
{
    public class Orbit : MonoBehaviour
    {
        public Orbit() { }
        public Orbit(float speed, Vector3 vector, Transform pivot)
        {
            orbitSpeed = speed;
            orbitVector = vector;
            pivotTransform = pivot;
        }
        [SerializeField] private float orbitSpeed;
        [SerializeField] private Vector3 orbitVector;
        [SerializeField] private Transform pivotTransform;

        private void Update() => OrbitObject();

        public void OrbitObject() => transform.RotateAround(pivotTransform.position,
                                                            orbitVector,
                                                            orbitSpeed);
    }
}
