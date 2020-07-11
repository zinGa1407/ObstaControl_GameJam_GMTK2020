using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPosition = null;

    public Vector3 GetRespawnPosition()
    {
        if (respawnPosition == null)
        {
            Debug.LogError("No RespawnPosition set on the checkpoint :" + gameObject.name);
            return Vector3.zero;
        }
        return respawnPosition.position;
    }
}
