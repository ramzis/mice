using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.Debug;
using static UnityEngine.Random;
using static LevelTools;
using static EventManager;
using static LevelState;

public class Level : MonoBehaviour
{
    public LevelState state;
    public LevelAgents<Mouse> agents;
    public UIManager ui;

    public float time;
    public int targetCount;
    public int completeTargetCount;

    private void OnEnable()
    {
        SetEventRules();
    }

    private void OnDisable()
    {
        Purge();
    }

    private void Awake()
    {
        InitState(DateTime.Now.Millisecond);
    }

    private void Start()
    {
        state = new LevelState();
        agents = new LevelAgents<Mouse>();
        gameObject.AddComponent<InputManager>().Init(state);

        time = GetLevelTime(0);
        targetCount = GetLevelTargetCount(0);
        completeTargetCount = 0;

        state.BeginObjective();
        StartCoroutine(Counter());
    }

    private void FixedUpdate()
    {
        if(state.timeState == TimeState.RUNNING)
        {
            agents.Tick();
        }
    }

    private void OnAgentHit(GameObject agent, string hit)
    {
        if (state.objectiveState == ObjectiveState.FAILED)
            return;

        switch (hit)
        {
            case "Target":
                agents.Remove(agent);
                targetCount -= 1;
                completeTargetCount += 1;
                if (targetCount == 0)
                    state.CompleteObjective();
                break;
            case "Trap":
                agents.Remove(agent);
                if (agents.activeAgentCount < targetCount)
                    state.FailObjective();
                break;
            default:
                break;
        }
    }

    public void SetEventRules()
    {
        Subscribe(Events.AGENT_HIT, data => OnAgentHit(data.Item1, data.Item2));

        Subscribe(Events.TIME_OVER, _ =>
        {
            if (state.objectiveState == ObjectiveState.INPROGRESS)
                state.FailObjective();
            Log("Time over");
        });

        Subscribe(Events.OBJECTIVE_FAILED, _ =>
        {
            Log("Objective failed");
            float percentage = completeTargetCount / GetLevelTargetCount(0);
            int stars = 0;
            ui.UpdateCanvas(
                "Oh no! You need to save more mice!",
                $"You saved {completeTargetCount} / {GetLevelTargetCount(0)} mice",
                stars);
            ui.ToggleCanvas(true);
        });

        Subscribe(Events.OBJECTIVE_COMPLETED, _ =>
        {
            Log("Objective completed");
            float percentage = completeTargetCount / GetLevelTargetCount(0);
            int stars;
            if (percentage <= 0.3f) stars = 1;
            else if (percentage <= 0.8f) stars = 2;
            else stars = 3;
            ui.UpdateCanvas(
                "Level completed!",
                $"You saved {completeTargetCount} / {GetLevelTargetCount(0)} mice",
                stars);
            ui.ToggleCanvas(true);
        });
    }

    private IEnumerator Counter()
    {
        while (true)
        {
            if (state.timeState == TimeState.RUNNING)
            {
                time -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
                if (time <= 0)
                {
                    state.Stop();
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
