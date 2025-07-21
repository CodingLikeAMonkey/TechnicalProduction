using Godot;
using System;

public partial class TestAnimPlayer : AnimationPlayer
{
    public override void _Ready()
    {
        const string animName = "humanoid_animation_library/run";

        // Set the animation to loop via code
        if (HasAnimation(animName))
        {
            Animation animation = GetAnimation(animName);
            if (animation != null)
            {
            }

            Play(animName);
        }
        else
        {
            GD.PrintErr($"Animation '{animName}' not found.");
        }
    }
}
