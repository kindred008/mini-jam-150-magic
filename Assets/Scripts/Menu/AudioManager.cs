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

    private void Update()
    {
        _audioSource.volume = GlobalData.MusicVolume;
    }

    public float GetVolume()
    {
        return _audioSource.volume;
    }

    public void PauseAudio()
    {
        //_audioSource.Pause();
        _audioSource.Stop();
    }

    public void ResumeAudio()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void PlaySound(AudioClip audioClip)
    {

    }
}
