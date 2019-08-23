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
        if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
        {
            Emit(Events.DO_LEVEL_SETUP, 0);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.X))
        {
            Emit(Events.DO_LEVEL_BEGIN);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.C))
        {
            Emit(Events.DO_LEVEL_RESET);
        }
        if (!isInit) return;
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && objective.state == Objective.State.INPROGRESS)
        {
            switch (time.state)
            {
                case Time.State.PAUSED:
                    time.Play();
                    break;
                case Time.State.PLAYING:
                    time.Pause();
                    break;
                case Time.State.STOPPED:
                    break;
                default:
                    break;
            }
        }
    }
}
