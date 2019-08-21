using System.Collections;
using UnityEngine;

public interface ITimer
{
    float GetTime();
    Timer StartTimer();
    Timer PauseTimer();
    Timer SetTimer(float time);
}

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
                yield return null;
            }
        }
    }
}
