using System;
using System.Collections.Generic;

public static class EventManager
{
    private static Dictionary<string, List<Action<dynamic>>> dc;
    static EventManager()
    {
        dc = new Dictionary<string, List<Action<dynamic>>>();
    }

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
}
