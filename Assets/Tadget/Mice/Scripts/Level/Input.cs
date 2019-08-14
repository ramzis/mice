using UnityEngine;
using static EventManager;

public class Input : MonoBehaviour
{
    private Time time;
    private Objective objective;
    private bool isInit;

    public Input Init(Time time, Objective objective)
    {
        this.time = time;
        this.objective = objective;
        isInit = true;
        return this;
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.X))
        {
            Emit(Events.LEVEL_BEGIN);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
        {
            Emit(Events.LEVEL_SETUP, 0);
        }
        if (!isInit) return;
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && objective.state == Objective.State.INPROGRESS)
        {
            switch (time.state)
            {
                case Time.State.PAUSED:
                    time.Resume();
                    break;
                case Time.State.RUNNING:
                    time.Pause();
                    break;
                case Time.State.OVER:
                    break;
                default:
                    break;
            }
        }
    }
}
