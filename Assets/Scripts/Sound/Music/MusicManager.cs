using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayClip(AudioClip audioClip, float volume = 1f, float pitch = 1f)
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;

        audioSource.Play();
    }

    public void ChangePitch(float pitch)
    {
        audioSource.pitch = pitch;
    }

    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
