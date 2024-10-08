using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameplaySettings gameplaySettings;
    public GameObject UIElements;
    public GameObject eventSystemGameObject;
    private EventSystem eventSystem;
    internal FlightController flightControllerScript;
    internal bool pauseScreenEnabled;
    internal bool tutorialScreenEnabled;
    internal bool timerBeforeTheFlightEnabled;
    [System.Serializable]
    public class PlayerUI
    {
        internal float powerUpMessageOnScreenCounter;
        internal bool powerUpMessageOnScreenEnabled;
        [Header("Color Panels")]
        [SerializeField] internal GameObject colorPanelGameObject;
        [Header("Regular HUD")]
        [SerializeField] internal GameObject regularHUDMainGameObject;
        [SerializeField] internal GameObject regularHUDLevelProgressGameObject;
        [SerializeField] internal GameObject regularHUDScoreGameObject;
        [SerializeField] internal GameObject regularHUDBottlesGameObject;
        [Header("Game Statistics")]
        [SerializeField] internal GameObject gameSummaryPlayerIndicator;
        [SerializeField] internal GameObject gameSummaryYearGameObject;
        [SerializeField] internal GameObject gameSummaryScoreGameObject;
        [SerializeField] internal GameObject gameSummaryBottlesGameObject;
        [Header("PowerUps")]
        [SerializeField] internal GameObject powerUpBarParentGameObject;
        [SerializeField] internal GameObject powerUpBarParentGameObjectMobile;
        PlayerUI()
        {
            powerUpMessageOnScreenCounter = 0;
            powerUpMessageOnScreenEnabled = false;
        }
    };
    public PlayerUI playerOneUI;
    public PlayerUI playerTwoUI;
    [Header("Color Panels")]
    [SerializeField] internal Color winColor;
    [SerializeField] internal Color loseColor;
    [Header("Pause Screen")]
    [SerializeField] private GameObject fadePanelGameObject;
    [SerializeField] private GameObject pauseScreenGameObject;
    [SerializeField] private GameObject pauseScreenRegularButtonsGameObject;
    [SerializeField] private GameObject pauseScreenWarningGameObject;
    [SerializeField] private GameObject pauseScreenTitleGameObject;
    [SerializeField] private GameObject pauseScreenOptionsButtonGameObject;
    [SerializeField] private GameObject pauseScreenControlsButtonGameObject;
    [SerializeField] private GameObject pauseScreenBackToMainMenuButtonGameObject;
    [SerializeField] private GameObject pauseScreenResumeGameButtonGameObject;
    [SerializeField] internal GameObject fullScreenButton;
    [Header("Game Statistics")]
    [SerializeField] private GameObject gameStatsGameObject;
    [SerializeField] private GameObject gameSummaryYearTitle;
    [SerializeField] private GameObject gameSummaryBottlesTitle;
    [SerializeField] private GameObject gameSummaryScoreTitle;
    [Header("PowerUps")]
    [SerializeField] internal GameObject powerUpUIClockPrefab;
    [Header("Options Menu")]
    [SerializeField] private GameObject optionsMenuGameObject;
    [Header("Controls Menu")]
    [SerializeField] private GameObject controlsMenuGameObject;
    [Header("TimerCircle")]
    [SerializeField] private GameObject timerPrefab;
    [SerializeField] private float pauseScreenTimerDuration;
    [Header("ActiveBottleWarning")]
    [SerializeField] private GameObject activeBottleWarningPrefab;
    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverScreenButtonsGameObject;
    [SerializeField] private GameObject gameOverScreenTryAgainButtonGameObject;
    [SerializeField] private GameObject gameOverScreenExitButtonGameObject;
    [Header("Exit Warning")]
    [SerializeField] private GameObject exitWarningTitleGameObject;
    [SerializeField] private GameObject exitWarningYesButtonGameObject;
    [SerializeField] private GameObject exitWarningNoButtonGameObject;
    [Header("Touch Screen")]
    [SerializeField] private GameObject touchScreenMainGameObject;
    [Header("Tutorial")]
    [SerializeField] internal GameObject tutorialMainGameObject;
    [SerializeField] private GameObject tutorialTitleGameObject;
    [SerializeField] internal GameObject tutorialPlaceToSpawnScreens;
    [SerializeField] private GameObject tutorialOKButton;

    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        eventSystem = eventSystemGameObject.GetComponent<EventSystem>();
        pauseScreenEnabled = false;
        optionsMenuGameObject.GetComponent<UIOptionsMenu>().DisableLanguageButtons();
        if (UnityEngine.Device.Application.isMobilePlatform)
            TurnOnTouchScreenButtons();
    }
    private ref PlayerUI ReturnPlayersUIObject(Plane plane)
    {
        flightControllerScript = GetComponent<FlightController>();
        if (plane == flightControllerScript.gameModeScript.playerOnePlane)
            return ref playerOneUI;
        return ref playerTwoUI;
    }
    internal void CloseWindowsUsingESC()
    {
        if (pauseScreenWarningGameObject.activeSelf)
            DisableExitWarning();
        else if (optionsMenuGameObject.activeSelf)
            DisableOptionsMenu();
        else if (controlsMenuGameObject.activeSelf)
            DisableControlsMenu();
        else if (((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless) && flightControllerScript.gameModeScript.playerOneState != GameModeManager.PlayerState.crashed) || ((flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && flightControllerScript.gameModeScript.playerOneState != GameModeManager.PlayerState.crashed && flightControllerScript.gameModeScript.playerTwoState != GameModeManager.PlayerState.crashed))
        {
            if (!pauseScreenEnabled && !pauseScreenGameObject.activeSelf && !tutorialScreenEnabled && !timerBeforeTheFlightEnabled)
            {
                EnablePauseScreen();
                flightControllerScript.gameModeScript.playerOnePlane.activeBottleWarning = flightControllerScript.gameModeScript.playerOnePlane.attackKeyPressed;
                if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
                    flightControllerScript.gameModeScript.playerTwoPlane.activeBottleWarning = flightControllerScript.gameModeScript.playerTwoPlane.attackKeyPressed;
            }
            else if (pauseScreenEnabled && pauseScreenGameObject.activeSelf)
                DisablePauseScreen();
        }
    }
    private void UpdatePauseScreenHUD()
    {
        gameSummaryBottlesTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenBottlesTitle;
        gameSummaryScoreTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned0;
        gameSummaryYearTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenLevelTitle;
        playerOneUI.gameSummaryBottlesGameObject.GetComponent<TMP_Text>().text = flightControllerScript.gameModeScript.playerOnePlane.bottlesDrunkTotal.ToString();
        playerOneUI.gameSummaryScoreGameObject.GetComponent<TMP_Text>().text = ((int)flightControllerScript.gameModeScript.playerOnePlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
        if (flightControllerScript.gameplaySettings.safeMode)
            playerOneUI.gameSummaryYearGameObject.GetComponent<TMP_Text>().text = flightControllerScript.rewardAndProgressionManagerScript.levelCounter.ToString();
        else
            playerOneUI.gameSummaryYearGameObject.GetComponent<TMP_Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
        pauseScreenOptionsButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuOptions;
        pauseScreenResumeGameButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenResumeGame;
        pauseScreenControlsButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuControls;
        pauseScreenBackToMainMenuButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            playerOneUI.gameSummaryPlayerIndicator.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
            playerTwoUI.gameSummaryPlayerIndicator.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
            playerTwoUI.gameSummaryBottlesGameObject.GetComponent<TMP_Text>().text = flightControllerScript.gameModeScript.playerTwoPlane.bottlesDrunkTotal.ToString();
            playerTwoUI.gameSummaryScoreGameObject.GetComponent<TMP_Text>().text = ((int)flightControllerScript.gameModeScript.playerTwoPlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
            if (flightControllerScript.gameplaySettings.safeMode)
                playerTwoUI.gameSummaryYearGameObject.GetComponent<TMP_Text>().text = flightControllerScript.rewardAndProgressionManagerScript.levelCounter.ToString();
            else
                playerTwoUI.gameSummaryYearGameObject.GetComponent<TMP_Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
        }
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            gameSummaryYearTitle.GetComponent<TMP_Text>().text = "";
            playerOneUI.gameSummaryYearGameObject.GetComponent<TMP_Text>().text = "";
            if(playerTwoUI.regularHUDMainGameObject != null)
                playerTwoUI.gameSummaryYearGameObject.GetComponent<TMP_Text>().text = "";

        }
    }
    internal void UpdateLevelProgressBar(Plane plane)
    {
        if ((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic) && !timerBeforeTheFlightEnabled && plane.currentPlaneState == PlaneState.standard || plane.currentPlaneState == PlaneState.wheelsOn)
        {
            if (flightControllerScript.rewardAndProgressionManagerScript.ReturnPlayerProgressObject(plane).levelProgressCounter < flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace)
            {
                int levelProgress = (int)(flightControllerScript.rewardAndProgressionManagerScript.ReturnPlayerProgressObject(plane).levelProgressCounter / (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) * 100);
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression0 + levelProgress + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression1;
            }
            else if (flightControllerScript.rewardAndProgressionManagerScript.ReturnPlayerProgressObject(plane).levelProgressCounter >= (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) && plane.currentPlaneSpeed > 0)
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudLandingMessage;
            else if (flightControllerScript.rewardAndProgressionManagerScript.ReturnPlayerProgressObject(plane).levelProgressCounter >= (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) && plane.currentPlaneSpeed == 0 && flightControllerScript.gameModeScript.playerOneState == GameModeManager.PlayerState.landed)
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudCongratulationsAfterLanding;
        }
        if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.tutorial && plane.currentPlaneState == PlaneState.damaged)
            ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudPlaneHit;
        if(flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.tutorial && (plane.currentPlaneState == PlaneState.standard || plane.currentPlaneState == PlaneState.wheelsOn))
        {
            if (flightControllerScript.tutorialManagerScript.checkpointNumber == 1)
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen1ProgressBar;
            else if (flightControllerScript.tutorialManagerScript.checkpointNumber == 2)
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen2ProgressBar;
            else if (flightControllerScript.tutorialManagerScript.checkpointNumber == 3)
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen5ProgressBar;
            else if (flightControllerScript.tutorialManagerScript.checkpointNumber == 4)
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen7ProgressBar;
            else if (flightControllerScript.tutorialManagerScript.checkpointNumber == 5)
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen9ProgressBar;
            else
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = "";
        }
    }
    internal void UpdateBottlesCounter(Plane plane)
    {
        if ((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.tutorial) && (plane.bottlesDrunk != plane.bottlesDrunkTotal))
            ReturnPlayersUIObject(plane).regularHUDBottlesGameObject.GetComponent<TMP_Text>().text = ((int)plane.bottlesDrunk).ToString() + " (" + ((int)plane.bottlesDrunkTotal).ToString() + ")";
        else
            ReturnPlayersUIObject(plane).regularHUDBottlesGameObject.GetComponent<TMP_Text>().text = ((int)plane.bottlesDrunk).ToString();
    }
    internal void UpdateScoreCounter(Plane plane)
    {
        ReturnPlayersUIObject(plane).regularHUDScoreGameObject.GetComponent<TMP_Text>().text = ((int)plane.gameScore).ToString();
    }
    private void EnablePauseScreen()
    {
        flightControllerScript = GetComponent<FlightController>();
        if (((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless) && flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed) || ((flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && (flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed || flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState != PlaneState.crashed)))
        {
            Time.timeScale = 0;
            pauseScreenEnabled = true;
            UpdatePauseScreenHUD();
            if (UnityEngine.Device.Application.isMobilePlatform)
            {
                TurnOffTouchScreenButtons();
                flightControllerScript.inputManagerScript.ESCpressed = false;
                fullScreenButton.GetComponent<FullScreenManager>().TurnOnFullScreenButton();
            }
            else
                eventSystem.SetSelectedGameObject(pauseScreenResumeGameButtonGameObject);
            fadePanelGameObject.SetActive(true);
            pauseScreenGameObject.SetActive(true);
            pauseScreenTitleGameObject.SetActive(true);
            pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenPauseMainTitle;
            pauseScreenRegularButtonsGameObject.SetActive(true);
            gameStatsGameObject.SetActive(true);
            playerOneUI.regularHUDMainGameObject.SetActive(false);
            if (playerTwoUI.regularHUDMainGameObject != null)
                playerTwoUI.regularHUDMainGameObject.SetActive(false);
            flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localSFX);
            flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localOneLinersSounds);
            flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localHitReactionSounds);
            flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localLandingSounds);
        }
    }
    internal void EnableGameOverScreen()
    {
        if (UnityEngine.Device.Application.isMobilePlatform)
            TurnOffTouchScreenButtons();
        else
            eventSystem.SetSelectedGameObject(gameOverScreenTryAgainButtonGameObject);
        UpdatePauseScreenHUD();   
        TurnOffColorPanel(playerOneUI.colorPanelGameObject);
        TurnOffColorPanel(playerTwoUI.colorPanelGameObject);
        pauseScreenTitleGameObject.SetActive(true);
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenGameOverMainTitle;
        gameOverScreenTryAgainButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameOverScreenTryAgain;
        gameOverScreenExitButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        pauseScreenRegularButtonsGameObject.SetActive(false);
        gameOverScreenButtonsGameObject.SetActive(true);
        playerOneUI.regularHUDMainGameObject.SetActive(false);
        if (playerTwoUI.regularHUDMainGameObject != null)
            playerTwoUI.regularHUDMainGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(true);
        pauseScreenEnabled = true;
    }
    internal void DisableGameOverScreen()
    {
        if (UnityEngine.Device.Application.isMobilePlatform)
        {
            TurnOnTouchScreenButtons();
            fullScreenButton.GetComponent<FullScreenManager>().TurnOffFullScreenButton();
        }
        gameOverScreenButtonsGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(false);
        pauseScreenEnabled = false;
    }
    public void DisablePauseScreen()
    {
        fadePanelGameObject.SetActive(false);
        pauseScreenTitleGameObject.SetActive(false);
        pauseScreenRegularButtonsGameObject.SetActive(false);
        gameStatsGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(false);
        if (UnityEngine.Device.Application.isMobilePlatform)
            flightControllerScript.uiManagerScript.fullScreenButton.GetComponent<FullScreenManager>().TurnOffFullScreenButton();
        if (flightControllerScript.gameModeScript.playerOnePlane.activeBottleWarning || flightControllerScript.gameModeScript.playerTwoPlane.activeBottleWarning)
        {
            GameObject waring = Instantiate(activeBottleWarningPrefab, UIElements.transform);
            waring.transform.name = "ActiveBottleWarning";
        }
        GameObject timer = SpawnTimerOnTheScreen(pauseScreenTimerDuration);
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            timer.GetComponent<RectTransform>().position = new Vector3(timer.GetComponent<RectTransform>().position.x, timer.GetComponent<RectTransform>().position.y + 100f, 0);
            timer.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            timer = SpawnTimerOnTheScreen(pauseScreenTimerDuration);
            timer.GetComponent<RectTransform>().position = new Vector3(timer.GetComponent<RectTransform>().position.x, timer.GetComponent<RectTransform>().position.y - 150f, 0);
            timer.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
    internal void EnableBeforeTheFlightProcedure(string message, float timeToCount)
    {
        //hack
        flightControllerScript = GetComponent<FlightController>();
        //end of hack
        timerBeforeTheFlightEnabled = true;
        Time.timeScale = 0;
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localSFX);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localOneLinersSounds);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localHitReactionSounds);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localLandingSounds);
        playerOneUI.regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = message;
        GameObject timer = SpawnTimerOnTheScreen(timeToCount);
        if(flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            timer.GetComponent<RectTransform>().position = new Vector3(timer.GetComponent<RectTransform>().position.x, timer.GetComponent<RectTransform>().position.y + 100f, 0);
            timer.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            timer = SpawnTimerOnTheScreen(timeToCount);
            timer.GetComponent<RectTransform>().position = new Vector3(timer.GetComponent<RectTransform>().position.x, timer.GetComponent<RectTransform>().position.y - 150f, 0);
            timer.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            playerTwoUI.regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = message;
        }
    }
    public GameObject SpawnTimerOnTheScreen(float timeToCount)
    {
        GameObject timer = Instantiate(timerPrefab, UIElements.transform);
        timer.transform.name = "CircleTimer";
        timer.GetComponent<UIClock>().TurnOnTheTimer(timeToCount);
        return timer;
    }
    public void EnableExitWarning()
    {
        gameStatsGameObject.SetActive(false);
        if(!UnityEngine.Device.Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(exitWarningNoButtonGameObject);
        exitWarningTitleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].warningTitle;
        exitWarningYesButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].warningYes;
        exitWarningNoButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].warningNo;
        pauseScreenRegularButtonsGameObject.SetActive(false);
        pauseScreenWarningGameObject.SetActive(true);
    }
    public void DisableExitWarning()
    {
        gameStatsGameObject.SetActive(true);
        pauseScreenRegularButtonsGameObject.SetActive(true);
        if (!UnityEngine.Device.Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(pauseScreenOptionsButtonGameObject);
        pauseScreenWarningGameObject.SetActive(false);
    }
    public void EnableOptionsMenu()
    {
        if (!UnityEngine.Device.Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(optionsMenuGameObject.GetComponent<UIOptionsMenu>().backToPauseScreenButton);
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenResumeGame;
        optionsMenuGameObject.SetActive(true);
        pauseScreenRegularButtonsGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(false);
        playerOneUI.regularHUDMainGameObject.SetActive(false);
        if (playerTwoUI.regularHUDMainGameObject != null)
            playerTwoUI.regularHUDMainGameObject.SetActive(false);
    }
    public void DisableOptionsMenu()
    {
        optionsMenuGameObject.SetActive(false);
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenPauseMainTitle;
        flightControllerScript.GetComponent<FlightController>().audioManagerScript.UpdateAllSoundsVolume();
        if (pauseScreenEnabled)
            EnablePauseScreen();
    }
    public void EnableControlsMenu()
    {
        if (!UnityEngine.Device.Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(controlsMenuGameObject.GetComponent<UIControlsMenu>().backToMainMenuButton);
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenResumeGame;
        controlsMenuGameObject.SetActive(true);
        pauseScreenRegularButtonsGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(false);
        playerOneUI.regularHUDMainGameObject.SetActive(false);
        if (playerTwoUI.regularHUDMainGameObject != null)
            playerTwoUI.regularHUDMainGameObject.SetActive(false);
    }
    public void DisableControlsMenu()
    {
        controlsMenuGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(true);
        gameStatsGameObject.SetActive(true);
        pauseScreenRegularButtonsGameObject.SetActive(true);
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenPauseMainTitle;
        if (pauseScreenEnabled)
            EnablePauseScreen();
    }
    internal void TurnOnColorPanel(GameObject colorPanel, Color panelColor)
    {
        colorPanel.SetActive(true);
        colorPanel.GetComponent<Image>().color = panelColor;
    }
    internal void TurnOffColorPanel(GameObject colorPanel)
    {
        colorPanel.SetActive(false);
    }
    internal void SetTheTextOnTheColorPanel(GameObject colorPanel, string text)
    {
        colorPanel.transform.Find("Text").gameObject.GetComponent<TMP_Text>().text = text;
    }
    internal void DisplayPowerUpDescriptionOnHUD(Plane plane, string messageToDisplay)
    {
        flightControllerScript.gameModeScript.flightControllerScript.uiManagerScript.ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = messageToDisplay;
        if (ReturnPlayersUIObject(plane).powerUpMessageOnScreenEnabled)
            ReturnPlayersUIObject(plane).powerUpMessageOnScreenCounter = 0;
        else
            ReturnPlayersUIObject(plane).powerUpMessageOnScreenEnabled = true;
        if(UnityEngine.Device.Application.isMobilePlatform)
            ChangeTheOrderOnThePowerUpsBar(playerOneUI.powerUpBarParentGameObjectMobile);
        else
            ChangeTheOrderOnThePowerUpsBar(playerOneUI.powerUpBarParentGameObject);
    }
    internal void DisablePowerUpMessage(Plane plane)
    {
        if(ReturnPlayersUIObject(plane).powerUpMessageOnScreenEnabled)
        {
            ReturnPlayersUIObject(plane).powerUpMessageOnScreenCounter += Time.deltaTime;
            if (ReturnPlayersUIObject(plane).powerUpMessageOnScreenCounter > gameplaySettings.durationTimeForPowerUpMessageOnTheScreen)
            {
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = "";
                ReturnPlayersUIObject(plane).powerUpMessageOnScreenCounter = 0;
                ReturnPlayersUIObject(plane).powerUpMessageOnScreenEnabled = false;
            }
        }
    }
    internal void SpawnPowerUpUIClock(Plane plane, PowerUp currentPowerUp)
    {
        if ((plane == flightControllerScript.gameModeScript.playerOnePlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUp.powerUpName) == null) || (plane == flightControllerScript.gameModeScript.playerTwoPlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUp.powerUpName) == null))
        {
            GameObject powerUpUIGameObject = Instantiate(powerUpUIClockPrefab);
            powerUpUIGameObject.transform.name = currentPowerUp.powerUpName;
            if(gameplaySettings.safeMode)
                powerUpUIGameObject.GetComponent<Image>().sprite = currentPowerUp.currentPowerUpSafeUIClockImage;
            else
                powerUpUIGameObject.GetComponent<Image>().sprite = currentPowerUp.currentPowerUpUIClockImage;
            if (UnityEngine.Device.Application.isMobilePlatform)
                powerUpUIGameObject.transform.SetParent(ReturnPlayersUIObject(plane).powerUpBarParentGameObjectMobile.transform);
            else
                powerUpUIGameObject.transform.SetParent(ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform);
            powerUpUIGameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            powerUpUIGameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            powerUpUIGameObject.GetComponent<UIPowerUp>().EnableUIPowerUp(currentPowerUp.powerUpDuration, currentPowerUp.powerUpName);
        }
    }
    internal void DeletePowerUpUIClock(Plane plane, string currentPowerUpName)
    {
        if ((plane == flightControllerScript.gameModeScript.playerOnePlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUpName) != null) || (plane == flightControllerScript.gameModeScript.playerTwoPlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUpName) != null))
        {
            GameObject.Destroy(ReturnPowerUpUIClockIfExists(plane, currentPowerUpName));
            if (UnityEngine.Device.Application.isMobilePlatform)
                ChangeTheOrderOnThePowerUpsBar(ReturnPlayersUIObject(plane).powerUpBarParentGameObjectMobile);
            else
                ChangeTheOrderOnThePowerUpsBar(ReturnPlayersUIObject(plane).powerUpBarParentGameObject);
        }
    }
    internal void ClearPowerUpBar(Plane plane)
    {
        if (UnityEngine.Device.Application.isMobilePlatform)
        {
            if (ReturnPlayersUIObject(plane).powerUpBarParentGameObjectMobile.transform.childCount != 0 && ReturnPlayersUIObject(plane).powerUpBarParentGameObjectMobile.transform.childCount > 0)
            {
                foreach (Transform child in ReturnPlayersUIObject(plane).powerUpBarParentGameObjectMobile.transform)
                    GameObject.Destroy(child.gameObject);
            }
        }
        else
        {
            if (ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.childCount != 0 && ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.childCount > 0)
            {
                foreach (Transform child in ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform)
                    GameObject.Destroy(child.gameObject);
            }
        }
    }
    internal void ChangeTheOrderOnThePowerUpsBar(GameObject powerUpBarGameObject)
    {
        if (powerUpBarGameObject.transform.childCount != 0)
        {
            List<GameObject> myObjectsUnsorted = new();
            for (int i=0;i<powerUpBarGameObject.transform.childCount;i++)
            {
                if (powerUpBarGameObject.transform.GetChild(i).gameObject.GetComponent<UIPowerUp>().powerUpDurationCounter > 0)
                    myObjectsUnsorted.Add(powerUpBarGameObject.transform.GetChild(i).gameObject);
                else
                    GameObject.Destroy(powerUpBarGameObject.transform.GetChild(i).gameObject);
            }
            List<GameObject> myObjectsSorted = myObjectsUnsorted.OrderBy(o => o.GetComponent<UIPowerUp>().powerUpDurationCounter).ToList();
            for (int i = 0; i < myObjectsSorted.Count; i++)
            {
                myObjectsSorted[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                myObjectsSorted[i].GetComponent<RectTransform>().localPosition = new Vector3(0-(i*100), 0, 0);
            }
        }
    }
    internal GameObject ReturnPowerUpUIClockIfExists(Plane plane, string powerUpUIClockName)
    {
        if(UnityEngine.Device.Application.isMobilePlatform)
        {
            if (ReturnPlayersUIObject(plane).powerUpBarParentGameObjectMobile.transform.childCount != 0)
            {
                for (int i = 0; i < ReturnPlayersUIObject(plane).powerUpBarParentGameObjectMobile.transform.childCount; i++)
                {
                    if (ReturnPlayersUIObject(plane).powerUpBarParentGameObjectMobile.transform.GetChild(i).gameObject.name == powerUpUIClockName)
                        return ReturnPlayersUIObject(plane).powerUpBarParentGameObjectMobile.transform.GetChild(i).gameObject;
                }
            }
        }
        else
        {
            if (ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.childCount != 0)
            {
                for (int i = 0; i < ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.childCount; i++)
                {
                    if (ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.GetChild(i).gameObject.name == powerUpUIClockName)
                        return ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.GetChild(i).gameObject;
                }
            }
        }
        return null;
    }
    internal void ResetDurationForTheUIPowerUpClock(Plane plane, string powerUpUIClockName)
    {
        if (ReturnPowerUpUIClockIfExists(plane, powerUpUIClockName) != null)
            ReturnPowerUpUIClockIfExists(plane, powerUpUIClockName).GetComponent<UIPowerUp>().powerUpDurationCounter = ReturnPowerUpUIClockIfExists(plane, powerUpUIClockName).GetComponent<UIPowerUp>().powerUpDuration;
    }
    internal void TurnOnTouchScreenButtons()
    {
        if (touchScreenMainGameObject != null)
            touchScreenMainGameObject.SetActive(true);
    }
    private void TurnOffTouchScreenButtons()
    {
        if (touchScreenMainGameObject != null)
            touchScreenMainGameObject.SetActive(false);
    }
    internal void EnableTutorialScreen()
    {
        Time.timeScale = 0;
        tutorialScreenEnabled = true;
        playerOneUI.regularHUDMainGameObject.SetActive(false);
        tutorialMainGameObject.SetActive(true);
        flightControllerScript.tutorialManagerScript.currentTutorialState = TutorialManager.TutorialPlayerState.Frozen;
        if (UnityEngine.Device.Application.isMobilePlatform)
            TurnOffTouchScreenButtons();
        else
            eventSystem.SetSelectedGameObject(tutorialOKButton);
        flightControllerScript.tutorialManagerScript.SpawnTutorialInfoScreen();
        tutorialTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialTitle;
        tutorialOKButton.transform.Find("Text").GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].acceptanceMessage;
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localSFX);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localOneLinersSounds);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localHitReactionSounds);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localLandingSounds);
    }
    public void DisableTutorialScreen()
    {
        //display another screen
        if (!flightControllerScript.tutorialManagerScript.checkpointFailedTryAgain && (flightControllerScript.tutorialManagerScript.tutorialScreens[flightControllerScript.tutorialManagerScript.checkpointNumber].screensToShow.Length-1) != flightControllerScript.tutorialManagerScript.tutorialScreens[flightControllerScript.tutorialManagerScript.checkpointNumber].screenToShowIndex)
        {
            GameObject.Destroy(tutorialPlaceToSpawnScreens.transform.Find("TutorialScreen").gameObject);
            flightControllerScript.tutorialManagerScript.tutorialScreens[flightControllerScript.tutorialManagerScript.checkpointNumber].screenToShowIndex++;
            EnableTutorialScreen();
        }
        //close Try Again screen and start reverting
        else if(flightControllerScript.tutorialManagerScript.checkpointFailedTryAgain)
        {
            Time.timeScale = 1;
            flightControllerScript.tutorialManagerScript.CalculateRevertDuration();
            GameObject.Destroy(tutorialPlaceToSpawnScreens.transform.Find("TutorialScreen").gameObject);
            flightControllerScript.levelManagerScript.CleanLevel(flightControllerScript.gameModeScript.playerOnePlane);
            flightControllerScript.tutorialManagerScript.PlayRewindSFX();
            tutorialScreenEnabled = false;
            playerOneUI.regularHUDMainGameObject.SetActive(true);
            tutorialMainGameObject.SetActive(false);
            flightControllerScript.uiManagerScript.playerOneUI.regularHUDMainGameObject.SetActive(true);
            UpdateLevelProgressBar(flightControllerScript.gameModeScript.playerOnePlane);
            flightControllerScript.tutorialManagerScript.currentTutorialState = TutorialManager.TutorialPlayerState.Reverting;
        }
        //close tutorial
        else if (flightControllerScript.tutorialManagerScript.checkpointNumber == 6)
        {
            Time.timeScale = 1;
            flightControllerScript.gameModeScript.BackToMainMenu();
        }
        //close regular screen
        else
        {
            Time.timeScale = 1;
            if (UnityEngine.Device.Application.isMobilePlatform)
                TurnOnTouchScreenButtons();
            flightControllerScript.tutorialManagerScript.currentTutorialState = TutorialManager.TutorialPlayerState.Flying;
            GameObject.Destroy(tutorialPlaceToSpawnScreens.transform.Find("TutorialScreen").gameObject);
            tutorialScreenEnabled = false;
            playerOneUI.regularHUDMainGameObject.SetActive(true);
            tutorialMainGameObject.SetActive(false);
            flightControllerScript.uiManagerScript.playerOneUI.regularHUDMainGameObject.SetActive(true);
            UpdateLevelProgressBar(flightControllerScript.gameModeScript.playerOnePlane);
        }
        flightControllerScript.tutorialManagerScript.SpawnTutorialObstacles();
        flightControllerScript.tutorialManagerScript.RemoveOldLevelObstacles();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
