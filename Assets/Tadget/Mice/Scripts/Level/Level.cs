using UnityEngine;
using static UnityEngine.Debug;
using static EventManager;
using static EventReceiver;

public class Level
{
    private Objective objective;
    private ITimer timer;
    private Targets targets;
    // TODO: get actual level index through Begin()

    public Level(ITimer timer, Objective objective)
    {
        this.timer = timer;
        this.objective = objective;
        SubscribeEvents();
    }

    public void Begin()
    {
        // TODO: spawn items
        SetTargets();
        objective.Begin();
        timer.StartTimer();
    }

    public void Reset()
    {
        timer.PauseTimer();
        objective.Nullify();
        timer.SetTimer(LevelTools.GetLevelTime(0));
        // TODO: despawn items if they exist
    }

    public void SetTargets()
    {
        this.targets = new Targets(
            LevelTools.GetLevelTargetCount(0),
            UnityEngine.GameObject.FindObjectsOfType<Agent>().Length,
            0);
    }

    #region EVENTS

    private void SubscribeEvents()
    {
        Subscribe<(string tag, GameObject go)>(Events.AGENT_HIT, OnAgentHit);
        Subscribe(Events.TIME_OVER, OnTimeOver);
        Subscribe(Events.OBJECTIVE_FAILED, OnObjectiveFailed);
        Subscribe(Events.OBJECTIVE_COMPLETED, OnObjectiveCompleted);
        Subscribe(Events.LEVEL_BEGIN, OnLevelBegin);
        Subscribe(Events.LEVEL_RESET, OnLevelReset);
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
        Emit(Events.UPDATE_CANVAS, ("Oh no! You need to save more mice!",
            $"You saved {targets.complete} / {targets.complete + targets.available} mice",
            stars));
        Emit(Events.TOGGLE_CANVAS, true);
    }

    private void OnObjectiveCompleted()
    {
        Log("Objective completed");
        float percentage = targets.complete / (targets.complete + targets.available);
        int stars;
        if (percentage <= 0.3f) stars = 1;
        else if (percentage <= 0.8f) stars = 2;
        else stars = 3;
        Emit(Events.UPDATE_CANVAS, ("Level completed!",
            $"You saved {targets.complete} / {targets.complete + targets.available} mice",
            stars));
        Emit(Events.TOGGLE_CANVAS, true);
    }

    private void OnLevelBegin()
    {
        Begin();
    }

    private void OnLevelReset()
    {
        Reset();
    }

    #endregion
}
