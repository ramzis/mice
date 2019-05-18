using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;
using Random = UnityEngine.Random;


public sealed partial class Events
{
    public static readonly string UNPAUSE = "UNPAUSE";
    public static readonly string PAUSE = "PAUSE";
    public static readonly string OBJECTIVE_FAILED = "OBJECTIVE_FAILED";
    public static readonly string OBJECTIVE_COMPLETED = "OBJECTIVE_COMPLETED";
    public static readonly string TIME_OVER = "TIME_OVER";
}

public class GameManager : MonoBehaviour
{
    public enum TimeState
    {
        PAUSED,
        RUNNING,
        OVER
    }

    public enum ObjectiveState
    {
        INPROGRESS,
        COMPLETED,
        FAILED
    }

    public TimeState timeState;
    public ObjectiveState objectiveState;

    public int levelTime;
    public int targetCount;
    public int activeAgentCount;

    public Mouse[] mice;

    private void Awake()
    {
        Random.InitState(DateTime.Now.Millisecond);
    }

    void FixedUpdate()
    {
        foreach (Mouse mouse in mice)
        {
            mouse.UpdateState();
        }
        foreach (Mouse mouse in mice)
        {
            mouse.Act();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (timeState)
            {
                case TimeState.PAUSED:
                    SetTimeState(TimeState.RUNNING);
                    break;
                case TimeState.RUNNING:
                    SetTimeState(TimeState.PAUSED);
                    break;
                case TimeState.OVER:
                    break;
                default:
                    break;
            }
        }
    }

    private void Start()
    {
        mice = FindObjectsOfType<Mouse>();
        activeAgentCount = mice.Length;
        InitLevelStates();
        levelTime = GetLevelTime(0);
        targetCount = GetLevelTargetCount(0);
        SetEventRules();
        StartCoroutine(Counter());
    }

    private void OnDisable()
    {
        var mice = FindObjectsOfType<Mouse>();
        foreach (var mouse in mice)
        {
            mouse.OnHit -= OnAgentHit;
        }
    }

    private void OnAgentHit(GameObject agent, string hit)
    {
        if (hit == "Target" && objectiveState != ObjectiveState.FAILED)
        {
            agent.SetActive(false);
            targetCount -= 1;
            activeAgentCount -= 1;
            if (targetCount <= 0 && objectiveState != ObjectiveState.COMPLETED)
            {
                SetObjectiveState(ObjectiveState.COMPLETED);
            }
        }
        else if (hit == "Trap" && objectiveState != ObjectiveState.FAILED)
        {
            agent.SetActive(false);
            activeAgentCount -= 1;
            if (activeAgentCount < targetCount && objectiveState != ObjectiveState.COMPLETED)
            {
                SetObjectiveState(ObjectiveState.FAILED);
            }
        }
    }

    public void SetEventRules()
    {
        foreach (var mouse in mice)
            mouse.OnHit += OnAgentHit;

        EventManager.Subscribe(Events.TIME_OVER, _ =>
        {
            if (objectiveState == ObjectiveState.INPROGRESS)
                SetObjectiveState(ObjectiveState.FAILED);
        });

        EventManager.Subscribe(Events.OBJECTIVE_FAILED, _ =>
        {
            SetAgentsStopped(true);
            Log("Objective failed");
        });

        EventManager.Subscribe(Events.PAUSE, _ =>
        {
            SetAgentsStopped(true);
        });

        EventManager.Subscribe(Events.UNPAUSE, _ =>
        {
            SetAgentsStopped(false);
        });
    }

    public void SetAgentsStopped(bool stopped)
    {
        foreach (var mouse in FindObjectsOfType<Mouse>())
        {
            if(mouse.state != Mouse.State.REMOVED)
            {
                mouse.SetState(stopped ? Mouse.State.STOPPED : Mouse.State.MOVING);
                mouse.UpdateState();
            }
        }
    }

    public int GetLevelTime(int level)
    {
        return 30;
    }

    public int GetLevelTargetCount(int level)
    {
        return 8;
    }

    public void InitLevelStates()
    {
       SetTimeState(TimeState.PAUSED);
       SetObjectiveState(ObjectiveState.INPROGRESS);
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

    public void SetTimeState(TimeState next)
    {
        var prev = timeState;
        timeState = next;
        switch (timeState)
        {
            case TimeState.PAUSED:
                EventManager.Emit(Events.PAUSE);
                break;
            case TimeState.RUNNING:
                if (prev == TimeState.PAUSED)
                    EventManager.Emit(Events.UNPAUSE);
                break;
            case TimeState.OVER:
                EventManager.Emit(Events.TIME_OVER);
                break;
            default:
                break;
        }
    }

    public void SetObjectiveState(ObjectiveState next)
    {
        objectiveState = next;
        switch (objectiveState)
        {
            case ObjectiveState.INPROGRESS:
                break;
            case ObjectiveState.COMPLETED:
                break;
            case ObjectiveState.FAILED:
                EventManager.Emit(Events.OBJECTIVE_FAILED);
                break;
            default:
                break;
        }
    }

    public IEnumerator Counter()
    {
        while(timeState != TimeState.OVER && objectiveState != ObjectiveState.FAILED)
        {
            if(timeState == TimeState.RUNNING)
            {
                yield return new WaitForSeconds(1f);
                levelTime -= 1;
                if(levelTime <= 0)
                    SetTimeState(TimeState.OVER);
            }
            else
            {
                yield return null;
            }
        }
    }
}
