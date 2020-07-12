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
    private float FEEDBACK_TIME = 3f;

    [SerializeField]
    private TMP_Text feedbackText = null;

    [SerializeField]
    private GameObject ingameMenu = null;

    [SerializeField]
    private Slider volumeSlider = null;

    [SerializeField]
    private SlowTextAdder slowTextAdder = null;

    private void Start()
    {
        if (SoundManager.Instance != null) volumeSlider.value = SoundManager.Instance.GetMusicSourceVolume();

        //Start of game dialogue
        StartDialogue();
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

    public void StartDialogue()
    {
        slowTextAdder.StartNewDialogueText("Welcome test subject #001407\nGrab the remote control and get to the end\nGood Luck...");
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
