using Godot;

[GlobalClass]
public partial class COMP_MouseCapture : Node
{
    private bool isMouseCaptured = false;

    public override void _UnhandledInput(InputEvent @event)
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;

    }
}
