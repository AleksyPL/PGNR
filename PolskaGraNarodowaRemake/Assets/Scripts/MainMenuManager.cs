using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public enum GameMode
{
    SinglePlayerClassic,
    SinglePlayerEndless,
    VersusClassic,
    VersusEndless
}

public class MainMenuManager : MonoBehaviour
{
    public GameObject eventSystemGameObject;
    private EventSystem eventSystem;
    internal AudioManager audioManagerScript;
    public GameObject audioManagerGameObject;
    internal PlaneSkinSelector planeSkinSelectorScript;
    public static GameMode currentGameMode;
    internal static bool fromMainMenu = false;
    public GameplaySettings gameplaySettings;
    [Header("Main menu buttons")]
    public GameObject menuButtonsMainGameObject;
    public GameObject mainMenuPlayGameButton;
    public GameObject mainMenuPlotMenuButton;
    public GameObject mainMenuControlsMenuButton;
    public GameObject mainMenuExitGameButton;
    public GameObject mainMenuOptionsMenuButton;
    [Header("Options panel")]
    public GameObject optionsMenuGameObject;
    [Header("Controls menu")]
    public GameObject controlsMenuMainGameObject;
    public GameObject controlsMenuTitleGameObject;
    public GameObject controlsMenuPlayerOneTextGameObject;
    public GameObject controlsMenuPlayerTwoTextGameObject;
    public GameObject controlsMenuPlayerOneIndicatorGameObject;
    public GameObject controlsMenuPlayerTwoIndicatorGameObject;
    public GameObject controlsMenuBackToMainMenuButton;
    [Header("Plot panel")]
    public GameObject plotPanelGameObject;
    public GameObject plotPanelTitleGameObject;
    public GameObject plotPanelStoryGameObject;
    public GameObject plotPanelBackToMainMenuButton;
    [Header("Skin selection panel")]
    public GameObject skinSelectorMenuGameObject;
    public GameObject skinSelectorStartGameGameObject;
    [Header("Game mode selection panel")]
    public GameObject startSinglePlayerClassicModeMenuButton;
    public GameObject startMultiPlayerClassicModeMenuButton;
    public GameObject startSinglePlayerEndlessModeMenuButton;
    public GameObject startMultiPlayerEndlessModeMenuButton;
    public GameObject gameModeSelectorMenuGameObject;
    public GameObject gameModeSelectorMissingGameModesGameObject;
    public GameObject gameModeSelectionMenuTitleGameObject;
    public GameObject gameModeSelectionMenuBackToMainMenuButton;
    [Header("Disclaimer panel")]
    public GameObject disclaimerPanelGameObject;
    public GameObject disclaimerPanelMessageGameObject;
    public GameObject disclaimerPanelAcceptanceButtonGameObject;
    void Start()
    {
        Application.targetFrameRate = 144;
#if (UNITY_WEBGL || UNITY_MOBILE)
        DisableQuitGameButton();
#endif
        currentGameMode = GameMode.SinglePlayerClassic;
        eventSystem = eventSystemGameObject.GetComponent<EventSystem>();
        planeSkinSelectorScript = GetComponent<PlaneSkinSelector>();
        audioManagerScript = audioManagerGameObject.GetComponent<AudioManager>();
        audioManagerScript.PlaySound("MainMenuTheme", audioManagerScript.localOtherSounds);
        UpdateUIButtonsWithLocalization();
        if (!gameplaySettings.safeMode && !gameplaySettings.introductionScreens)
            EnableDisclaimerPanel();
        else if (gameplaySettings.safeMode && !gameplaySettings.introductionScreens)
            EnablePlotPanel();
        else
            EnableMainMenuButtons();
    }
    private void UpdateUIButtonsWithLocalization()
    {
        mainMenuPlayGameButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuStartGame;
        mainMenuControlsMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuHowToPlay;
        mainMenuOptionsMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuOptions;
        mainMenuExitGameButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuQuitGame;
        mainMenuPlotMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButtonPlot;
        gameModeSelectionMenuBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        gameModeSelectionMenuTitleGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuTitle;
        startSinglePlayerClassicModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuSinglePlayerClassic;
        startSinglePlayerEndlessModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuSinglePlayerEndless;
        startMultiPlayerClassicModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuMultiPlayerClassic;
        startMultiPlayerEndlessModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuMultiPlayerEndless;
        gameModeSelectorMissingGameModesGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuMissingGameModes;
        controlsMenuTitleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuHowToPlay;
        plotPanelStoryGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].plotMenuPlot;
        plotPanelTitleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButtonPlot;
        controlsMenuPlayerOneTextGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].ControlsMenuPlayerOne;
        controlsMenuPlayerTwoTextGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].ControlsMenuPlayerTwo;
        controlsMenuPlayerOneIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
        controlsMenuPlayerTwoIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
        controlsMenuBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        plotPanelBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        if(!gameplaySettings.introductionScreens)
        {
            plotPanelBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].acceptanceMessage;
            controlsMenuBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].acceptanceMessage;
            disclaimerPanelAcceptanceButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].acceptanceMessage;
            disclaimerPanelMessageGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].disclaimerMessage;
        }
    }
    private void Update()
    {
        if (optionsMenuGameObject.activeSelf && Input.GetButtonDown("Cancel"))
            DisableOptionsMenu();
        else if (controlsMenuMainGameObject.activeSelf && Input.GetButtonDown("Cancel"))
            DisableControlsMenu();
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
    internal void EnableMainMenuButtons()
    {
        if (!UnityEngine.Device.Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(mainMenuPlayGameButton);
        menuButtonsMainGameObject.SetActive(true);
    }
    internal void DisableMainMenuButtons()
    {
        menuButtonsMainGameObject.SetActive(false);
    }
    public void EnableOptionsMenu()
    {
        DisableMainMenuButtons();
        optionsMenuGameObject.SetActive(true);
    }
    public void DisableOptionsMenu()
    {
        EnableMainMenuButtons();
        optionsMenuGameObject.SetActive(false);
        UpdateUIButtonsWithLocalization();
    }
    public void EnableGameModeSelectorMenu()
    {
        DisableMainMenuButtons();
        if (UnityEngine.Device.Application.isMobilePlatform)
        {
            startMultiPlayerClassicModeMenuButton.SetActive(false);
            startMultiPlayerEndlessModeMenuButton.SetActive(false);
            gameModeSelectorMissingGameModesGameObject.SetActive(true);
        }
        else
            eventSystem.SetSelectedGameObject(startSinglePlayerClassicModeMenuButton);
        gameModeSelectorMenuGameObject.SetActive(true);
    }
    public void DisableGameModeSelectorMenu()
    {
        EnableMainMenuButtons();
        gameModeSelectorMenuGameObject.SetActive(false);
        UpdateUIButtonsWithLocalization();
    }
    public void EnableControlsMenu()
    {
        if (!UnityEngine.Device.Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(controlsMenuBackToMainMenuButton);
        DisableMainMenuButtons();
        controlsMenuMainGameObject.SetActive(true);
    }
    public void DisableControlsMenu()
    {
        if (!gameplaySettings.introductionScreens)
            gameplaySettings.introductionScreens = true;
        EnableMainMenuButtons();
        controlsMenuMainGameObject.SetActive(false);
        UpdateUIButtonsWithLocalization();
    }
    public void EnablePlotPanel()
    {
        if (!UnityEngine.Device.Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(plotPanelBackToMainMenuButton);
        DisableMainMenuButtons();
        plotPanelGameObject.SetActive(true);
    }
    public void DisablePlotPanel()
    {
        if (!gameplaySettings.introductionScreens)
            EnableControlsMenu();
        else
            EnableMainMenuButtons();
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
        if (!UnityEngine.Device.Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(skinSelectorStartGameGameObject);
        skinSelectorMenuGameObject.SetActive(true);
        gameModeSelectorMenuGameObject.SetActive(false);
        DisableMainMenuButtons();
        planeSkinSelectorScript.UpdateUIElements();
    }
    public void DisableSkinSelectorMenu()
    {
        EnableMainMenuButtons();
        skinSelectorMenuGameObject.SetActive(false);
        UpdateUIButtonsWithLocalization();
        gameplaySettings.ResetPlayerSkins();
    }
    private void EnableDisclaimerPanel()
    {
        if (!UnityEngine.Device.Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(disclaimerPanelAcceptanceButtonGameObject);
        disclaimerPanelGameObject.SetActive(true);
    }
    public void DisableDisclaimerPanel()
    {
        disclaimerPanelGameObject.SetActive(false);
        EnablePlotPanel();
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
        mainMenuExitGameButton.SetActive(false);
    }
}
