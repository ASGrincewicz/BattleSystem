using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
   
    private void Start() => _audioSource = GetComponent<AudioSource>();

    public void PlaySFX(AudioClip clip) => _audioSource.PlayOneShot(clip);

}
