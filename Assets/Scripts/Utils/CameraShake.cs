// Aaron Grincewicz ASGrincewicz@icloud.com 6/2/2021
using System.Collections;
using UnityEngine;

namespace Veganimus
{
    public class CameraShake : MonoBehaviour
    {
       [SerializeField] private bool _isShaking;
       [SerializeField] private float _duration;
       [SerializeField] private float _magnitude;
       [SerializeField] private CameraShakeChannel _cameraShakeChannel;

        private void OnEnable() => _cameraShakeChannel.OnCameraShake.AddListener(StartCameraShake);
        private void OnDisable() => _cameraShakeChannel.OnCameraShake.RemoveListener(StartCameraShake);
        private void StartCameraShake() => StartCoroutine(CameraShakeRoutine());

        private IEnumerator CameraShakeRoutine()
        {
            var defaultPosition = transform.position;
            var elapsedTime = 0f;

            while(elapsedTime < _duration)
            {
                _isShaking = true;
                float x = Random.Range(-1, 1) * _magnitude;
                float y = Random.Range(-1, 1) * _magnitude;
                transform.position = new Vector3(x, y, -10f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = defaultPosition;
            _isShaking = false;
        }
    }
}
