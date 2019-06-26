using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class Linker : MonoBehaviour
{
    private Level level;
    private GameObject level_go;

    private InputManager inputManager;
    private GameObject inputManager_go;

    private UIManager uiManager;
    public GameObject uiManager_prefab;

    private TimeState time;
    private ObjectiveState objective;

    private CoroutineLauncher coroutineLauncher;

    private void Awake()
    {
        InitState(DateTime.Now.Millisecond);
        coroutineLauncher = new GameObject("Coroutines").AddComponent<CoroutineLauncher>();

        uiManager = Instantiate(uiManager_prefab).GetComponent<UIManager>();
        uiManager.ToggleCanvas(false);
        time = new TimeState();
        objective = new ObjectiveState();

        level_go = new GameObject("Level");
        level = level_go.AddComponent<Level>();
        level.objective = objective;

        level.agents = new LevelAgents<Mouse>();
        level.ui = uiManager;

        inputManager_go = new GameObject("Input");
        inputManager = inputManager_go.AddComponent<InputManager>().Init(time, objective);

        var timer = new Timer().Init(/*TODO GET REAL TIME OF LEVEL*/10f, time, coroutineLauncher);
        level.StartLevel(10, timer);
    }
}

public class CoroutineLauncher : MonoBehaviour { }
