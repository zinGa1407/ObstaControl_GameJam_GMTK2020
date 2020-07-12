using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    [SerializeField] private AudioSource sfxSource = null; // Initialize a reference to the audio source which will play the sound effects.
    [SerializeField] private AudioSource musicSource = null; // Initialize a reference to the audio source which will play the Music.

    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    [Serializable]
    public struct AudioClips
    {
        public string name;
        public AudioClip clip;
    }
    public AudioClips[] audioFiles;

    //[SerializeField] private AudioClip shootingSound = null;
    //[SerializeField] private AudioClip gameWon = null;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }

        audioClips = audioFiles.ToDictionary(key => key.name, value => value.clip);
        //Debug.Log("Current amount of audio clips in Dictionary : " + audioClips.Count);


        //Set Volume
        if (GamePreferences.Instance != null)
        {
            //Debug.Log("Change Volume called with value of : " + GamePreferences.Instance.audioVolume);
            //Set Volume on Sources
            ChangeSourceVolume(GamePreferences.Instance.audioVolume);
        }
    }

    public void ChangeSourceVolume(float sliderValue)
    {
        musicSource.volume = sliderValue;
        if (sliderValue != 0f)
        {
            sfxSource.volume = sliderValue / 2f;
        }
        else sfxSource.volume = sliderValue;

    }

    public float GetMusicSourceVolume()
    {
        return musicSource.volume;
    }

    public void PlayMusic()
    {
        if(musicSource != null) musicSource.Play();
    }

    /// <summary>
    /// Play sound effect
    /// </summary>
    /// <param name="clip">Audioclip to play</param>
    public void PlaySoundEffect(AudioClip clip)
    {
        if (sfxSource != null) sfxSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Play sound effect
    /// </summary>
    /// <param name="clip">Name of the clip to play</param>
    public void PlaySoundEffect(string clip)
    {
        if (sfxSource != null)
        {
            if (audioClips.ContainsKey(clip)) sfxSource.PlayOneShot(audioClips[clip]);

        }
    }
}
