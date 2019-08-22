using System;
using System.Collections.Generic;
using UnityEngine;
using static EventReceiver;

public abstract class Agent : MonoBehaviour
{
    public enum State
    {
        MOVING,
        ROTATING,
        STOPPED,
        REMOVED
    }

    public State state { get; private set; }
    public abstract void UpdateState();
    public abstract void Act();

    private List<Action> actionHandles;

    private void OnEnable()
    {
        actionHandles = new List<Action>();
        actionHandles.Add(Subscribe(Events.ON_PAUSED, TimeStatePausedEvent));
        actionHandles.Add(Subscribe(Events.ON_UNPAUSED, TimeStateUnpausedEvent));
        actionHandles.Add(Subscribe(Events.ON_TIME_STOPPED, TimeStateStoppedEvent));
    }

    private void OnDisable()
    {
        foreach (var dispose in actionHandles)
            dispose();
    }

    private void TimeStatePausedEvent()
    {
        if (state != State.REMOVED)
            SetState(State.STOPPED);
    }

    private void TimeStateUnpausedEvent()
    {
        if (state != State.REMOVED)
            SetState(State.MOVING);
    }

    private void TimeStateStoppedEvent()
    {
        if (state != State.REMOVED)
            SetState(State.STOPPED);
    }

    public void SetState(State next)
    {
        if (state == next) return;
        state = next;
    }

    private void FixedUpdate()
    {
        if (state == State.REMOVED || state == State.STOPPED)
            return;

        UpdateState();
        Act();
    }
}
