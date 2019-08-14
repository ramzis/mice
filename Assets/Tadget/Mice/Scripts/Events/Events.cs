using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Events
{
    public static readonly string OBJECTIVE_FAILED = "OBJECTIVE_FAILED";
    public static readonly string OBJECTIVE_COMPLETED = "OBJECTIVE_COMPLETED";
    public static readonly string OBJECTIVE_INPROGRESS = "OBJECTIVE_INPROGRESS";
    public static readonly string TIME_OVER = "TIME_OVER";
    public static readonly string RUNNING = "RUNNING";
    public static readonly string PAUSED = "PAUSED";
    public static readonly string UNPAUSED = "UNPAUSED";
    public static readonly string AGENT_HIT = "AGENT_HIT";
    public static readonly string UPDATE_CANVAS = "UPDATE_CANVAS";
    public static readonly string TOGGLE_CANVAS = "TOGGLE_CANVAS";
    public static readonly string LEVEL_SETUP = "LEVEL_SETUP";
    public static readonly string LEVEL_BEGIN = "LEVEL_BEGIN";
}
