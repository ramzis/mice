using System;
using System.Collections.Generic;

public class SharedUtilityContainer
{
    private static Dictionary<Type, object> sharedUtilities;

    public SharedUtilityContainer()
    {
        if (sharedUtilities == null)
            sharedUtilities = new Dictionary<Type, object>();
    }

    public void SetSharedUtilty<T>(T obj)
    {
        var t = obj.GetType();
        if (sharedUtilities.TryGetValue(t, out _))
        {
            sharedUtilities[t] = obj;
        }
        else
        {
            sharedUtilities.Add(t, obj);
        }
    }

    public T GetSharedUtility<T>() where T : new()
    {
        if (!sharedUtilities.ContainsKey(typeof(T)))
        {
            sharedUtilities.Add(typeof(T), new T());
        }
        if (sharedUtilities.TryGetValue(typeof(T), out var @object))
        {
            return (T)@object;
        }
        else
        {
            return default(T);
        }
    }
}
