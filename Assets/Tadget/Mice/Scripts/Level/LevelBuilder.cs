using static EventReceiver;

public class LevelBuilder
{
    private Level level;
    private bool isLevelSetup;

    public LevelBuilder(Level level)
    {
        this.level = level;
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
    }
}