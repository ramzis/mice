using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.Debug;
using static EventManager;
using static ObjectiveState;

public class Level : MonoBehaviour
{
    public interface ITimer
    {
        float GetTime();
        void StartTimer();
    }

    public ObjectiveState objective;
    public LevelAgents<Mouse> agents; //Todo: look into removing this
    public UIManager ui;

    private ITimer timer;
    private int targetCount;
    private int completeTargetCount;

    public void StartLevel(int targetCount, ITimer timer)
    {
        this.timer = timer;
        this.targetCount = targetCount;
        completeTargetCount = 0;

        objective.Begin();
        timer.StartTimer();
        Log("Starting level");
    }

    #region EVENTS

    private void OnEnable()
    {
        Subscribe(Events.AGENT_HIT, OnAgentHit);
        Subscribe(Events.TIME_OVER, OnTimeOver);
        Subscribe(Events.OBJECTIVE_FAILED, OnObjectiveFailed);
        Subscribe(Events.OBJECTIVE_COMPLETED, OnObjectiveCompleted);
    }

    private void OnDisable()
    {
        Unsubscribe(Events.AGENT_HIT, OnAgentHit);
        Unsubscribe(Events.TIME_OVER, OnTimeOver);
        Unsubscribe(Events.OBJECTIVE_FAILED, OnObjectiveFailed);
        Unsubscribe(Events.OBJECTIVE_COMPLETED, OnObjectiveCompleted);
    }

    private void OnAgentHit(dynamic t)
    {
        if(t.Item1 is string && t.Item2 is GameObject)
            switch (t.Item1)
            {
                case "Target":
                    targetCount -= 1;
                    completeTargetCount += 1;
                    if (targetCount == 0)
                        objective.Complete();
                    break;
                case "Trap":
                    agents.Remove(t.Item2);
                    if (agents.activeAgentCount < targetCount)
                        objective.Fail();
                    break;
                default:
                    break;
            }
    }

    private void OnTimeOver(dynamic t)
    {
        Log("Time over");
        if (objective.state == State.INPROGRESS)
            objective.Fail();
    }

    private void OnObjectiveFailed(dynamic t)
    {
        Log("Objective failed");
        float percentage = completeTargetCount / targetCount;
        int stars = 0;
        ui.UpdateCanvas(
            "Oh no! You need to save more mice!",
            $"You saved {completeTargetCount} / {targetCount} mice",
            stars);
        ui.ToggleCanvas(true);
    }

    private void OnObjectiveCompleted(dynamic t)
    {
        Log("Objective completed");
        float percentage = completeTargetCount / targetCount;
        int stars;
        if (percentage <= 0.3f) stars = 1;
        else if (percentage <= 0.8f) stars = 2;
        else stars = 3;
        ui.UpdateCanvas(
            "Level completed!",
            $"You saved {completeTargetCount} / {targetCount} mice",
            stars);
        ui.ToggleCanvas(true);
    }

    #endregion
}
