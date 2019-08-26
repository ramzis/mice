using static EventReceiver;

public class LevelBuilder
{
    private Level level;
    private bool isSpawnedObjects;
    private Spawner spawner;
    private int levelId;

    public LevelBuilder(Level level)
    {
        this.level = level;
        this.spawner = new Spawner();
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        Subscribe<int>(Events.DO_LEVEL_SETUP, Build);
        Subscribe(Events.DO_LEVEL_BEGIN, Begin);
        Subscribe(Events.DO_LEVEL_RESET, Reset);
    }

    public void Build(int id)
    {
        spawner.SetLevelId(id);
        spawner.Spawn();
        var targets = new Targets(
            LevelTools.GetLevelTargetCount(id),
            UnityEngine.GameObject.FindObjectsOfType<Agent>().Length,
            0
        );
        var p = new Parameters(
            LevelTools.GetLevelTime(id),
            targets);
        level.Setup(p);
    }

    public void Begin()
    {
        level.Begin();
    }

    public void Reset()
    {
        level.Reset();
        spawner.Despawn();
        spawner.Spawn();
    }
}