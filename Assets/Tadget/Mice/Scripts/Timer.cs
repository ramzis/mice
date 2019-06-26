using System;
using System.Collections;
using UnityEngine;
using static TimeState;

public class Timer : Level.ITimer
{
    private float s;
    private TimeState time;
    private Coroutine counter;
    private MonoBehaviour mono;

    public Timer Init(float s, TimeState state, MonoBehaviour mono)
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
                s -= Time.deltaTime;
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
