using Godot;
using System;

public partial class ENT_Player : CharacterBody3D
{
    [Export] public CharacterStats Movement;
    [Export] private CTRL_PlayerHud HUD;
    public CTRL_Camera CameraController { get; set; }

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
            GD.PrintErr("No stats resource assigned!");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;
        velocity = MovementMotion(velocity, delta);
        Velocity = velocity;
        MoveAndSlide();

        if (CameraController != null)
        {
            // CameraController manages player rotation when assigned
            return;
        }

        Rotation = GhostBodyRotation.GetEuler();
    }

    private Vector3 MovementMotion(Vector3 velocity, double delta)
    {
        Vector2 inputDir = SYS_Input.MainInputDir;
        bool hasInput = inputDir.Length() > 0.1f;

        // --- Input Tracking ---
        if (hasInput)
        {
            inputHoldTime += (float)delta;
            lastInputDirection = inputDir;
            wasRotatingFromTap = true;
        }
        else
        {
            inputHoldTime = 0f;
        }

        // --- Get CameraController Relative Direction ---
        Vector3 inputVec = new Vector3(lastInputDirection.X, 0, lastInputDirection.Y);
        Vector3 direction = (Transform.Basis * inputVec).Normalized();

        Vector3 cameraRelativeDirection = Vector3.Zero;
        if (CameraController != null)
            cameraRelativeDirection = (CameraController.Transform.Basis * direction).Normalized();
        else
            cameraRelativeDirection = direction; // fallback if no camera controller assigned

        // --- Gravity ---
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // --- Ghost Rotation Calculation ---
        if (IsOnFloor() && (hasInput || wasRotatingFromTap))
        {
            float targetYaw = Mathf.Atan2(cameraRelativeDirection.X, cameraRelativeDirection.Z);

            GhostBodyYaw = Mathf.LerpAngle(GhostBodyYaw, targetYaw, Movement.TurnSpeed * (float)delta);
            GhostBodyYawDegrees = Mathf.RadToDeg(GhostBodyYaw);
            GhostBodyRotation = new Quaternion(Vector3.Up, GhostBodyYaw);

            float angleDiff = Mathf.Abs(Mathf.PosMod(GhostBodyYaw - targetYaw + Mathf.Pi, Mathf.Tau) - Mathf.Pi);
            if (!hasInput && angleDiff < 0.05f)
                wasRotatingFromTap = false;
        }

        // --- Movement ---
        if (IsOnFloor())
        {
            if (hasInput && inputHoldTime > Movement.TapThreshold)
            {
                float currentSpeed = (inputDir.Length() < Movement.WalkThreshold) ? Movement.WalkSpeed : Movement.Speed;
                velocity.X = Mathf.MoveToward(velocity.X, cameraRelativeDirection.X * currentSpeed, Movement.Acceleration * (float)delta);
                velocity.Z = Mathf.MoveToward(velocity.Z, cameraRelativeDirection.Z * currentSpeed, Movement.Acceleration * (float)delta);
            }
            else
            {
                velocity.X = Mathf.MoveToward(velocity.X, 0, Movement.Friction * (float)delta);
                velocity.Z = Mathf.MoveToward(velocity.Z, 0, Movement.Friction * (float)delta);
            }
        }

        return velocity;
    }
}
