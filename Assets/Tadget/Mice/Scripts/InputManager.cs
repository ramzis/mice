using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private TimeState time;
    private ObjectiveState objective;

    public InputManager Init(TimeState time, ObjectiveState objective)
    {
        this.time = time;
        this.objective = objective;
        return this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && objective.state == ObjectiveState.State.INPROGRESS)
        {
            switch (time.state)
            {
                case TimeState.State.PAUSED:
                    time.Resume();
                    break;
                case TimeState.State.RUNNING:
                    time.Pause();
                    break;
                case TimeState.State.OVER:
                    break;
                default:
                    break;
            }
        }
    }
}
