﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObstacle
{
    void Interaction(bool interacting);

    void ShowFeedbackText(bool active);
}
