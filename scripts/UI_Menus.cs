using Godot;
using System;

[GlobalClass]
public partial class UI_Menus : Control
{
    [Signal] public delegate void GameStartedEventHandler();
    [Signal] public delegate void ReturnToTitleScreenEventHandler();
    [Signal] public delegate void ResumeGameEventHandler();


    Button NewGame = null;
    VBoxContainer StartNewGameScreen;
    VBoxContainer MainMenuScreen;
    Button BtnBack;
    Button BtnStartGame;
    LineEdit SaveFileInput;
    Button NewGameBtn;
    CenterContainer MenuContainer;
    UI_StartNewGameMenu NewGameMenuScene;
    UI_MainMenu MainMenuScene;
    UI_PauseMenue PauseMenuScene;

    UI_Menus Menus;

    UI_SaveFiles SaveFilesScene;

    public override void _Ready()
    {
        MenuContainer = GetNode<CenterContainer>("%CC_MenuContainer");
        if (SYS_GameMode.CurrentMode == GameMode.MainMenu)
        {
            SetupMainMenu();
        }
        else if (SYS_GameMode.CurrentMode == GameMode.PauseMenu)
        {
            UI_PauseMenue pauseMenu = (UI_PauseMenue)ResourceLoader.Load<PackedScene>("res://ui/UI_pause_menue.tscn").Instantiate();
            PauseMenuScene = pauseMenu;
            MenuContainer.AddChild(PauseMenuScene);

            PauseMenuScene.TitleScreenBtn.ButtonDown += OnPauseMenuTitleBtnDown;
            PauseMenuScene.ResumeBtn.ButtonDown += OnPauseMenuResumeBtnDown;
        }
    }
    private void SetupMainMenu()
    {
        SYS_GameMode.CurrentMode = GameMode.MainMenu;
        UI_MainMenu mainMenu = (UI_MainMenu)ResourceLoader.Load<PackedScene>("res://ui/UI_main_menu.tscn").Instantiate();
        MainMenuScene = mainMenu;
        MenuContainer.AddChild(MainMenuScene);
        MainMenuScene.TransitionToNewGameWindow += OnTransitionToNewGameWindow;
        MainMenuScene.TransitionToSaveFiles += OnTransitionToSaveFiles;
    }
    private void OnTransitionToNewGameWindow()
    {
        SYS_GameMode.CurrentMode = GameMode.NewGameMenu;
        MainMenuScene.QueueFree();
        UI_StartNewGameMenu newGameMenu = (UI_StartNewGameMenu)ResourceLoader.Load<PackedScene>("res://ui/UI_start_new_game_menu.tscn").Instantiate();
        NewGameMenuScene = newGameMenu;
        MenuContainer.AddChild(NewGameMenuScene);
        NewGameMenuScene.TransitionToMainMenu += OnTransitionToMainMenu;
        NewGameMenuScene.StartGame += OnStartGame;
    }

    private void OnTransitionToMainMenu()
    {
        NewGameMenuScene.QueueFree();
        SetupMainMenu();
    }

    private void OnStartGame(string fileName)
    {
        SYS_GameMode.CurrentMode = GameMode.InGame;
        NewGameMenuScene.QueueFree();
        GD.Print(fileName);
        EmitSignal(SignalName.GameStarted);
    }

    private void OnPauseMenuResumeBtnDown()
    {
        EmitSignal(SignalName.ResumeGame);
    }

    private void OnPauseMenuTitleBtnDown()
    {
        EmitSignal(SignalName.ReturnToTitleScreen);
    }

    private void OnTransitionToSaveFiles()
    {
        GD.Print("you go gurl");
        SYS_GameMode.CurrentMode = GameMode.SaveFiles;
        MainMenuScene.QueueFree();
        UI_SaveFiles saveFiles = (UI_SaveFiles)ResourceLoader.Load<PackedScene>("res://ui/UI_save_files.tscn").Instantiate();
        SaveFilesScene = saveFiles;
        MenuContainer.AddChild(SaveFilesScene);  
    }

}
