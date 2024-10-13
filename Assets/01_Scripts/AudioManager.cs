using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAS;
    public AudioSource sfxAS;
    public float musicVol = 0.5f;
    public float sfxVol = 1.0f;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicAS.volume = musicVol;
        musicAS.playOnAwake = true;
        musicAS.loop = true;
        sfxAS.volume = sfxVol;
        sfxAS.playOnAwake = false;
        sfxAS.loop = false;
    }

    public void PlaySound(AudioClip sound)
    {
        sfxAS.PlayOneShot(sound);
    }

    public void SetMusic(AudioClip music)
    {
        musicAS.Stop();
        musicAS.clip = music;
        musicAS.Play();
    }
}