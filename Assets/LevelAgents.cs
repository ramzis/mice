public class LevelAgents<T> where T : Agent
{
    public T[] agents;
    public int activeAgentCount;

    public LevelAgents()
    {
        this.agents = UnityEngine.GameObject.FindObjectsOfType<T>();
        this.activeAgentCount = agents.Length;
    }

    public LevelAgents(T[] agents)
    {
        this.agents = agents;
        activeAgentCount = agents.Length;
    }

    public void Tick()
    {
        foreach (T agent in agents)
        {
            agent.UpdateState();
        }
        foreach (T agent in agents)
        {
            agent.Act();
        }
    }
}
