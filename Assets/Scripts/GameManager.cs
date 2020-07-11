using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private Checkpoint lastCheckPoint;

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

    public void RespawnPlayer( GameObject player )
    {
        player.transform.position = lastCheckPoint.GetRespawnPosition();
    }
}
