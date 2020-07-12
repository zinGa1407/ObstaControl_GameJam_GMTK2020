using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePreferences : MonoBehaviour
{
    private static GamePreferences instance;
    public static GamePreferences Instance { get { return instance; } }

    public float audioVolume { get; set; } = 0.3f;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
