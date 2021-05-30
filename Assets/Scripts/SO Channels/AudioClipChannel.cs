using UnityEngine;
using UnityEngine.Events;
using System;
namespace Veganimus
{
    [CreateAssetMenu(menuName = "Audio Clip Channel")]
    public class AudioClipChannel : ScriptableObject
    {
        public UnityEvent<AudioClip> OnPlayClip;

        public void RaisePlayClipEvent(AudioClip clip)
        {
            if (OnPlayClip != null)
                OnPlayClip.Invoke(clip);
        }
    }
}