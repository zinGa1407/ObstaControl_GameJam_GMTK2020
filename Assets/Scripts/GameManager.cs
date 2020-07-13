using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private Checkpoint lastCheckPoint;

    private bool isRemoteBroken = false;

    public GameState gameState = GameState.PLAY;

    public enum GameState
    {
        PLAY,
        PAUSE
    }


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
        Time.timeScale = 1f;
    }

    public Checkpoint GetLastCheckpoint()
    {
        return lastCheckPoint;
    }

    public void SetNewCheckpoint( Checkpoint cp )
    {
        lastCheckPoint = cp;
    }

    public void RespawnPlayer( GameObject player, Rigidbody rb )
    {
        rb.velocity = Vector3.zero;
        player.transform.position = lastCheckPoint.GetRespawnPosition();
        SoundManager.Instance.PlaySoundEffect("die");
    }

    public bool GetRemoteBroken()
    {
        return isRemoteBroken;
    }

    public void SetRemoteBroken(bool isBroken)
    {
        isRemoteBroken = isBroken;
    }

    public void SetControllerBroken()
    {
        SetRemoteBroken(true);

        UIManager.Instance.ShowDialogueText("Ooooh no!\nThe freezing temperatures must have broken the remote control\nIt will only get harder now...");
    }

    public void ShowIngameMenu()
    {
        GameObject IngameMenu = UIManager.Instance.GetIngameMenu();
        if (IngameMenu.activeSelf)
        {
            IngameMenu.SetActive(false);
            gameState = GameState.PLAY;
            Time.timeScale = 1f;
        }
        else
        {
            IngameMenu.SetActive(true);
            gameState = GameState.PAUSE;
            Time.timeScale = 0f;
        }
    }

    public void LoadScene(int v)
    {
        SceneManager.LoadScene(v);
        Time.timeScale = 1f;
    }

    public void FinishReached()
    {
        UIManager.Instance.ShowGameOverScreen();
        gameState = GameState.PAUSE;
    }
}
