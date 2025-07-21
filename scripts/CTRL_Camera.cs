using Godot;
using System;

public partial class CTRL_Camera : Node3D
{
    private float MinPitch { get; set; } = 35.0f;
    private float MaxPitch { get; set; } = -90.0f;
    private Vector3 rotDeg;
    private Vector2 lookvector;
    private float HorizontalMouseSensitivity { get; set; } = 0.2f; // 1.0
    private float VerticalMouseSensitivity { get; set; } = 0.2f; // 1.0
    private float HorizontalControllerSensitivity { get; set; } = 2.0f; //2.0
    private float VerticalControllerSensitivity { get; set; } = 2.0f; // 2.0

    //private Camera3D cam;
    private SpringArm3D springArm;

    public override void _Ready()
    {
        //cam = GetNode<Camera3D>("%Camera3D");
        springArm = GetNode<SpringArm3D>("%SpringArm3D");
    }

    public override void _Process(double delta)
    {
        if (Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            // Mouse Support
            rotDeg = RotationDegrees;
            rotDeg.Y -= SYS_Input.MouseRelative.X * HorizontalMouseSensitivity;
            RotationDegrees = rotDeg;

            rotDeg = springArm.RotationDegrees;
            rotDeg.X -= SYS_Input.MouseRelative.Y * VerticalMouseSensitivity;
            rotDeg.X = Mathf.Clamp(rotDeg.X, MaxPitch, MinPitch);
            springArm.RotationDegrees = rotDeg;
        }

        // Controller Support
        lookvector = SYS_Input.RightStickInputDir;
        RotateY(Mathf.DegToRad((-lookvector.X) * HorizontalControllerSensitivity));

        rotDeg = springArm.RotationDegrees;
        rotDeg.X -= lookvector.Y * VerticalControllerSensitivity;
        rotDeg.X = Mathf.Clamp(rotDeg.X, MaxPitch, MinPitch);
        springArm.RotationDegrees = rotDeg;
    }
}
