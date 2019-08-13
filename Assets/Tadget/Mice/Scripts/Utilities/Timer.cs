using System;
using System.Collections;
using UnityEngine;
using static Time;

public interface ITimer
{
    float GetTime();
    void StartTimer();
}

public class Timer : ITimer
{
    private float s;
    private Time time;
    private Coroutine counter;
    private MonoBehaviour mono;

    public Timer Init(float s, Time state, MonoBehaviour mono)
    {
        this.s = s;
        this.time = state;
        this.mono = mono;
        return this;
    }

    public float GetTime()
    {
        return s;
    }

    public void StartTimer()
    {
        counter = mono.StartCoroutine(Counter());
    }

    private IEnumerator Counter()
    {
        time.Play();
        while (true)
        {
            if (time.state == State.RUNNING)
            {
                s -= UnityEngine.Time.deltaTime;
                yield return new WaitForEndOfFrame();
                if (s <= 0)
                {
                    time.Stop();
                    break;
                }
            }
            else
            {
                yield return null;
            }
        }
    }
}
