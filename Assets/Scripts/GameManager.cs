using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private Checkpoint lastCheckPoint;

    private bool isRemoteBroken = false;

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
}
