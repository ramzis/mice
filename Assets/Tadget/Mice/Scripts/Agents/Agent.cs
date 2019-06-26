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
    public Action<GameObject, string> OnHit;
    public abstract void UpdateState();
    public abstract void Act();

    public void SetState(State next)
    {
        if (state == next) return;
        state = next;
    }
}
