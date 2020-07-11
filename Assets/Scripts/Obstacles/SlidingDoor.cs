using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour, IObstacle
{
    private bool activeInteraction = false;

    public Vector3 startPosition;
    public Vector3 endPosition;

    private float speed = 2.0f;
    private float minSpeed = 1.0f;
    private float maxSpeed = 5.0f;

    private float randomSpeedTimer = 2.0f;

    public void Interaction(bool interacting)
    {
        activeInteraction = interacting;
    }

    private void Update()
    {
        if(activeInteraction)
        {
            Vector3 v = startPosition;
            v.y += Mathf.Abs(startPosition.y - endPosition.y) * Mathf.Abs(Mathf.Sin(Time.time * speed));
            transform.position = v;

            if(GameManager.Instance.GetRemoteBroken())
            {
                if (randomSpeedTimer >= 0f) randomSpeedTimer -= Time.deltaTime;
                else GetRandomSpeed();
            }
        }
    }

    private void GetRandomSpeed()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

}
