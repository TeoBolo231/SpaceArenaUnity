using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource gameMusic;

    public static AudioManager instance = null;

    void Awake()
    {
        AudioManagerSingleton();
    }

    private void AudioManagerSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip clip)
    {
        gameMusic.clip = clip;
        gameMusic.Play();
    }
}
