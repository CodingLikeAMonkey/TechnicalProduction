using Godot;
using System;

[GlobalClass]
public partial class COMP_CharacterLastPosition : Marker3D
{
    [Export] CharacterBody3D Character { get; set; }

    public override void _Ready()
    {
       if (Character != null)
        {
            GD.Print("yeeea");
            Timer timer = new Timer();
            timer.WaitTime = 1.0f;
            timer.OneShot = false;
            timer.Autostart = true;
            timer.Timeout += OnTimerTimeout;
            AddChild(timer);
        }

    }

    private void OnTimerTimeout()
    {
        if (Character.IsOnFloor() && Character.Velocity != Vector3.Zero)
        {
            GlobalPosition = Character.Position;
            GD.Print("move the marker");
        }
    }
}
