using static EventManager;

public class Time
{
    public enum State
    {
        PAUSED,
        RUNNING,
        OVER
    }

    public State state { get; private set; } = State.PAUSED;

    public Time()
    {

    }

    public Time(State state)
    {
        this.state = state;
    }

    public void Play()
    {
        Set(State.RUNNING);
    }

    public void Pause()
    {
        Set(State.PAUSED);
    }

    public void Resume()
    {
        Set(State.RUNNING);
    }

    public void Stop()
    {
        Set(State.OVER);
    }

    private void Set(State next)
    {
        if (next == state) return;
        var prev = state;
        state = next;
        switch (state)
        {
            case State.PAUSED:
                Emit(Events.PAUSED);
                break;
            case State.RUNNING:
                if (prev == State.PAUSED)
                    Emit(Events.UNPAUSED);
                Emit(Events.RUNNING);
                break;
            case State.OVER:
                Emit(Events.TIME_OVER);
                break;
            default:
                break;
        }
    }
}
