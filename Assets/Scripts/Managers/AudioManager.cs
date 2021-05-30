using UnityEngine;

namespace Veganimus
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClipChannel _audioClipChannel;
        private AudioSource _audioSource;

        private void OnEnable()
        {
            _audioClipChannel.OnPlayClip.AddListener(PlaySFX);
        }
        private void Start() => _audioSource = GetComponent<AudioSource>();

        public void PlaySFX(AudioClip clip)
        {
            if (_audioSource.isPlaying == false)
                _audioSource.PlayOneShot(clip);
            else
                return;
        }
    }
}
