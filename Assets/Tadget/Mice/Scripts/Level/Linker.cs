using System;
using UnityEngine;
using static EventManager;
using static EventReceiver;
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
        Subscribe(Events.LEVEL_SETUP, SetupLevel);
        Subscribe(Events.LEVEL_SETUP, () => Emit(Events.TOGGLE_CANVAS, false));
        Subscribe(Events.LEVEL_RESET, () => Emit(Events.TOGGLE_CANVAS, false));
    }

    private void SetupLevel()
    {
        var time = new Time();
        var objective = new Objective();

        input.Init(time, objective);

        var timer = new Timer(time, coroutineLauncher);

        canvas.timer = timer;

        var level = new Level(timer, objective);
    }
}
