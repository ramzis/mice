﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.Debug;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class Mouse : MonoBehaviour
{
    public enum State
    {
        MOVING,
        ROTATING,
        STOPPED,
        REMOVED
    }

    public State state;
    public float rayCircleRadius;
    public Vector2[] sensors;
    public Vector2 sensorOffset;
    public bool act = true;

    public LayerMask hitLayerMask;
    public float forwardVelocity;
    public float rotationAngle;

    private SpriteRenderer sprite;
    private float rotationDir;
    private bool directionChosen;

    private void Start()
    {
        Random.InitState(DateTime.Now.Millisecond);
        circleResults = new Collider2D[2];
        sprite = GetComponent<SpriteRenderer>();
        directionChosen = false;
        validHits = new bool[3];
    }

    private void SetState(State newState)
    {
        state = newState;
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
    public void UpdateState()   
    {
        if (state == State.STOPPED)
            return;

        for(int i = 0; i < sensors.Length; i++)
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
                    if(circleResults[j].CompareTag("Mouse") || circleResults[j].CompareTag("Wall"))
                    {
                        SetState(State.ROTATING);
                        validHit = true;
                    }
                    else if (circleResults[j].CompareTag("Target"))
                    {
                        if (i == 1)
                        {
                            SetState(State.STOPPED);
                            validHit = true;
                        }
                    }
                    if (validHit) break;
                }
            }
            validHits[i] = validHit;
        }

        if((validHits[1] && (validHits[0] || validHits[2])) && directionChosen)
        {

        }
        else if(((validHits[0] && validHits[2]) || validHits[1]) && !directionChosen)
        {
            directionChosen = true;
            rotationDir = Random.value > 0.5f ? 1 : -1;
        }
        else if (validHits[0])
        {
            directionChosen = false;
            rotationDir = -1f;
        }
        else if(validHits[2])
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

    public void Act()
    {
        if(!act) return;
        //DrawRay(transform.position + sprite.sprite.bounds.extents.y * transform.up, 
        //    transform.up * rayDistance, Color.magenta);

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
            transform.Translate(Vector3.up * forwardVelocity * Time.deltaTime, Space.Self);
        }

        void Rotate()
        {
            transform.Rotate(Vector3.forward, (0.95f + (Random.value * 0.1f)) * rotationAngle * rotationDir * Time.deltaTime, Space.Self);   
        }
    }
}