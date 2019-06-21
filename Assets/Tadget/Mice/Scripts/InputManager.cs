using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelState;

public class InputManager : MonoBehaviour
{
    private LevelState state;

    public void Init(LevelState state)
    {
        this.state = state;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && state.objectiveState == ObjectiveState.INPROGRESS)
        {
            switch (state.timeState)
            {
                case TimeState.PAUSED:
                    state.Resume();
                    break;
                case TimeState.RUNNING:
                    state.Pause();
                    break;
                case TimeState.OVER:
                    break;
                default:
                    break;
            }
        }
    }
}
