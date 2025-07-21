using Godot;
using System;
[GlobalClass]
public partial class UI_PauseMenue : VBoxContainer
{
    public Button ResumeBtn;
    public Button SettingsBtn;
    public Button TitleScreenBtn;
    Button QuitGameBtn;

    public override void _Ready()
    {
        ResumeBtn = GetNode<Button>("%BTN_Resume");
        TitleScreenBtn = GetNode<Button>("%BTN_TitleScreen");
        QuitGameBtn = GetNode<Button>("%BTN_QuitGame");

        QuitGameBtn.ButtonDown += OnQuitGameBtnDown;
    }

    private void OnQuitGameBtnDown()
    {
        GetTree().Quit();
    }
}
