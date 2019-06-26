using static EventManager;

public class ObjectiveState
{
    public enum State
    {
        NULL,
        INPROGRESS,
        COMPLETED,
        FAILED
    }

    public State state { get; private set; } = State.NULL;

    public ObjectiveState()
    {

    }

    public ObjectiveState(State state)
    {
        this.state = state;
    }

    public void Nullify()
    {
        Set(State.NULL);
    }

    public void Begin()
    {
        Set(State.INPROGRESS);
    }

    public void Complete()
    {
        Set(State.COMPLETED);
    }

    public void Fail()
    {
        Set(State.FAILED);
    }

    private void Set(State next)
    {
        if (next == state) return;
        state = next;
        switch (state)
        {
            case State.INPROGRESS:
                Emit(Events.OBJECTIVE_INPROGRESS);
                break;
            case State.COMPLETED:
                Emit(Events.OBJECTIVE_COMPLETED);
                break;
            case State.FAILED:
                Emit(Events.OBJECTIVE_FAILED);
                break;
            default:
                break;
        }
    }
}
