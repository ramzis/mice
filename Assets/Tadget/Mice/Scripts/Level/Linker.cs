using System;
using UnityEngine;
using static EventManager;
using static UnityEngine.Random;

public class Linker : MonoBehaviour
{
    public class CoroutineLauncher : MonoBehaviour { }

    public UICanvas canvas;
    private Input input;
    private GameObject input_go;
    private CoroutineLauncher coroutineLauncher;

    private void Awake()
    {
        InitState(DateTime.Now.Millisecond);
        coroutineLauncher = new GameObject("Coroutines").AddComponent<CoroutineLauncher>();
        input_go = new GameObject("Input");
        input = input_go.AddComponent<Input>();
    }

    private void OnEnable()
    {
        Subscribe(Events.LEVEL_SETUP, SetupLevelEvent);
    }

    private void OnDisable()
    {
        Unsubscribe(Events.LEVEL_SETUP, SetupLevelEvent);
    }

    private void SetupLevel(int levelIdx)
    {
        Emit(Events.TOGGLE_CANVAS, false);

        var time = new Time();
        var objective = new Objective();

        input.Init(time, objective);

        var timer = new Timer()
            .Init(LevelTools.GetLevelTime(levelIdx), time, coroutineLauncher);

        canvas.timer = timer;

        var targets = new Targets(
            LevelTools.GetLevelTargetCount(levelIdx),
            UnityEngine.GameObject.FindObjectsOfType<Agent>().Length,
            0);

        var level = new Level(timer, targets, objective);
    }

    private void SetupLevelEvent(dynamic t)
    {
        if (t is int)
        {
            SetupLevel(t);
        }
    }
}
