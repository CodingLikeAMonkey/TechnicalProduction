using Godot;
using System;

[GlobalClass]
public partial class UI_SaveFiles : ScrollContainer
{
    VBoxContainer SaveFileContainer;
    public override void _Ready()
    {
        SaveFileContainer = GetNode<VBoxContainer>("%VB_SaveFileContainer");

        foreach (Node child in SaveFileContainer.GetChildren())
        {
            if (child is Button btn)
            {
                btn.ButtonDown += () => OnSaveFileBtnDown(btn);
            }
        }
    }

    private void OnSaveFileBtnDown(Button btn)
    {
        GD.Print(btn.Name);
    }
}
