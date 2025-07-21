using Godot;
using System;

[GlobalClass]
public partial class UI_MainMenu : VBoxContainer
{
    [Signal] public delegate void TransitionToNewGameWindowEventHandler();
    [Signal] public delegate void TransitionToSaveFilesEventHandler();

    Button ContinueBtn;
    Button NewGameBtn;
    Button LoadGameBtn;
    Button SettingsBtn;

    public override void _Ready()
    {
        NewGameBtn = GetNodeOrNull<Button>("%BTN_NewGame");
        LoadGameBtn = GetNode<Button>("%BTN_LoadGame");
        NewGameBtn.ButtonDown += OnNewGameBtnDown;
        LoadGameBtn.ButtonDown += OnLoadGameBtnDown;
    }

    private void OnNewGameBtnDown()
    {
        EmitSignal(SignalName.TransitionToNewGameWindow);
    }

    private void OnLoadGameBtnDown()
    {
        EmitSignal(SignalName.TransitionToSaveFiles);
    }
}
