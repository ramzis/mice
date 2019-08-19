using System;
using System.Collections.Generic;
// Abstracts the implementation of event management
// Creates a way to contain events and their handlers inside a class
public static class EventReceiver
{
    private static List<Action> disposeEvents;
    private static readonly Destructor Finalise = new Destructor();

    static EventReceiver()
    {
        disposeEvents = new List<Action>();
    }

    private sealed class Destructor
    {
        ~Destructor()
        {
            DisposeEvents();
            UnityEngine.Debug.Log("Disposed of events");
        }
    }

    private static void DisposeEvents()
    {
        foreach (var dispose in disposeEvents)
        {
            dispose();
        }
        disposeEvents.Clear();
    }

    public static Action Subscribe<T>(string e, Action<T> action)
    {
        Action<dynamic> actionWrapper = (t) =>
        {
            if (t is T) action(t);
            else UnityEngine.Debug.LogWarning("Unhandled event " + e);
        };
        EventManager.Subscribe(e, actionWrapper);
        Action handle = () => EventManager.Unsubscribe(e, actionWrapper);
        disposeEvents.Add(handle);
        return handle;
    }

    public static Action Subscribe(string e, Action action)
    {
        Action<dynamic> actionWrapper = (t) => action();
        EventManager.Subscribe(e, actionWrapper);
        Action handle = () => EventManager.Unsubscribe(e, actionWrapper);
        disposeEvents.Add(handle);
        return handle;
    }
}