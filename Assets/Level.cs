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

    public float time;
    public int targetCount;
    public int completeTargetCount;

    private void Awake()
    {
        InitState(DateTime.Now.Millisecond);
    }

    private void Start()
    {
        // TODO inject state to input manager
        state = new LevelState();
        agents = new LevelAgents<Mouse>();
        gameObject.AddComponent<InputManager>().Init(state);

        time = GetLevelTime(0);
        targetCount = GetLevelTargetCount(0);
        completeTargetCount = 0;

        SetEventRules();
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

    private void OnDisable()
    {
        var mice = GameObject.FindObjectsOfType<Mouse>();
        foreach (var mouse in mice)
            mouse.OnHit -= OnAgentHit;
        Purge();
    }

    private void OnAgentHit(GameObject agent, string hit)
    {
        if (state.objectiveState == ObjectiveState.FAILED)
            return;

        switch (hit)
        {
            case "Target":
                agent.SetActive(false);
                targetCount -= 1;
                completeTargetCount += 1;
                agents.activeAgentCount -= 1;
                // TODO: think of "decorator" for changing state to same state
                if (targetCount <= 0 && state.objectiveState != ObjectiveState.COMPLETED)
                    state.CompleteObjective();
                break;
            case "Trap":
                agent.SetActive(false);
                agents.activeAgentCount -= 1;
                if (agents.activeAgentCount < targetCount && state.objectiveState == ObjectiveState.INPROGRESS)
                    state.FailObjective();
                break;
            default:
                break;
        }
    }

    public void SetEventRules()
    {
        foreach (var mouse in agents.agents)
            mouse.OnHit += OnAgentHit;

        Subscribe(Events.TIME_OVER, _ =>
        {
            if (state.objectiveState == ObjectiveState.INPROGRESS)
                state.FailObjective();
            Log("Time over");
        });

        Subscribe(Events.OBJECTIVE_FAILED, _ =>
        {
            SetAgentsStopped(true);
            Log("Objective failed");
            float percentage = completeTargetCount / GetLevelTargetCount(0);
            int stars = 0;
            UIManager.Instance.UpdateCanvas(
                "Oh no! You need to save more mice!",
                $"You saved {completeTargetCount} / {GetLevelTargetCount(0)} mice",
                stars);
            UIManager.Instance.ToggleCanvas(true);
        });

        Subscribe(Events.OBJECTIVE_COMPLETED, _ =>
        {
            Log("Objective completed");
            float percentage = completeTargetCount / GetLevelTargetCount(0);
            int stars;
            if (percentage <= 0.3f) stars = 1;
            else if (percentage <= 0.8f) stars = 2;
            else stars = 3;
            UIManager.Instance.UpdateCanvas(
                "Level completed!",
                $"You saved {completeTargetCount} / {GetLevelTargetCount(0)} mice",
                stars);
            UIManager.Instance.ToggleCanvas(true);
        });

        Subscribe(Events.PAUSED, _ =>
        {
            Log("Paused");
        });

        Subscribe(Events.UNPAUSED, _ =>
        {
            Log("Unpaused");
        });
    }

    // TODO refactor state to be local to agent
    public void SetAgentsStopped(bool stopped)
    {
        foreach (var mouse in GameObject.FindObjectsOfType<Mouse>())
        {
            if(mouse.state != Mouse.State.REMOVED)
            {
                mouse.SetState(stopped ? Mouse.State.STOPPED : Mouse.State.MOVING);
                mouse.UpdateState();
            }
        }
    }

    private IEnumerator Counter()
    {
        while (state.timeState != TimeState.OVER && state.objectiveState != ObjectiveState.FAILED)
        {
            if (state.timeState == TimeState.RUNNING)
            {
                time -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
                if (time <= 0)
                    state.Stop();
            }
            else
            {
                yield return null;
            }
        }
    }

}
