public sealed class Events
{
    // Action responders
    public static readonly string ON_OBJECTIVE_NULL = "ON_OBJECTIVE_NULL";
    public static readonly string ON_OBJECTIVE_FAILED = "ON_OBJECTIVE_FAILED";
    public static readonly string ON_OBJECTIVE_COMPLETED = "ON_OBJECTIVE_COMPLETED";
    public static readonly string ON_OBJECTIVE_INPROGRESS = "ON_OBJECTIVE_INPROGRESS";
    public static readonly string ON_TIME_STOPPED = "ON_TIME_STOPPED";
    public static readonly string ON_RUNNING = "ON_RUNNING";
    public static readonly string ON_PAUSED = "ON_PAUSED";
    public static readonly string ON_UNPAUSED = "ON_UNPAUSED";
    public static readonly string ON_AGENT_HIT = "ON_AGENT_HIT";
    // Action creators
    public static readonly string DO_UPDATE_CANVAS = "DO_UPDATE_CANVAS";
    public static readonly string DO_TOGGLE_CANVAS = "DO_TOGGLE_CANVAS";
    public static readonly string DO_LEVEL_SETUP = "DO_LEVEL_SETUP";
    public static readonly string DO_LEVEL_BEGIN = "DO_LEVEL_BEGIN";
    public static readonly string DO_LEVEL_RESET = "DO_LEVEL_RESET";
}
