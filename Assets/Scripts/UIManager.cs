using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    

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

    private List<string> feedbackQueue = new List<string>();
    private float feedbackTimer = 0f;
    private float FEEDBACK_TIME = 4f;

    [SerializeField]
    private TMP_Text feedbackText = null;

    [SerializeField]
    private GameObject RemoteInstructions = null;
    [SerializeField]
    private GameObject RemoteVisual = null;

    [SerializeField]
    private GameObject ingameMenu;

    [SerializeField]
    private Slider volumeSlider;

    private void Start()
    {
        if (SoundManager.Instance != null) volumeSlider.value = SoundManager.Instance.GetMusicSourceVolume();
    }

    private void Update()
    {
        if (feedbackTimer > 0f)
            feedbackTimer -= Time.deltaTime;
        else if (feedbackQueue.Count > 0)
        {
            ShowFeedbackText();
        }
        else DisableFeedbackText();
    }

    public GameObject GetIngameMenu()
    {
        return ingameMenu;
    }

    public void AddFeedbackText(string feedbacktxt)
    {
        feedbackQueue.Add(feedbacktxt);
    }

    private void ShowFeedbackText()
    {
        feedbackText.text = feedbackQueue[0];
        feedbackText.gameObject.SetActive(true);
        feedbackQueue.RemoveAt(0);
        feedbackTimer = FEEDBACK_TIME;
    }

    private void DisableFeedbackText()
    {
        feedbackText.text = "";
        feedbackText.gameObject.SetActive(true);
    }

    public void ActivateUIElement(string element)
    {
        switch(element)
        {
            case "RemoteControl":
                if(RemoteInstructions != null) RemoteInstructions.SetActive(true);
                if(RemoteVisual != null) RemoteVisual.SetActive(true);
                break;
        }
    }

    public void ChangeVolume(float sliderValue)
    {
        GamePreferences.Instance.audioVolume = sliderValue;

        if (SoundManager.Instance != null) SoundManager.Instance.ChangeSourceVolume(sliderValue);
    }

    public void OpenURL(string link)
    {
        switch (link)
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
