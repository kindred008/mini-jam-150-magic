using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    private float _elapsedTime = 0.0f;
    private float _interval;

    public UnityEvent IntervalPassed { get; private set; } = new UnityEvent();

    public Timer(float interval)
    {
        _interval = interval;
    }

    public void Update(float deltaTime)
    {
        _elapsedTime += deltaTime;

        while (_elapsedTime > _interval)
        {
            _elapsedTime -= _interval;

            IntervalPassed.Invoke();
        }
    }
}
