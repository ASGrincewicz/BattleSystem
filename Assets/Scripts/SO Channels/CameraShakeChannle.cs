// Aaron Grincewicz Veganimus@icloud.com 6/2/2021
using UnityEngine;
using UnityEngine.Events;

namespace Veganimus
{
    [CreateAssetMenu(menuName ="Camera Shake Channel")]
    public class CameraShakeChannel : ScriptableObject
    {
        public UnityEvent OnCameraShake;

        public void RaiseCameraShakeEvent()
        {
            if (OnCameraShake != null)
                OnCameraShake.Invoke();
        }
    }
}
