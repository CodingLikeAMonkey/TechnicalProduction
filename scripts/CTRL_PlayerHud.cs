using Godot;
using System;

[GlobalClass]
public partial class CTRL_PlayerHud : Control
{
    private UI_Menus Menus;
    private bool IsGameStarted = false;

    public override void _Ready()
    {
        if (SYS_GameMode.CurrentMode != GameMode.InGame)
        {
            SpawnMenus();
        }

        if (SYS_GameMode.CurrentMode == GameMode.InGame)
        {
            IsGameStarted = true;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }

    public override void _Process(double delta)
    {
        ToggleMenu();
    }

    private void SpawnMenus()
    {
        Menus = GetNodeOrNull<UI_Menus>("%UI_Menus");
        UI_Menus menus = (UI_Menus)ResourceLoader.Load<PackedScene>("res://ui/UI_menus.tscn").Instantiate();
        Menus = menus;
        AddChild(Menus);
        Menus.GameStarted += OnGameStarted;
        Menus.ReturnToTitleScreen += OnReturnToTitleScreen;
        Menus.ResumeGame += OnResumeGame;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    private void ToggleMenu()
    {
        if (SYS_Input.Escape && SYS_GameMode.CurrentMode == GameMode.InGame && IsGameStarted == true)
        {
            SYS_GameMode.CurrentMode = GameMode.PauseMenu;
            SpawnMenus();
        }
        else if (SYS_Input.Escape && SYS_GameMode.CurrentMode != GameMode.InGame && IsGameStarted == true)
        {
            CloseMenus();
        }
    }

    private void CloseMenus()
    {
        SYS_GameMode.CurrentMode = GameMode.InGame;
        Menus.QueueFree();
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    private void OnGameStarted()
    {
        SYS_GameMode.CurrentMode = GameMode.InGame;
        GetTree().ChangeSceneToFile("res://Level/LVL_Main.tscn");
    }

    private void OnReturnToTitleScreen()
    {
        SYS_GameMode.CurrentMode = GameMode.MainMenu;
        GetTree().ChangeSceneToFile("res://Level/LVL_start_screen.tscn");
    }

    private void OnResumeGame()
    {
        CloseMenus();
    }
}
