using System;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public Action<GameObject, string> OnHit;
    public abstract void UpdateState();
    public abstract void Act();
}
