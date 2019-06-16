using static EventManager;

public class LevelState
{
    public enum TimeState
    {
        PAUSED,
        RUNNING,
        OVER
    }

    public enum ObjectiveState
    {
        NULL,
        INPROGRESS,
        COMPLETED,
        FAILED
    }

    public TimeState timeState { get; private set; }
    public ObjectiveState objectiveState { get; private set; }

    public LevelState()
    {
        this.timeState = TimeState.PAUSED;
        this.objectiveState = ObjectiveState.NULL;
    }

    public LevelState(TimeState timeState, ObjectiveState objectiveState)
    {
        this.timeState = timeState;
        this.objectiveState = objectiveState;
    }

    public void Play()
    {
        SetTimeState(TimeState.RUNNING);
    }

    public void Pause()
    {
        SetTimeState(TimeState.PAUSED);
    }

    public void Resume()
    {
        SetTimeState(TimeState.RUNNING);
    }

    public void Stop()
    {
        SetTimeState(TimeState.OVER);
    }

    public void NullifyObjective()
    {
        SetObjectiveState(ObjectiveState.NULL);
    }

    public void BeginObjective()
    {
        SetObjectiveState(ObjectiveState.INPROGRESS);
    }

    public void CompleteObjective()
    {
        SetObjectiveState(ObjectiveState.COMPLETED);
    }

    public void FailObjective()
    {
        SetObjectiveState(ObjectiveState.FAILED);
    }

    private void SetTimeState(TimeState next)
    {
        var prev = timeState;
        timeState = next;
        switch (timeState)
        {
            case TimeState.PAUSED:
                Emit(Events.PAUSED);
                break;
            case TimeState.RUNNING:
                if (prev == TimeState.PAUSED)
                    Emit(Events.UNPAUSED);
                Emit(Events.RUNNING);
                break;
            case TimeState.OVER:
                Emit(Events.TIME_OVER);
                break;
            default:
                break;
        }
    }

    private void SetObjectiveState(ObjectiveState next)
    {
        objectiveState = next;
        switch (objectiveState)
        {
            case ObjectiveState.INPROGRESS:
                Emit(Events.OBJECTIVE_INPROGRESS);
                break;
            case ObjectiveState.COMPLETED:
                Emit(Events.OBJECTIVE_COMPLETED);
                break;
            case ObjectiveState.FAILED:
                Emit(Events.OBJECTIVE_FAILED);
                break;
            default:
                break;
        }
    }
}
