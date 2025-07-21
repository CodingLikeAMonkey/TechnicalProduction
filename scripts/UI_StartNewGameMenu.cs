using Godot;
using System;
[GlobalClass]
public partial class UI_StartNewGameMenu : VBoxContainer
{
    [Signal] public delegate void TransitionToMainMenuEventHandler();
    [Signal] public delegate void StartGameEventHandler(string saveFileName);
    Button BackBtn;
    Button StartBtn;
    LineEdit FileNameInput;

    public override void _Ready()
    {
        BackBtn = GetNode<Button>("%BTN_Back");
        StartBtn = GetNode<Button>("%BTN_StartGame");
        FileNameInput = GetNode<LineEdit>("%INP_SaveFileName");

        BackBtn.ButtonDown += OnBackBtnDown;
        StartBtn.ButtonDown += OnStartBtn;
    }

    private void OnBackBtnDown()
    {
        EmitSignal(SignalName.TransitionToMainMenu);
    }

    private void OnStartBtn()
    {
        EmitSignal(SignalName.StartGame, FileNameInput.Text);
    }

}
