using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip defaultMusic;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  //to persist across different scenes
        }
        else
        {
            Destroy(gameObject); 
            return;
        }

        if (defaultMusic != null)
        {
            PlayMusic(defaultMusic);
        }
    }

    //play specific audio clip
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    // play audio clip from library
    public void PlaySFX(string clipName)
    {
        AudioClip clip = SoundLibrary.Instance?.GetClip(clipName);
        if (clip != null)
            PlaySFX(clip); 
    }

    // play specific audio clip
    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    //play music from sound library
    public void PlayMusic(string clipName)
    {
        AudioClip clip = SoundLibrary.Instance?.GetClip(clipName);
        if (clip != null)
            PlayMusic(clip);
    }


}
