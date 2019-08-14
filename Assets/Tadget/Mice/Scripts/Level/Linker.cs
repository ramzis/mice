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

    private CoroutineLauncher coroutineLauncher;

    private void Awake()
    {
        InitState(DateTime.Now.Millisecond);
        coroutineLauncher = new GameObject("Coroutines").AddComponent<CoroutineLauncher>();

        Emit(Events.TOGGLE_CANVAS, false);

        var time = new Time();
        var objective = new Objective();

        inputManager_go = new GameObject("Input");
        input = inputManager_go.AddComponent<Input>().Init(time, objective);

        var timer = new Timer().Init(LevelTools.GetLevelTime(0), time, coroutineLauncher);

        var targets = new Targets(
            LevelTools.GetLevelTargetCount(0),
            UnityEngine.GameObject.FindObjectsOfType<Agent>().Length,
            0);

        level = new Level(timer, targets, objective);
    }
}

public class CoroutineLauncher : MonoBehaviour { }
