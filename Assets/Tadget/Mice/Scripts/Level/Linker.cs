using System;
using UnityEngine;
using static UnityEngine.Random;

// The composition root
public class Linker : MonoBehaviour
{
    public sealed class CoroutineLauncher : MonoBehaviour { }

    public UICanvas canvas;
    private Input input;
    private CoroutineLauncher coroutineLauncher;

    private void Awake()
    {
        InitState(DateTime.Now.Millisecond);
        Create();
        Link();
    }

    private void Create()
    {
        coroutineLauncher = this.Create<CoroutineLauncher>("Coroutine");
        input = this.Create<Input>("Input");
    }

    private void Link()
    {
        var time = new Time();
        var objective = new Objective();

        input.Init(time, objective);

        var timer = new Timer(time, coroutineLauncher);

        canvas.timer = timer;

        var level = new Level(timer, objective);
    }
}
