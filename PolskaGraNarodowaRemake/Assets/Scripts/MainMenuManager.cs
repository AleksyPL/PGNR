using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public enum GameMode
{
    SinglePlayerClassic,
    SinglePlayerEndless,
    VersusClassic,
    VersusEndless
}

public class MainMenuManager : MonoBehaviour
{
    internal AudioManager audioScript;
    public GameObject audioManagerGameObject;
    internal PlaneSkinSelector planeSkinSelectorScript;
    public static GameMode currentGameMode;
    internal static bool fromMainMenu = false;
    public GameplaySettings gameplaySettings;
    //menu main buttons
    public GameObject menuButtonsMainGameObject;
    public GameObject playGameButton;
    public GameObject plotButton;
    public GameObject howToPlayPanelMenuButton;
    public GameObject exitGameMenuButton;
    public GameObject optionsMenuButton;
    //options
    public GameObject optionsMenuGameObject;
    //how to play menu
    public GameObject howToPlayPanelGameObject;
    public GameObject howToPlayPanelTitleGameObject;
    public GameObject howToPlayPanelControlsPlayerOneGameObject;
    public GameObject howToPlayPanelControlsPlayerTwoGameObject;
    public GameObject howToPlayPanelPlayerOneIndicatorGameObject;
    public GameObject howToPlayPanelPlayerTwoIndicatorGameObject;
    public GameObject howToPlayPanelBackToMainMenuButton;
    //plot
    public GameObject plotPanelGameObject;
    public GameObject plotPanelTitleGameObject;
    public GameObject plotPanelStoryGameObject;
    public GameObject plotPanelBackToMainMenuButton;
    //skin selection menu
    public GameObject skinSelectorMenuGameObject;
    //game mode selection menu
    public GameObject startSinglePlayerClassicModeMenuButton;
    public GameObject startMultiPlayerClassicModeMenuButton;
    public GameObject startSinglePlayerEndlessModeMenuButton;
    public GameObject startMultiPlayerEndlessModeMenuButton;
    public GameObject gameModeSelectorMenuGameObject;
    public GameObject gameModeSelectionMenuTitleGameObject;
    public GameObject gameModeSelectionMenuBackToMainMenuButton;
    void Start()
    {
        Application.targetFrameRate = 144;
#if UNITY_EDITOR
        gameplaySettings.safeMode = false;
#else
        GetArguments();
#endif
        currentGameMode = GameMode.SinglePlayerClassic;
        planeSkinSelectorScript = GetComponent<PlaneSkinSelector>();
        audioScript = audioManagerGameObject.GetComponent<AudioManager>();
        audioScript.PlaySound("MainMenuTheme", audioScript.otherSounds);
        menuButtonsMainGameObject.SetActive(true);
        UpdateUIButtonsWithLocalization();
    }
    private void UpdateUIButtonsWithLocalization()
    {
        playGameButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuStartGame;
        howToPlayPanelMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton1;
        optionsMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton2;
        exitGameMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton3;
        plotButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButtonPlot;
        gameModeSelectionMenuBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenuButton;
        gameModeSelectionMenuTitleGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuTitle;
        startSinglePlayerClassicModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuButton0;
        startSinglePlayerEndlessModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuButton1;
        startMultiPlayerClassicModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuButton2;
        startMultiPlayerEndlessModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuButton3;
        howToPlayPanelTitleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton1;
        plotPanelTitleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButtonPlot;
        if(gameplaySettings.safeMode)
            plotPanelStoryGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].plotPlotAlt;
        else
            plotPanelStoryGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].plotPlot;
        howToPlayPanelControlsPlayerOneGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].howtoPlayControlsPlayerOne;
        howToPlayPanelControlsPlayerTwoGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].howtoPlayControlsPlayerTwo;
        howToPlayPanelPlayerOneIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
        howToPlayPanelPlayerTwoIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
        howToPlayPanelBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenuButton;
        plotPanelBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenuButton;
    }
    private void Update()
    {
        if (optionsMenuGameObject.activeSelf && Input.GetButtonDown("Cancel"))
            DisableOptionsMenu();
        else if (howToPlayPanelGameObject.activeSelf && Input.GetButtonDown("Cancel"))
            DisableHowToPlayPanel();
        else if (skinSelectorMenuGameObject.activeSelf && Input.GetButtonDown("Cancel"))
            DisableSkinSelectorMenu();
        else if (gameModeSelectorMenuGameObject.activeSelf && Input.GetButtonDown("Cancel"))
            DisableGameModeSelectorMenu();
    }
    public void StartGameSinglePlayer()
    {
        SceneManager.LoadScene("SinglePlayer");
    }
    public void StartGameMultiPlayer()
    {
        SceneManager.LoadScene("MultiPlayer");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void EnableOptionsMenu()
    {
        menuButtonsMainGameObject.SetActive(false);
        optionsMenuGameObject.SetActive(true);
    }
    public void DisableOptionsMenu()
    {
        menuButtonsMainGameObject.SetActive(true);
        optionsMenuGameObject.SetActive(false);
        UpdateUIButtonsWithLocalization();
    }
    public void EnableGameModeSelectorMenu()
    {
        menuButtonsMainGameObject.SetActive(false);
        gameModeSelectorMenuGameObject.SetActive(true);
    }
    public void DisableGameModeSelectorMenu()
    {
        menuButtonsMainGameObject.SetActive(true);
        gameModeSelectorMenuGameObject.SetActive(false);
        UpdateUIButtonsWithLocalization();
    }
    public void EnableHowToPlayPanel()
    {
        menuButtonsMainGameObject.SetActive(false);
        howToPlayPanelGameObject.SetActive(true);
    }
    public void DisableHowToPlayPanel()
    {
        menuButtonsMainGameObject.SetActive(true);
        howToPlayPanelGameObject.SetActive(false);
        UpdateUIButtonsWithLocalization();
    }
    public void EnablePlotPanel()
    {
        menuButtonsMainGameObject.SetActive(false);
        plotPanelGameObject.SetActive(true);
    }
    public void DisablePlotPanel()
    {
        menuButtonsMainGameObject.SetActive(true);
        plotPanelGameObject.SetActive(false);
        UpdateUIButtonsWithLocalization();
    }
    public void EnableSkinSelectorMenu(int newGameMode)
    {
        if (newGameMode == 0)
            currentGameMode = GameMode.SinglePlayerClassic;
        else if (newGameMode == 1)
            currentGameMode = GameMode.SinglePlayerEndless;
        else if (newGameMode == 2)
            currentGameMode = GameMode.VersusClassic;
        else if (newGameMode == 3)
            currentGameMode = GameMode.VersusEndless;
        skinSelectorMenuGameObject.SetActive(true);
        gameModeSelectorMenuGameObject.SetActive(false);
        menuButtonsMainGameObject.SetActive(false);
        planeSkinSelectorScript.UpdateUIElements();
    }
    public void DisableSkinSelectorMenu()
    {
        menuButtonsMainGameObject.SetActive(true);
        skinSelectorMenuGameObject.SetActive(false);
        UpdateUIButtonsWithLocalization();
        gameplaySettings.ResetPlayerSkins();
    }
    public void LaunchTheGame()
    {
        fromMainMenu = true;
        if (currentGameMode == GameMode.SinglePlayerClassic || currentGameMode == GameMode.SinglePlayerEndless)
            StartGameSinglePlayer();
        else if (currentGameMode == GameMode.VersusClassic || currentGameMode == GameMode.VersusEndless)
            StartGameMultiPlayer();
    }
    private void GetArguments()
    {
        string[] arguments = Environment.GetCommandLineArgs();
        if(arguments.Length > 0)
        {
            for(int i=0;i<arguments.Length;i++)
            {
                if (arguments[i] == "-TrueGame")
                {
                    gameplaySettings.safeMode = false;
                    break;
                }
            }
            gameplaySettings.safeMode = true;
        }
    }
}
