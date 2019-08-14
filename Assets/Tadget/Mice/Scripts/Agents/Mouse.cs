using System;
using UnityEngine;
using Random = UnityEngine.Random;
using static EventManager;

[DisallowMultipleComponent]
public class Mouse : Agent
{
    public float rayCircleRadius;
    public Vector2[] sensors;
    public Vector2 sensorOffset;

    public LayerMask hitLayerMask;
    public float forwardVelocity;
    public float rotationAngle;

    private float rotationDir;
    private bool directionChosen;

    private void Awake()
    {
        circleResults = new Collider2D[2];
        directionChosen = false;
        validHits = new bool[3];
        SetState(State.STOPPED);
    }

    private void OnEnable()
    {
        Subscribe(Events.PAUSED, TimeStatePausedEvent);
        Subscribe(Events.UNPAUSED, TimeStateUnpausedEvent);
        Subscribe(Events.TIME_OVER, TimeStateOverEvent);
    }

    private void OnDisable()
    {
        Unsubscribe(Events.PAUSED, TimeStatePausedEvent);
        Unsubscribe(Events.UNPAUSED, TimeStateUnpausedEvent);
        Unsubscribe(Events.TIME_OVER, TimeStateOverEvent);
    }

    private void TimeStatePausedEvent(dynamic t)
    {
        if (state != State.REMOVED)
            SetState(State.STOPPED);
    }

    private void TimeStateUnpausedEvent(dynamic t)
    {
        if (state != State.REMOVED)
            SetState(State.MOVING);
    }

    private void TimeStateOverEvent(dynamic t)
    {
        if (state != State.REMOVED)
            SetState(State.STOPPED);
    }

    private void OnDrawGizmos()
    {
        foreach (var s in sensors)
            Gizmos.DrawWireSphere(transform.position + transform.TransformDirection(sensorOffset + s), rayCircleRadius);
    }

    private Collider2D[] circleResults;
    bool[] validHits;
    int circleHits;
    bool validHit;
    public override void UpdateState()
    {
        for (int i = 0; i < sensors.Length; i++)
        {
            circleHits = Physics2D.OverlapCircleNonAlloc(
                transform.position + transform.TransformDirection(sensors[i] + sensorOffset),
                rayCircleRadius,
                circleResults,
                hitLayerMask);

            validHit = false;
            for (int j = 0; j < circleHits; j++)
            {
                if (circleResults[j].gameObject != gameObject)
                {
                    if (circleResults[j].CompareTag("Mouse") || circleResults[j].CompareTag("Wall"))
                    {
                        SetState(State.ROTATING);
                        validHit = true;
                    }
                    else if (i == 1 && circleResults[j].CompareTag("Target"))
                    {
                        SetState(State.REMOVED);
                        validHit = true;
                        Emit(Events.AGENT_HIT, (gameObject, "Target"));
                        gameObject.SetActive(false);
                    }
                    else if (i == 1 && circleResults[j].CompareTag("Trap"))
                    {
                        SetState(State.REMOVED);
                        validHit = true;
                        Emit(Events.AGENT_HIT, (gameObject, "Trap"));
                        gameObject.SetActive(false);
                    }
                    if (validHit) break;
                }
            }
            validHits[i] = validHit;
        }

        if ((validHits[1] && (validHits[0] || validHits[2])) && directionChosen)
        {

        }
        else if (((validHits[0] && validHits[2]) || validHits[1]) && !directionChosen)
        {
            directionChosen = true;
            rotationDir = Random.value > 0.5f ? 1 : -1;
        }
        else if (validHits[0])
        {
            directionChosen = false;
            rotationDir = -1f;
        }
        else if (validHits[2])
        {
            directionChosen = false;
            rotationDir = 1f;
        }
        else
        {
            directionChosen = false;
            SetState(State.MOVING);
        }
    }

    public override void Act()
    {
        switch (state)
        {
            case State.STOPPED:
                break;
            case State.REMOVED:
                break;
            case State.MOVING:
                Move();
                break;
            case State.ROTATING:
                Rotate();
                break;
            default:
                throw new Exception($"Unhandled Mouse.State {state}");
        }

        void Move()
        {
            transform.Translate(Vector3.up * forwardVelocity * UnityEngine.Time.deltaTime, Space.Self);
        }

        void Rotate()
        {
            transform.Rotate(Vector3.forward, (0.95f + (Random.value * 0.1f)) * rotationAngle * rotationDir * UnityEngine.Time.deltaTime, Space.Self);
        }
    }
}
