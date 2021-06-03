// Aaron Grincewicz Veganimus@icloud.com 6/2/2021
using UnityEngine;

namespace Veganimus
{
    public class Rotate : MonoBehaviour
    {
        public Rotate() { }

        public Rotate(Vector3 vector3) => _rotationVector = vector3;

        [SerializeField] private Vector3 _rotationVector;

        private void Update() => RotateThis(_rotationVector);

        public void RotateThis(Vector3 rotationVector) => transform.Rotate(rotationVector * Time.deltaTime);
    }
}
