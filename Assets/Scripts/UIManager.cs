﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

}