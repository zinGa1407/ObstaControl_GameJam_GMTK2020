using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour, IObstacle
{
    private bool activeInteraction = false;

    [SerializeField]
    private bool isBroken = false;

    [SerializeField]
    private bool cannotBeTurnedOff = false;

    [SerializeField]
    private GameObject thisObstacle = null;

    [SerializeField]
    private GameObject feedbackText = null;

    public Transform startPosition;
    public Transform endPosition;

    [SerializeField]
    private float speed = 2.0f;
    private float minSpeed = 1.0f;
    private float maxSpeed = 2.0f;

    private float randomSpeedTimer = 2.0f;

    private float minRandomTimer = 2.0f;
    private float maxRandomTimer = 4.0f;

    public void Interaction(bool interacting)
    {
        if (activeInteraction && cannotBeTurnedOff) return;
        activeInteraction = interacting;
    }

    private void Update()
    {
        if (activeInteraction)
        {
            if (!thisObstacle.activeSelf) thisObstacle.SetActive(true);

            if (isBroken)
            {
                //Vector3 v = startPosition.position;
                Vector3 v = startPosition.position;
                v.x += (endPosition.position.x - startPosition.position.x) * Mathf.Abs(Mathf.Sin(Time.time * speed));
                v.y += (endPosition.position.y - startPosition.position.y) * Mathf.Abs(Mathf.Sin(Time.time * speed));
                v.z += (endPosition.position.z - startPosition.position.z) * Mathf.Abs(Mathf.Sin(Time.time * speed));
                thisObstacle.transform.position = v;

                /*if (GameManager.Instance.GetRemoteBroken())
                {
                    if (randomSpeedTimer >= 0f) randomSpeedTimer -= Time.deltaTime;
                    else GetRandomSpeed();
                }*/
            }
            else
            {
                thisObstacle.transform.position = Vector3.Slerp(thisObstacle.transform.position, endPosition.position, Time.deltaTime);
            }

        }
    }

    private void GetRandomSpeed()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        randomSpeedTimer = Random.Range(minRandomTimer, maxRandomTimer);
    }

    public void ShowFeedbackText(bool _active)
    {
        if (feedbackText != null) feedbackText.SetActive(_active);
    }
}
