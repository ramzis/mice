using System;
using UnityEngine;

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
