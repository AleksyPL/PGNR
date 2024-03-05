using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;

public enum GameMode
{
    SinglePlayerClassic,
    SinglePlayerEndless,
    VersusClassic,
    VersusEndless
}

public class MainMenuManager : MonoBehaviour
{
    internal AudioManager audioManagerScript;
    public GameObject audioManagerGameObject;
    internal PlaneSkinSelector planeSkinSelectorScript;
    public static GameMode currentGameMode;
    internal static bool fromMainMenu = false;
    public GameplaySettings gameplaySettings;
    [Header("Main menu buttons")]
    public GameObject menuButtonsMainGameObject;
    public GameObject playGameButton;
    public GameObject plotButton;
    public GameObject howToPlayPanelMenuButton;
    public GameObject exitGameMenuButton;
    public GameObject optionsMenuButton;
    [Header("Options panel")]
    public GameObject optionsMenuGameObject;
    [Header("How to play panel")]
    public GameObject howToPlayPanelGameObject;
    public GameObject howToPlayPanelTitleGameObject;
    public GameObject howToPlayPanelControlsPlayerOneGameObject;
    public GameObject howToPlayPanelControlsPlayerTwoGameObject;
    public GameObject howToPlayPanelPlayerOneIndicatorGameObject;
    public GameObject howToPlayPanelPlayerTwoIndicatorGameObject;
    public GameObject howToPlayPanelBackToMainMenuButton;
    [Header("Plot panel")]
    public GameObject plotPanelGameObject;
    public GameObject plotPanelTitleGameObject;
    public GameObject plotPanelStoryGameObject;
    public GameObject plotPanelBackToMainMenuButton;
    [Header("Skin selection panel")]
    public GameObject skinSelectorMenuGameObject;
    [Header("Game mode selection panel")]
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
#if (UNITY_WEBGL || UNITY_MOBILE)
        DisableQuitGameButton();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.LandscapeLeft;
#endif
        currentGameMode = GameMode.SinglePlayerClassic;
        planeSkinSelectorScript = GetComponent<PlaneSkinSelector>();
        audioManagerScript = audioManagerGameObject.GetComponent<AudioManager>();
        audioManagerScript.PlaySound("MainMenuTheme", audioManagerScript.localOtherSounds);
        menuButtonsMainGameObject.SetActive(true);
        UpdateUIButtonsWithLocalization();
    }
    private void UpdateUIButtonsWithLocalization()
    {
        playGameButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuStartGame;
        howToPlayPanelMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuHowToPlay;
        optionsMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuOptions;
        exitGameMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuQuitGame;
        plotButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButtonPlot;
        gameModeSelectionMenuBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        gameModeSelectionMenuTitleGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuTitle;
        startSinglePlayerClassicModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuSinglePlayerClassic;
        startSinglePlayerEndlessModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuSinglePlayerEndless;
        startMultiPlayerClassicModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuMultiPlayerClassic;
        startMultiPlayerEndlessModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuMultiPlayerEndless;
        howToPlayPanelTitleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuHowToPlay;
        plotPanelTitleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButtonPlot;
        plotPanelStoryGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].plotMenuPlot;
        howToPlayPanelControlsPlayerOneGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].howtoPlayMenuControlsPlayerOne;
        howToPlayPanelControlsPlayerTwoGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].howtoPlayMenuControlsPlayerTwo;
        howToPlayPanelPlayerOneIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
        howToPlayPanelPlayerTwoIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
        howToPlayPanelBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        plotPanelBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
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
        if(Application.isMobilePlatform)
        {
            startMultiPlayerClassicModeMenuButton.SetActive(false);
            startMultiPlayerEndlessModeMenuButton.SetActive(false);
        }
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
    private void DisableQuitGameButton()
    {
        exitGameMenuButton.SetActive(false);
    }
}
