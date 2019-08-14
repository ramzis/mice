using System;
using System.Collections;
using UnityEngine;
using static Time;
using static EventManager;

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
        if (counter != null)
        {
            throw new Exception("Attempting to start a counter while another one exists.");
        }
        else
        {
            counter = mono.StartCoroutine(Counter());
        }
    }

    private IEnumerator Counter()
    {
        time.Play();
        while (true)
        {
            if (time.state == State.RUNNING)
            {
                yield return new WaitForEndOfFrame();
                s -= UnityEngine.Time.deltaTime;
                if (s <= 0)
                {
                    s = 0;
                    time.Stop();
                    break;
                }
            }
            else
            {
                yield return null;
            }
        }
        counter = null;
    }
}
