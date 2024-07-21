using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set; }
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayClip(AudioClip audioClip, float volume)
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        
        audioSource.Play();
    }

    public void PlayClipInWorld(AudioClip audioClip, Transform spawnPoint, float volume)
    {
        AudioSource audioSourceInstance = Instantiate(soundFXObject, spawnPoint.position, Quaternion.identity);
        audioSourceInstance.clip = audioClip;
        audioSourceInstance.volume = volume;

        audioSourceInstance.Play();

        float duration = audioSourceInstance.clip.length + 0.05f;
        Destroy(audioSourceInstance.gameObject, duration);
    }
}
