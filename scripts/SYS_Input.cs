using Godot;

public partial class SYS_Input : Node
{
    // Action names
    private string up = "up";
    private string down = "down";
    private string left = "left";
    private string right = "right";

    private string rightStickUp = "right_stick_up";
    private string rightStickDown = "right_stick_down";
    private string rightStickLeft = "right_stick_left";
    private string rightStickRight = "right_stick_right";

    private string accept = "accept";
    private string cancel = "cancel";
    private string escape = "escape";
    private string interact = "interact";
    private string leftClick = "left_click";
    private string rightClick = "right_click";
    private string jump = "jump";

    // Mouse input (updated per frame)
    public static Vector2 MousePosition { get; private set; } = Vector2.Zero;
    public static Vector2 MouseRelative { get; private set; } = Vector2.Zero;

    // Internal: accumulate mouse delta between frames
    private Vector2 _frameMouseDelta = Vector2.Zero;

    // Movement and input directions
    public static Vector2 MainInputDir { get; private set; } = Vector2.Zero;
    public static Vector2 RightStickInputDir { get; private set; } = Vector2.Zero;

    // Buttons
    public static bool Accept { get; private set; } = false;
    public static bool Cancel { get; private set; } = false;
    public static bool Escape { get; private set; } = false;
    public static bool Interact { get; private set; } = false;
    public static bool LeftClick { get; private set; } = false;
    public static bool RightClick { get; private set; } = false;
    public static bool Jump { get; private set; } = false;

    public override void _Process(double delta)
    {
        // Finalize mouse delta for this frame and reset accumulator
        MouseRelative = _frameMouseDelta;
        _frameMouseDelta = Vector2.Zero;

        // Capture movement directions
        MainInputDir = Input.GetVector(left, right, up, down);
        RightStickInputDir = Input.GetVector(rightStickLeft, rightStickRight, rightStickUp, rightStickDown);

        // Capture button inputs
        Accept = Input.IsActionJustPressed(accept);
        Cancel = Input.IsActionJustPressed(cancel);
        Escape = Input.IsActionJustPressed(escape);

        Interact = Input.IsActionJustPressed(interact);

        LeftClick = Input.IsActionJustPressed(leftClick);
        RightClick = Input.IsActionJustPressed(rightClick);
        Jump = Input.IsActionJustPressed(jump);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motionEvent)
        {
            MousePosition = motionEvent.Position;
            _frameMouseDelta += motionEvent.Relative;
        }
    }
}
