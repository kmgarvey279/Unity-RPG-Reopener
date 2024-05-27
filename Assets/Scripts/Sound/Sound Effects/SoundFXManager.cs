using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set; }
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (Instance = null)
        {
            Instance = this;
        }
    }

    public void PlayClip(AudioClip audioClip, Transform spawnPoint, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnPoint.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        
        audioSource.Play();
        
        float duration = audioSource.clip.length;
        Destroy(audioSource, duration);
    }
}
