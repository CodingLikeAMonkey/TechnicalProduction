using Godot;
using System;

public enum GameMode
{
    // menus
    MainMenu,
    NewGameMenu,
    InGame,
    PauseMenu,
    SaveFiles
}
public partial class SYS_GameMode : Node
{
    public static GameMode CurrentMode { get; set; } = GameMode.MainMenu;
}
