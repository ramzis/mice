using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;
using static UnityEngine.Random;

public class Linker : MonoBehaviour
{
    private Level level;
    private GameObject level_go;

    private Input input;
    private GameObject inputManager_go;

    private Canvas levelCanvas;
    public GameObject uiManager_prefab;

    private Time time;
    private Objective objective;

    private CoroutineLauncher coroutineLauncher;

    private void Awake()
    {
        InitState(DateTime.Now.Millisecond);
        coroutineLauncher = new GameObject("Coroutines").AddComponent<CoroutineLauncher>();

        levelCanvas = Instantiate(uiManager_prefab).GetComponent<Canvas>();
        Emit(Events.TOGGLE_CANVAS, false);
        time = new Time();
        objective = new Objective();

        level_go = new GameObject("Level");
        level = level_go.AddComponent<Level>();

        inputManager_go = new GameObject("Input");
        input = inputManager_go.AddComponent<Input>().Init(time, objective);

        var timer = new Timer().Init(LevelTools.GetLevelTime(0), time, coroutineLauncher);

        var targets = new Targets(
            LevelTools.GetLevelTargetCount(0),
            UnityEngine.GameObject.FindObjectsOfType<Agent>().Length,
            0);

        level.Init(timer, targets, objective);
    }
}

public class CoroutineLauncher : MonoBehaviour { }
