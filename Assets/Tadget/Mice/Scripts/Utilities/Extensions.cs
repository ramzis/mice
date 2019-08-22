using UnityEngine;

public static class MonoBehaviourExtensions
{
    /// <summary>
    /// Creates a new GameObject and adds the T component.
    /// </summary>
    /// <param name="name">Optional name for the GameObject</param>
    /// <typeparam name="T">Component to add to new GameObject</typeparam>
    /// <returns>The added Component</returns>
    public static T Create<T>(this MonoBehaviour mono, string name = null)
        where T : Component
    {
        var go = new GameObject();
        go.name = name ?? go.GetHashCode().ToString();
        return go.AddComponent<T>();
    }
}
