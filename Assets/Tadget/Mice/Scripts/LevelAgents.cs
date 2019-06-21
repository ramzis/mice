using static EventManager;

public class LevelAgents<T> where T : Agent
{
    private T[] agents;
    public int activeAgentCount { get; private set; }

    public LevelAgents()
    {
        this.agents = UnityEngine.GameObject.FindObjectsOfType<T>();
        this.activeAgentCount = agents.Length;
        LinkEvents();
    }

    public LevelAgents(T[] agents)
    {
        this.agents = agents;
        activeAgentCount = agents.Length;
        LinkEvents();
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

    public void Remove(UnityEngine.GameObject agent)
    {
        foreach (var target in agents)
        {
            if(agent == target.gameObject)
            {
                target.gameObject.SetActive(false);
                activeAgentCount -= 1;
            }
        }
    }

    public void LinkEvents()
    {
        foreach (var agent in agents)
        {
            agent.OnHit += (a, h) => Emit(Events.AGENT_HIT, (a, h));
        }
    }
}
