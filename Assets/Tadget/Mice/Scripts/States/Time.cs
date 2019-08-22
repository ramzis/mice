using static EventManager;

public class Time
{
    public enum State
    {
        PAUSED,
        PLAYING,
        STOPPED
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
        Set(State.PLAYING);
    }

    public void Pause()
    {
        Set(State.PAUSED);
    }

    public void Stop()
    {
        Set(State.STOPPED);
    }

    private void Set(State next)
    {
        if (next == state) return;
        var prev = state;
        state = next;
        switch (state)
        {
            case State.PAUSED:
                Emit(Events.ON_PAUSED);
                break;
            case State.PLAYING:
                if (prev == State.PAUSED)
                    Emit(Events.ON_UNPAUSED);
                else
                    Emit(Events.ON_RUNNING);
                break;
            case State.STOPPED:
                Emit(Events.ON_TIME_STOPPED);
                break;
            default:
                break;
        }
    }
}
