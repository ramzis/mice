using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.Debug;
using Random = UnityEngine.Random;

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

    public float levelTime;
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
        if (Input.GetKeyDown(KeyCode.Escape) && objectiveState == ObjectiveState.INPROGRESS)
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
            mouse.OnHit -= OnAgentHit;
        EventManager.Purge();
    }

    private void OnAgentHit(GameObject agent, string hit)
    {
        if (objectiveState == ObjectiveState.FAILED)
            return;

        switch (hit)
        {
            case "Target":
                agent.SetActive(false);
                targetCount -= 1;
                activeAgentCount -= 1;
                if (targetCount <= 0 && objectiveState != ObjectiveState.COMPLETED)
                    SetObjectiveState(ObjectiveState.COMPLETED);
                break;
            case "Trap":
                agent.SetActive(false);
                activeAgentCount -= 1;
                if (activeAgentCount < targetCount && objectiveState == ObjectiveState.INPROGRESS)
                    SetObjectiveState(ObjectiveState.FAILED);
                break;
            default:
                break;
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
            Log("Time over");
        });

        EventManager.Subscribe(Events.OBJECTIVE_FAILED, _ =>
        {
            SetAgentsStopped(true);
            Log("Objective failed");
        });

        EventManager.Subscribe(Events.OBJECTIVE_COMPLETED, _ =>
        {
            Log("Objective completed");
        });

        EventManager.Subscribe(Events.PAUSED, _ =>
        {
            SetAgentsStopped(true);
            Log("Paused");
        });

        EventManager.Subscribe(Events.UNPAUSED, _ =>
        {
            SetAgentsStopped(false);
            Log("Unpaused");
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

    public float GetLevelTime(int level)
    {
        return 30f;
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
                EventManager.Emit(Events.PAUSED);
                break;
            case TimeState.RUNNING:
                if (prev == TimeState.PAUSED)
                    EventManager.Emit(Events.UNPAUSED);
                EventManager.Emit(Events.RUNNING);
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
                EventManager.Emit(Events.OBJECTIVE_INPROGRESS);
                break;
            case ObjectiveState.COMPLETED:
                EventManager.Emit(Events.OBJECTIVE_COMPLETED);
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
                levelTime -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
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
