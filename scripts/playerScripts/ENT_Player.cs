using Godot;
using System;

public partial class ENT_Player : CharacterBody3D
{
    [Export] public CharacterStats Movement;
    [Export] private CTRL_PlayerHud HUD;

    private CharacterState characterState;
    private Node3D camera;

    // --- Ghost Rotation (Public) ---
    public float GhostBodyYaw { get; private set; } = 0f;             // Radians
    public float GhostBodyYawDegrees { get; private set; } = 0f;      // Degrees (optional)
    public Quaternion GhostBodyRotation { get; private set; } = Quaternion.Identity;

    // --- Input Tracking ---
    private float inputHoldTime = 0f;
    private Vector2 lastInputDirection = Vector2.Zero;
    private bool wasRotatingFromTap = false;

    public override void _Ready()
    {
        if (Movement == null)
        {
            GD.PrintErr("No stats resource assigned!");
        }

        camera = GetNode<Node3D>("%ThirdPersonCamera");
    }

    public override void _Input(InputEvent @event)
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        velocity = MovementMotion(velocity, delta);

        Velocity = velocity;
        MoveAndSlide();
    }

    private void OnMainMenuOpen(InputEvent @event)
    {
        GD.Print("The main menu is open");
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    private void OnMainMenuClosed(InputEvent @event)
    {
        GD.Print("Menu closed");
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
}
