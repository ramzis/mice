using UnityEngine;
using static UnityEngine.Debug;
using static EventManager;
using static EventReceiver;

public class Level
{
    private Objective objective;
    private ITimer timer;
    private Targets targets;
    private Parameters parameters;

    public Level(ITimer timer, Objective objective)
    {
        this.timer = timer;
        this.objective = objective;
        this.parameters = new Parameters(0, new Targets(0, 0, 0));
        SubscribeEvents();
    }

    public void Setup(Parameters parameters)
    {
        this.parameters = parameters;
        Reset();
    }

    public void Begin()
    {
        this.targets = parameters.levelTargets;
        objective.Begin();
        timer.SetTimer(parameters.levelTime).StartTimer();
    }

    public void Reset()
    {
        timer.PauseTimer();
        objective.Nullify();
        timer.SetTimer(parameters.levelTime);
    }

    #region EVENTS

    private void SubscribeEvents()
    {
        Subscribe<(string tag, GameObject go)>(Events.ON_AGENT_HIT, OnAgentHit);
        Subscribe(Events.ON_TIME_STOPPED, OnTimeOver);
        Subscribe(Events.ON_OBJECTIVE_FAILED, OnObjectiveFailed);
        Subscribe(Events.ON_OBJECTIVE_COMPLETED, OnObjectiveCompleted);
    }

    private void OnAgentHit((string tag, GameObject go) hit)
    {
        switch (hit.tag)
        {
            case "Target":
                targets.complete += 1;
                targets.available -= 1;
                targets.remaining -= 1;
                if (targets.remaining == 0)
                    objective.Complete();
                break;
            case "Trap":
                targets.available -= 1;
                if (targets.available < targets.remaining)
                    objective.Fail();
                break;
            default:
                break;
        }
    }

    private void OnTimeOver()
    {
        if (objective.state == Objective.State.INPROGRESS)
            objective.Fail();
    }

    private void OnObjectiveFailed()
    {
        int stars = 0;
        Emit(Events.DO_UPDATE_CANVAS, ("Oh no! You need to save more mice!",
            $"You saved {targets.complete} / {targets.complete + targets.available} mice",
            stars));
        Emit(Events.DO_TOGGLE_CANVAS, true);
    }

    private void OnObjectiveCompleted()
    {
        Log("Objective completed");
        float percentage = targets.complete / (targets.complete + targets.available);
        int stars;
        if (percentage <= 0.3f) stars = 1;
        else if (percentage <= 0.8f) stars = 2;
        else stars = 3;
        Emit(Events.DO_UPDATE_CANVAS, ("Level completed!",
            $"You saved {targets.complete} / {targets.complete + targets.available} mice",
            stars));
        Emit(Events.DO_TOGGLE_CANVAS, true);
    }

    #endregion
}
