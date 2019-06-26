using System;
using System.Collections.Generic;

public class EventManager
{
    private static EventManager eventManagerInstance = null;
    public static EventManager instance
    {
        get
        {
            if (eventManagerInstance == null)
            {
                eventManagerInstance = new EventManager();
            }

            return eventManagerInstance;
        }
    }

    private static Dictionary<string, List<Action<dynamic>>> dc = 
        new Dictionary<string, List<Action<dynamic>>>();

    public static void Subscribe(string eventName, Action<dynamic> action)
    {
        if (dc.TryGetValue(eventName, out List<Action<dynamic>> fs))
        {
            fs.Add(action);
        }
        else
        {
            dc.Add(eventName, new List<Action<dynamic>>() { action });
        }
    }

    public static void Unsubscribe(string eventName, Action<dynamic> action)
    {
        if (dc.TryGetValue(eventName, out List<Action<dynamic>> fs))
        {
            fs.RemoveAll(a => a == action);
        }
    }

    public static void Emit(string eventName, dynamic payload = null)
    {
        if (dc.TryGetValue(eventName, out List<Action<dynamic>> fs))
        {
            foreach (var f in fs)
            {
                f?.Invoke(payload);
            }
        }
    }

    public static void Purge()
    {
        dc?.Clear();
    }
}
