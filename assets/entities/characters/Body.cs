using Godot;
using System;

public partial class Body : CollisionShape3D
{
    private ENT_Player Player;

    public override void _Ready()
    {
        Player = GetOwner<ENT_Player>();
        GD.Print(Player.GhostBodyYaw);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 eulerRadians = Player.GhostBodyRotation.GetEuler();

        // Convert radians to degrees correctly
        Vector3 eulerDegrees = new Vector3(
            Mathf.RadToDeg(eulerRadians.X),
            Mathf.RadToDeg(eulerRadians.Y),
            Mathf.RadToDeg(eulerRadians.Z)
        );

        RotationDegrees = eulerDegrees;
    }
}
