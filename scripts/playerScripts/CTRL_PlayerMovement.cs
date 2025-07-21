using Godot;
using System;

public partial class ENT_Player
{
    private float inputHoldTime = 0f;
    private Vector2 lastInputDirection = Vector2.Zero;
    private bool wasRotatingFromTap = false;
    private Vector3 Gravity(Vector3 velocity, double delta)
    {
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }
        return velocity;
    }

    private Vector3 Jump(Vector3 velocity, double delta)
    {
        if (SYS_Input.Jump && IsOnFloor())
        {
            velocity.Y = Movement.JumpImpulse;
        }
        return velocity;
    }

    static private float WrapAngle(float angle)
    {
        angle = Mathf.PosMod(angle + Mathf.Pi, Mathf.Tau);
        return angle - Mathf.Pi;
    }

    private void UpdateInputTracking(Vector2 inputDir, double delta)
    {
        if (inputDir.Length() > 0.1f)
        {
            inputHoldTime += (float)delta;
            lastInputDirection = inputDir;
            wasRotatingFromTap = true;
        }
        else
        {
            inputHoldTime = 0f;
        }
    }

    private Vector3 GetCameraRelativeDirection()
    {
        Vector3 inputVec = new Vector3(lastInputDirection.X, 0, lastInputDirection.Y);
        Vector3 direction = (Transform.Basis * inputVec).Normalized();
        return (camera.Basis * direction).Normalized();
    }

    private bool ShouldRotate(bool hasInput)
    {
        return IsOnFloor() && (hasInput || wasRotatingFromTap);
    }

    private void ApplyRotation(Vector3 cameraRelativeDirection, double delta, bool hasInput)
    {
        float targetYaw = Mathf.Atan2(cameraRelativeDirection.X, cameraRelativeDirection.Z);
        Vector3 rotDegrees = colBody.Rotation;
        rotDegrees.Y = Mathf.LerpAngle(rotDegrees.Y, targetYaw, Movement.TurnSpeed * (float)delta);
        colBody.Rotation = rotDegrees;

        float angleDiff = Mathf.Abs(WrapAngle(colBody.Rotation.Y - targetYaw));
        if (!hasInput && angleDiff < 0.05f)
        {
            wasRotatingFromTap = false;
        }
    }

    private Vector3 ApplyMovement(Vector3 velocity, Vector3 direction, Vector2 inputDir, double delta, bool hasInput)
    {
        if (hasInput && inputHoldTime > Movement.TapThreshold)
        {
            float currentSpeed;
            if (inputDir.Length() < Movement.WalkThreshold)
            {
                currentSpeed = Movement.WalkSpeed;
                characterState = CharacterState.WalkState;
            }
            else
            {
                currentSpeed = Movement.Speed;
                characterState = CharacterState.RunState;
            }

            velocity.X = velocity.MoveToward(direction * currentSpeed, Movement.Acceleration * (float)delta).X;
            velocity.Z = velocity.MoveToward(direction * currentSpeed, Movement.Acceleration * (float)delta).Z;
        }
        else
        {
            velocity.X = Mathf.MoveToward(velocity.X, 0, Movement.Friction * (float)delta);
            velocity.Z = Mathf.MoveToward(velocity.Z, 0, Movement.Friction * (float)delta);

            if (!hasInput)
                characterState = CharacterState.IdleState;
        }

        return velocity;
    }

    private Vector3 MovementMotion(Vector3 velocity, double delta)
    {
        Vector2 inputDir = SYS_Input.MainInputDir;
        bool hasInput = inputDir.Length() > 0.1f;

        UpdateInputTracking(inputDir, delta);
        Vector3 cameraRelativeDirection = GetCameraRelativeDirection();
        GD.Print(cameraRelativeDirection);

        if (ShouldRotate(hasInput))
            ApplyRotation(cameraRelativeDirection, delta, hasInput);

        if (IsOnFloor())
            velocity = ApplyMovement(velocity, cameraRelativeDirection, inputDir, delta, hasInput);

        return velocity;
    }
}
