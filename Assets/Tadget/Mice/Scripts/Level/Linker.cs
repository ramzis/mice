using System;
using UnityEngine;
using static EventManager;
using static EventReceiver;
using static UnityEngine.Random;

// The composition root
public class Linker : MonoBehaviour
{
    public sealed class CoroutineLauncher : MonoBehaviour { }

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
        Subscribe(Events.DO_LEVEL_SETUP, SetupLevel);
        Subscribe(Events.DO_LEVEL_SETUP, () => Emit(Events.DO_TOGGLE_CANVAS, false));
        Subscribe(Events.DO_LEVEL_RESET, () => Emit(Events.DO_TOGGLE_CANVAS, false));
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
