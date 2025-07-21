using Godot;
using System;

public enum CharacterState
{
    IdleState,
    WalkState,
    RunState,
    JumpState,
    FallingState,
    LandingState,
    DodgeState,
}


[GlobalClass]
public partial class CharacterStats : Resource
{
    [Export] public float WalkThreshold = 0.7f;
    [Export] public float TapThreshold = 0.06f;
    [Export] public float Speed { get; set; } = 5.0f;
    [Export] public float WalkSpeed { get; set; } = 2.0f;
    [Export] public float Acceleration { get; set; } = 35.0f;
    [Export] public float Friction { get; set; } = 50.0f;
    [Export] public float TurnSpeed { get; set; } = 10.0f;
    [Export] public float JumpImpulse { get; set; } = 5.0f;
}
