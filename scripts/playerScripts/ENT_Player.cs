using Godot;
using System;

public partial class ENT_Player : CharacterBody3D
{
    [Export] public CharacterStats Movement;
    [Export] private CTRL_PlayerHud HUD;


    private CharacterState characterState;
    private Node3D camera;
    private CollisionShape3D colBody;

    // CTRL_PleayerMenu


    public override void _Ready()
    {
        if (Movement == null)
        {
            GD.PrintErr("No stats resource asigned!");
        }

        //HUD.MainMenuOpen += OnMainMenuOpen;
        //HUD.MainMenuClosed += OnMainMenuClosed;

        camera = GetNode<Node3D>("%ThirdPersonCamera");
        colBody = GetNode<CollisionShape3D>("%Body");
    }

    public override void _Input(InputEvent @event)
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        velocity = Gravity(velocity, delta);
        velocity = Jump(velocity, delta);
        velocity = MovementMotion(velocity, delta);

        Velocity = velocity;
        MoveAndSlide();
    }

    private void OnMainMenuOpen(InputEvent @event)
    {
        GD.Print("The main menuuuuuuuuu is ooooopen");
        Input.MouseMode = Input.MouseModeEnum.Visible;

    }

    private void OnMainMenuClosed(InputEvent @event)
    {
        GD.Print("Menu cloooosed");
        Input.MouseMode = Input.MouseModeEnum.Captured;

    }
}
