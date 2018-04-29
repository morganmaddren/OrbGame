using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Timer
{
    public float Duration { get; set; }
    public float Elapsed { get; private set; }

    public Timer(float duration)
    {
        this.Duration = duration;
    }

    public bool CheckAndTick(float deltaTime)
    {
        bool result = false;
        if (Elapsed >= Duration)
        {
            result = true;
            Elapsed -= Duration;
        }

        Elapsed += deltaTime;

        return result;
    }

    public void Restart()
    {
        Elapsed = 0;
    }

    public void Restart(float newDuration)
    {
        this.Duration = newDuration;
        Elapsed = 0;
    }
}
