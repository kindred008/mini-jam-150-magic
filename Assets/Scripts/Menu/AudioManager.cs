using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void UpdateVolume(float newVolume)
    {
        _audioSource.volume = newVolume;
    }

    public float GetVolume()
    {
        return _audioSource.volume;
    }

    public void PlaySound(AudioClip audioClip)
    {

    }
}
