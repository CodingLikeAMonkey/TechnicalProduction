using Godot;
using System;

public partial class ENT_Player
{
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

        // --- Get Camera Relative Direction ---
        Vector3 inputVec = new Vector3(lastInputDirection.X, 0, lastInputDirection.Y);
        Vector3 direction = (Transform.Basis * inputVec).Normalized();
        Vector3 cameraRelativeDirection = (camera.Basis * direction).Normalized();

        GD.Print(cameraRelativeDirection);

        // --- Gravity ---
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // --- Ghost Rotation Calculation ---
        if (IsOnFloor() && (hasInput || wasRotatingFromTap))
        {
            float targetYaw = Mathf.Atan2(cameraRelativeDirection.X, cameraRelativeDirection.Z);

            // Interpolate ghost yaw
            GhostBodyYaw = Mathf.LerpAngle(GhostBodyYaw, targetYaw, Movement.TurnSpeed * (float)delta);

            // Store degrees and Quaternionernion versions
            GhostBodyYawDegrees = Mathf.RadToDeg(GhostBodyYaw);
            GhostBodyRotation = new Quaternion(Vector3.Up, GhostBodyYaw);

            // Finish tap rotation if close enough
            float angleDiff = Mathf.Abs(Mathf.PosMod(GhostBodyYaw - targetYaw + Mathf.Pi, Mathf.Tau) - Mathf.Pi);
            if (!hasInput && angleDiff < 0.05f)
            {
                wasRotatingFromTap = false;
            }
        }

        // --- Movement ---
        if (IsOnFloor())
        {
            if (hasInput && inputHoldTime > Movement.TapThreshold)
            {
                float currentSpeed = (inputDir.Length() < Movement.WalkThreshold) ? Movement.WalkSpeed : Movement.Speed;
                characterState = (inputDir.Length() < Movement.WalkThreshold) ? CharacterState.WalkState : CharacterState.RunState;

                velocity.X = velocity.MoveToward(cameraRelativeDirection * currentSpeed, Movement.Acceleration * (float)delta).X;
                velocity.Z = velocity.MoveToward(cameraRelativeDirection * currentSpeed, Movement.Acceleration * (float)delta).Z;
            }
            else
            {
                velocity.X = Mathf.MoveToward(velocity.X, 0, Movement.Friction * (float)delta);
                velocity.Z = Mathf.MoveToward(velocity.Z, 0, Movement.Friction * (float)delta);

                if (!hasInput)
                    characterState = CharacterState.IdleState;
            }
        }

        return velocity;
    }
}
