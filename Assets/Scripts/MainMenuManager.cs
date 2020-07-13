using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance;
    public static MainMenuManager Instance { get { return instance; } }

    [SerializeField] private AudioSource sfxSource = null; // Initialize a reference to the audio source which will play the sound effects.
    [SerializeField] private AudioSource musicSource = null; // Initialize a reference to the audio source which will play the Music.

    [SerializeField]
    private Slider volumeSlider = null;

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
    }

    private void Start()
    {
        volumeSlider.value = musicSource.volume;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChangeVolume(float sliderValue)
    {
        GamePreferences.Instance.audioVolume = sliderValue;

        musicSource.volume = sliderValue;
        if (sliderValue != 0f)
        {
            sfxSource.volume = sliderValue / 2f;
        }
        else sfxSource.volume = sliderValue;
    }

    public void OpenURL(string link)
    {
        switch(link)
        {
            case "YouTube":
                Application.OpenURL("https://www.youtube.com/channel/UChSeKuELMh9bl1SgHl8ukHA");
                break;
            case "Itch":
                Application.OpenURL("https://zinga.itch.io/");
                break;
            case "Twitter":
                Application.OpenURL("https://twitter.com/ElzingaGames");
                break;
        }
    }

}
