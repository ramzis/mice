using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class Input : MonoBehaviour
{
    private Time time;
    private Objective objective;

    public Input Init(Time time, Objective objective)
    {
        this.time = time;
        this.objective = objective;
        return this;
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.X))
        {
            Emit(Events.LEVEL_BEGIN);
        }
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
