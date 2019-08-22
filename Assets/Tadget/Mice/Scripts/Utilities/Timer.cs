using System.Collections;
using UnityEngine;

public interface ITimer
{
    float GetTime();
    Timer StartTimer();
    Timer PauseTimer();
    Timer StopTimer();
    Timer SetTimer(float time);
}

/// <summary>
/// Controls Time with an ITimer implementation.
/// Implements ITimer using a Coroutine as the timekeeping solution.
/// </summary>
public class Timer : ITimer
{
    private float s;
    private Time time;
    private Coroutine counter;
    private MonoBehaviour mono;

    public Timer(Time time, MonoBehaviour mono)
    {
        this.time = time;
        this.mono = mono;
        counter = mono.StartCoroutine(Counter());
    }

    public float GetTime()
    {
        return s;
    }

    public Timer SetTimer(float s)
    {
        this.s = s;
        return this;
    }

    public Timer PauseTimer()
    {
        time.Pause();
        return this;
    }

    public Timer StartTimer()
    {
        time.Play();
        return this;
    }

    public Timer StopTimer()
    {
        time.Stop();
        return this;
    }

    /// <summary>
    /// Subtracts Time.deltaTime from s when the time state is PLAYING.
    /// </summary>
    private IEnumerator Counter()
    {
        while (true)
        {
            if (time.state == Time.State.PLAYING)
            {
                yield return new WaitForEndOfFrame();
                s -= UnityEngine.Time.deltaTime;
                if (s <= 0)
                {
                    s = 0;
                    time.Stop();
                }
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
