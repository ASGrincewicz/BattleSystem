using UnityEngine;

namespace Veganimus
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip _sceneMusic;
        [SerializeField] private AudioClipChannel _audioClipChannel;
        [SerializeField] private AudioSource _sFXAudioSource;
        [SerializeField] private AudioSource _sceneMusicAudioSource;

        private void OnEnable() => _audioClipChannel.OnPlayClip.AddListener(PlaySFX);

        private void Start()
        {
            _sFXAudioSource = GetComponent<AudioSource>();
            _sceneMusicAudioSource = GetComponent<AudioSource>();
            if (_sceneMusic != null)
            {
                _sceneMusicAudioSource.volume = 0.5f;
                _sceneMusicAudioSource.clip = _sceneMusic;
                _sceneMusicAudioSource.Play();
            }
        }

        public void PlaySFX(AudioClip clip) => _sFXAudioSource.PlayOneShot(clip);
    }
}
