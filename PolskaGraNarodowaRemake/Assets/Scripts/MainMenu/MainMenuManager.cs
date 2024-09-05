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
    internal bool gameIsNotFullScreenedAndVertical;
    internal static bool fromMainMenu = false;
    public GameplaySettings gameplaySettings;
    [Header("Fullscreen button")]
    public GameObject fullscreenButtonGameObject;
    [Header("Background image")]
    public GameObject mainMenuBackgroundUpperImageGameObject;
    public GameObject mainMenuBackgroundLowerImageGameObject;
    public GameObject mainMenuLogoImageGameObject;
    public GameObject mainMenuSignatureGameObject;
    public Sprite regularMainMenuBackgroundImage;
    public Sprite alternativeMainMenuBackgroundImage;
    [Header("Main menu buttons")]
    public GameObject menuButtonsMainGameObject;
    public GameObject mainMenuPlayGameButton;
    public GameObject mainMenuPlotMenuButton;
    public GameObject mainMenuControlsMenuButton;
    public GameObject mainMenuTutorialMenuButton;
    public GameObject mainMenuExitGameButton;
    public GameObject mainMenuOptionsMenuButton;
    [Header("Options panel")]
    public GameObject optionsMenuGameObject;
    [Header("Controls menu")]
    public GameObject controlsMenuMainGameObject;
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
    public GameObject disclaimerPanelTitleGameObject;
    public GameObject disclaimerPanelAcceptanceButtonGameObject;
    [Header("Tutorial panel")]
    public GameObject tutorialPanelGameObject;
    public GameObject tutorialPanelTitle;
    public GameObject tutorialPanelMessage;
    public GameObject tutorialPanelYesButton;
    public GameObject tutorialPanelNoButton;
    void Start()
    {
        Application.targetFrameRate = 144;
        gameIsNotFullScreenedAndVertical = false;
        if (UnityEngine.Device.Application.isMobilePlatform || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            DisableQuitGameButton();
            if (Screen.orientation != ScreenOrientation.LandscapeLeft && Screen.orientation != ScreenOrientation.LandscapeRight)
                gameIsNotFullScreenedAndVertical = true;
        }
//#if (UNITY_WEBGL || UNITY_MOBILE)
//        DisableQuitGameButton();
//#endif
        currentGameMode = GameMode.SinglePlayerClassic;
        eventSystem = eventSystemGameObject.GetComponent<EventSystem>();
        planeSkinSelectorScript = GetComponent<PlaneSkinSelector>();
        audioManagerScript = audioManagerGameObject.GetComponent<AudioManager>();
        audioManagerScript.PlaySound("MainMenuTheme", audioManagerScript.localOtherSounds);
        UpdateUIButtonsWithLocalization();
        if (!gameIsNotFullScreenedAndVertical)
            RegularGameLaunchProcedure();
        else
            GameStartedAsMobileVertical();
    }
    private void UpdateUIButtonsWithLocalization()
    {
        mainMenuPlayGameButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuStartGame;
        mainMenuControlsMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuControls;
        mainMenuOptionsMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuOptions;
        mainMenuExitGameButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuQuitGame;
        mainMenuPlotMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuPlot;
        mainMenuTutorialMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialTitle;
        gameModeSelectionMenuBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        gameModeSelectionMenuTitleGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuTitle;
        startSinglePlayerClassicModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuSinglePlayerClassic;
        startSinglePlayerEndlessModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuSinglePlayerEndless;
        startMultiPlayerClassicModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuMultiPlayerClassic;
        startMultiPlayerEndlessModeMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuMultiPlayerEndless;
        gameModeSelectorMissingGameModesGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameModeSelectionMenuMissingGameModes;
        plotPanelStoryGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].plotMenuPlot;
        plotPanelTitleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuPlot;
        plotPanelBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        if(!gameplaySettings.introductionScreens)
        {
            plotPanelBackToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].acceptanceMessage;
            disclaimerPanelAcceptanceButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].acceptanceMessage;
            disclaimerPanelTitleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].disclaimerTitle;
            disclaimerPanelMessageGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].disclaimerMessage;
        }
        if(!gameplaySettings.tutorialScreen)
        {
            tutorialPanelTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialTitle;
            tutorialPanelMessage.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialQuestion;
            tutorialPanelYesButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].warningYes;
            tutorialPanelNoButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].warningNo;
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
    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    internal void EnableMainMenuButtons()
    {
        if (!UnityEngine.Device.Application.isMobilePlatform)
        {
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(mainMenuPlayGameButton);
        }
        else if (UnityEngine.Device.Application.isMobilePlatform && fullscreenButtonGameObject.GetComponent<FullScreenManager>().landscapeModeEnabled)
            fullscreenButtonGameObject.GetComponent<FullScreenManager>().TurnOnFullScreenButton();
        menuButtonsMainGameObject.SetActive(true);
    }
    public void DisableMainMenuButtons()
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
        if (!gameplaySettings.tutorialScreen)
            EnableTutorialScreen();
        else
        {
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
            eventSystem.SetSelectedGameObject(controlsMenuMainGameObject.GetComponent<UIControlsMenu>().backToMainMenuButton);
        DisableMainMenuButtons();
        controlsMenuMainGameObject.SetActive(true);
    }
    public void DisableControlsMenu()
    {
        if (!gameplaySettings.introductionScreens)
            gameplaySettings.introductionScreens = true;
        eventSystem.SetSelectedGameObject(mainMenuPlayGameButton);
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
    private void EnableTutorialScreen()
    {
        gameplaySettings.tutorialScreen = true;
        if (!UnityEngine.Device.Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(tutorialPanelYesButton);
        tutorialPanelGameObject.SetActive(true);
    }
    public void DisableTutorialScreen()
    {
        tutorialPanelGameObject.SetActive(false);
        EnableGameModeSelectorMenu();
        UpdateUIButtonsWithLocalization();
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
    private void GameStartedAsMobileVertical()
    {
        fullscreenButtonGameObject.GetComponent<FullScreenManager>().ModifyRectTransformOfTheGameObject();
        mainMenuBackgroundUpperImageGameObject.GetComponent<Image>().sprite = alternativeMainMenuBackgroundImage;
        mainMenuBackgroundLowerImageGameObject.SetActive(false);
        mainMenuBackgroundUpperImageGameObject.GetComponent<BackgroundPlaneSpawner>().enabled = false;
        mainMenuLogoImageGameObject.SetActive(false);
        mainMenuSignatureGameObject.SetActive(false);
        DisableMainMenuButtons();
    }
    internal void ScreenOrientationChangedToHorizontal()
    {
        fullscreenButtonGameObject.GetComponent<FullScreenManager>().ResetRectTransformOfTheGameObject();
        mainMenuBackgroundUpperImageGameObject.GetComponent<Image>().sprite = regularMainMenuBackgroundImage;
        mainMenuBackgroundLowerImageGameObject.SetActive(true);
        mainMenuBackgroundUpperImageGameObject.GetComponent<BackgroundPlaneSpawner>().enabled = true;
        mainMenuLogoImageGameObject.SetActive(true);
        mainMenuSignatureGameObject.SetActive(true);
        RegularGameLaunchProcedure();
    }
    private void RegularGameLaunchProcedure()
    {
        if (!gameplaySettings.safeMode && !gameplaySettings.introductionScreens)
            EnableDisclaimerPanel();
        else if (gameplaySettings.safeMode && !gameplaySettings.introductionScreens)
            EnablePlotPanel();
        else
        {
            EnableMainMenuButtons();
            if (UnityEngine.Device.Application.isMobilePlatform)
                fullscreenButtonGameObject.GetComponent<FullScreenManager>().TurnOnFullScreenButton();
        }
    }
}
