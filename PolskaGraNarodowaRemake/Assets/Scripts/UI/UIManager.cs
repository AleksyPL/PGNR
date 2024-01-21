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
    public GameObject EventSystemGameObject;
    internal FlightController flightControllerScript;
    internal bool pauseScreenEnabled;
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
    [SerializeField] private GameObject pauseScreenBackToMainMenuButtonGameObject;
    [SerializeField] private GameObject pauseScreenResumeGameButtonGameObject;
    [Header("Game Statistics")]
    [SerializeField] private GameObject gameStatsGameObject;
    [SerializeField] private GameObject gameSummaryYearTitle;
    [SerializeField] private GameObject gameSummaryBottlesTitle;
    [SerializeField] private GameObject gameSummaryScoreTitle;
    [Header("PowerUps")]
    [SerializeField] private GameObject powerUpUIClockPrefab;
    [Header("Options Menu")]
    [SerializeField] private GameObject optionsMenuGameObject;
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

    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        EventSystemGameObject.GetComponent<EventSystem>().SetSelectedGameObject(null);
        pauseScreenEnabled = false;
        optionsMenuGameObject.GetComponent<UIOptionsMenu>().DisableLanguageButtons();
        //if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless)
        //    MoveUiElementsSinglePlayer();
    }
    void Update()
    {
        if (optionsMenuGameObject.activeSelf)
            flightControllerScript.audioManagerScript.UpdateAllSoundsVolume();
        if (!pauseScreenEnabled)
        {
            UpdateRegularHUD(flightControllerScript.gameModeScript.playerOnePlane, playerOneUI.regularHUDBottlesGameObject, playerOneUI.regularHUDLevelProgressGameObject, playerOneUI.regularHUDScoreGameObject);
            if(flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
                UpdateRegularHUD(flightControllerScript.gameModeScript.playerTwoPlane, playerTwoUI.regularHUDBottlesGameObject, playerTwoUI.regularHUDLevelProgressGameObject, playerTwoUI.regularHUDScoreGameObject);
        }
        if (pauseScreenWarningGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed)
            DisableExitWarning();
        else if (optionsMenuGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed)
            DisableOptionsMenu();
        else if (((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless) && flightControllerScript.gameModeScript.playerOneState != GameModeManager.PlayerState.crashed) || ((flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && flightControllerScript.gameModeScript.playerOneState != GameModeManager.PlayerState.crashed && flightControllerScript.gameModeScript.playerTwoState != GameModeManager.PlayerState.crashed))
        {
            if (!pauseScreenEnabled && !pauseScreenGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed && !timerBeforeTheFlightEnabled)
            {
                EnablePauseScreen();
                flightControllerScript.gameModeScript.playerOnePlane.activeBottleWarning = flightControllerScript.gameModeScript.playerOnePlane.attackKeyPressed;
                if(flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
                    flightControllerScript.gameModeScript.playerTwoPlane.activeBottleWarning = flightControllerScript.gameModeScript.playerTwoPlane.attackKeyPressed;
            }
            else if (pauseScreenEnabled && pauseScreenGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed)
                DisablePauseScreen();
        }
    }
    //private void MoveUiElementsSinglePlayer()
    //{
    //    playerOneUI.gameSummaryPlayerIndicator.SetActive(false);
    //    playerTwoUI.gameSummaryPlayerIndicator.SetActive(false);
    //    playerOneUI.regularHUDMainGameObject.SetActive(false);
    //    if (playerTwoUI != null)
    //        playerTwoUI.regularHUDMainGameObject.SetActive(false);
    //    powerUpBarPlayerOneParentGameObject.GetComponent<RectTransform>().position = new Vector3(powerUpBarPlayerOneParentGameObject.GetComponent<RectTransform>().position.x, powerUpBarPlayerOneParentGameObject.GetComponent<RectTransform>().position.y - 230f, powerUpBarPlayerOneParentGameObject.GetComponent<RectTransform>().position.z);
    //    powerUpBarPlayerOneParentGameObject.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
    //    playerOneUI.gameSummaryBottlesGameObject.GetComponent<RectTransform>().position = new Vector3(playerOneUI.gameSummaryBottlesGameObject.GetComponent<RectTransform>().position.x + 75, playerOneUI.gameSummaryBottlesGameObject.GetComponent<RectTransform>().position.y, playerOneUI.gameSummaryBottlesGameObject.GetComponent<RectTransform>().position.z);
    //    gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position = new Vector3(gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.x + 75, gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.y, gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.z);
    //    gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position = new Vector3(gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position.x + 75, gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position.y, gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position.z);
    //    gameSummaryBottlesTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryBottlesTitle.GetComponent<RectTransform>().position.x + 75, gameSummaryBottlesTitle.GetComponent<RectTransform>().position.y, gameSummaryBottlesTitle.GetComponent<RectTransform>().position.z);
    //    gameSummaryScoreTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryScoreTitle.GetComponent<RectTransform>().position.x + 75, gameSummaryScoreTitle.GetComponent<RectTransform>().position.y, gameSummaryScoreTitle.GetComponent<RectTransform>().position.z);
    //    gameSummaryYearTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryYearTitle.GetComponent<RectTransform>().position.x + 75, gameSummaryYearTitle.GetComponent<RectTransform>().position.y, gameSummaryYearTitle.GetComponent<RectTransform>().position.z);
    //}
    private void UpdatePauseScreenHUD()
    {
        gameSummaryBottlesTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenBottlesTitle;
        gameSummaryScoreTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned0;
        gameSummaryYearTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenYearTitle;
        playerOneUI.gameSummaryBottlesGameObject.GetComponent<TMP_Text>().text = flightControllerScript.gameModeScript.playerOnePlane.bottlesDrunkTotal.ToString();
        playerOneUI.gameSummaryScoreGameObject.GetComponent<TMP_Text>().text = (flightControllerScript.gameModeScript.playerOnePlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
        playerOneUI.gameSummaryYearGameObject.GetComponent<TMP_Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
        pauseScreenOptionsButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton2;
        pauseScreenResumeGameButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenButton0;
        pauseScreenBackToMainMenuButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenuButton;
        if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
        {
            playerOneUI.gameSummaryPlayerIndicator.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
            playerTwoUI.gameSummaryPlayerIndicator.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
            playerTwoUI.gameSummaryBottlesGameObject.GetComponent<TMP_Text>().text = flightControllerScript.gameModeScript.playerTwoPlane.bottlesDrunkTotal.ToString();
            playerTwoUI.gameSummaryScoreGameObject.GetComponent<TMP_Text>().text = (flightControllerScript.gameModeScript.playerTwoPlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
            playerTwoUI.gameSummaryYearGameObject.GetComponent<TMP_Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
        }
    }
    private void UpdateRegularHUD(Plane plane, GameObject regularHUDBottlesGameObject, GameObject regularHUDLevelProgressGameObject, GameObject regularHUDScoreGameObject)
    {
        //level progress
        if ((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic) && !timerBeforeTheFlightEnabled && plane.currentPlaneState == PlaneState.standard || plane.currentPlaneState == PlaneState.wheelsOn)
        {
            if(plane == flightControllerScript.gameModeScript.playerOnePlane)
            {
                if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter < flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace)
                {
                    int levelProgress = (int)(flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter / (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) * 100);
                    regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression0 + levelProgress + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression1).ToString();
                }
                else if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter > (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) && plane.currentPlaneSpeed > 0)
                    regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudLandingMessage;
                else if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter > (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) && plane.currentPlaneSpeed == 0 && flightControllerScript.gameModeScript.playerOneState == GameModeManager.PlayerState.landed)
                    regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudCongratulationsAfterLanding;
            }
            else
            {
                if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter < flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace)
                {
                    int levelProgress = (int)(flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter / (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) * 100);
                    regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression0 + levelProgress + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression1).ToString();
                }
                else if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter > (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) && plane.currentPlaneSpeed > 0)
                    regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudLandingMessage;
                else if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter > (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) && plane.currentPlaneSpeed == 0 && flightControllerScript.gameModeScript.playerTwoState == GameModeManager.PlayerState.landed)
                    regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudCongratulationsAfterLanding;
            }
        }
        else
            DisablePowerUpMessage();
        if (plane.currentPlaneState == PlaneState.damaged)
            regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudPlaneHit;
        //bottles
        if ((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless) && (plane.bottlesDrunk != plane.bottlesDrunkTotal))
            regularHUDBottlesGameObject.GetComponent<TMP_Text>().text = plane.bottlesDrunk.ToString() + " (" + plane.bottlesDrunkTotal.ToString() + ")";
        else
            regularHUDBottlesGameObject.GetComponent<TMP_Text>().text = plane.bottlesDrunk.ToString();
        //score
        regularHUDScoreGameObject.GetComponent<TMP_Text>().text = plane.gameScore.ToString();
    }
    private void EnablePauseScreen()
    {
        if (((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless) && flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed) || ((flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && (flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed || flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState != PlaneState.crashed)))
        {
            Time.timeScale = 0;
            EventSystemGameObject.GetComponent<EventSystem>().SetSelectedGameObject(pauseScreenOptionsButtonGameObject);
            pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenPauseMainTitle;
            fadePanelGameObject.SetActive(true);
            pauseScreenGameObject.SetActive(true);
            playerOneUI.regularHUDMainGameObject.SetActive(false);
            if(playerTwoUI.regularHUDMainGameObject != null)
                playerTwoUI.regularHUDMainGameObject.SetActive(false);
            flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localSFX);
            flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localOneLinersSounds);
            flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localHitReactionSounds);
            flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localLandingSounds);
            pauseScreenEnabled = true;
            UpdatePauseScreenHUD();
        }
    }
    internal void EnableGameOverScreen()
    {
        UpdatePauseScreenHUD();
        EventSystemGameObject.GetComponent<EventSystem>().SetSelectedGameObject(gameOverScreenTryAgainButtonGameObject);
        TurnOffColorPanel(playerOneUI.colorPanelGameObject);
        TurnOffColorPanel(playerTwoUI.colorPanelGameObject);
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenGameOverMainTitle;
        gameOverScreenTryAgainButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameOverScreenButton0;
        gameOverScreenExitButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenuButton;
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
        gameOverScreenButtonsGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(false);
        pauseScreenEnabled = false;
    }
    public void DisablePauseScreen()
    {
        
        fadePanelGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(false);
        if (flightControllerScript.gameModeScript.playerOnePlane.activeBottleWarning || flightControllerScript.gameModeScript.playerTwoPlane.activeBottleWarning)
        {
            GameObject waring = Instantiate(activeBottleWarningPrefab, UIElements.transform);
            waring.transform.name = "ActiveBottleWarning";
        }  
        SpawnTimerOnTheScreen(pauseScreenTimerDuration);
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
            playerOneUI.regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = message;
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
        EventSystemGameObject.GetComponent<EventSystem>().SetSelectedGameObject(exitWarningNoButtonGameObject);
        exitWarningTitleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].warningTitle;
        exitWarningYesButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].warningYes;
        exitWarningNoButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].warningNo;
        pauseScreenRegularButtonsGameObject.SetActive(false);
        pauseScreenWarningGameObject.SetActive(true);
    }
    public void DisableExitWarning()
    {
        gameStatsGameObject.SetActive(true);
        EventSystemGameObject.GetComponent<EventSystem>().SetSelectedGameObject(pauseScreenOptionsButtonGameObject);
        pauseScreenRegularButtonsGameObject.SetActive(true);
        pauseScreenWarningGameObject.SetActive(false);
    }
    public void EnableOptionsMenu()
    {
        EventSystemGameObject.GetComponent<EventSystem>().SetSelectedGameObject(optionsMenuGameObject.GetComponent<UIOptionsMenu>().backToMainMenuButton);
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenButton0;
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
        if (plane == flightControllerScript.gameModeScript.playerOnePlane)
        {
            flightControllerScript.gameModeScript.flightControllerScript.uiManagerScript.playerOneUI.regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = messageToDisplay;
            if(playerOneUI.powerUpMessageOnScreenEnabled)
                playerOneUI.powerUpMessageOnScreenCounter = 0;
            else
                playerOneUI.powerUpMessageOnScreenEnabled = true;
            ChangeTheOrderOnThePowerUpsBar(playerOneUI.powerUpBarParentGameObject);
        }
        else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
        {
            flightControllerScript.gameModeScript.flightControllerScript.uiManagerScript.playerTwoUI.regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = messageToDisplay;
            if (playerTwoUI.powerUpMessageOnScreenEnabled)
                playerTwoUI.powerUpMessageOnScreenCounter = 0;
            else
                playerTwoUI.powerUpMessageOnScreenEnabled = true;
            ChangeTheOrderOnThePowerUpsBar(playerTwoUI.powerUpBarParentGameObject);
        }
    }
    private void DisablePowerUpMessage()
    {
        if(playerOneUI.powerUpMessageOnScreenEnabled)
        {
            playerOneUI.powerUpMessageOnScreenCounter += Time.deltaTime;
            if (playerOneUI.powerUpMessageOnScreenCounter > gameplaySettings.durationTimeForPowerUpMessageOnTheScreen)
            {
                playerOneUI.regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = "";
                playerOneUI.powerUpMessageOnScreenCounter = 0;
                playerOneUI.powerUpMessageOnScreenEnabled = false;
            } 
        }
        if (playerTwoUI.powerUpMessageOnScreenEnabled)
        {
            playerTwoUI.powerUpMessageOnScreenCounter += Time.deltaTime;
            if (playerTwoUI.powerUpMessageOnScreenCounter > gameplaySettings.durationTimeForPowerUpMessageOnTheScreen)
            {
                playerTwoUI.regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = "";
                playerTwoUI.powerUpMessageOnScreenCounter = 0;
                playerTwoUI.powerUpMessageOnScreenEnabled = false;
            }
        }
    }
    internal void SpawnPowerUpUIClock(Plane plane, PowerUp currentPowerUp)
    {
        if ((plane == flightControllerScript.gameModeScript.playerOnePlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUp.powerUpName) == null) || (plane == flightControllerScript.gameModeScript.playerTwoPlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUp.powerUpName) == null))
        {
            GameObject powerUpUIGameObject = Instantiate(powerUpUIClockPrefab);
            powerUpUIGameObject.gameObject.transform.name = currentPowerUp.powerUpName;
            powerUpUIGameObject.GetComponent<Image>().sprite = currentPowerUp.currentPowerUpUIClockImage;
            if(plane == flightControllerScript.gameModeScript.playerOnePlane)
                powerUpUIGameObject.gameObject.transform.SetParent(playerOneUI.powerUpBarParentGameObject.transform);
            else if(plane == flightControllerScript.gameModeScript.playerTwoPlane)
                powerUpUIGameObject.gameObject.transform.SetParent(playerTwoUI.powerUpBarParentGameObject.transform);
            powerUpUIGameObject.GetComponent<UIPowerUp>().EnableUIPowerUp(currentPowerUp.powerUpDuration, currentPowerUp.powerUpName);
        }
    }
    internal void DeletePowerUpUIClock(Plane plane, PowerUp currentPowerUp)
    {
        if ((plane == flightControllerScript.gameModeScript.playerOnePlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUp.powerUpName) != null) || (plane == flightControllerScript.gameModeScript.playerTwoPlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUp.powerUpName) != null))
        {
            Destroy(ReturnPowerUpUIClockIfExists(plane, currentPowerUp.powerUpName));
            if (plane == flightControllerScript.gameModeScript.playerOnePlane)
                ChangeTheOrderOnThePowerUpsBar(playerOneUI.powerUpBarParentGameObject);
            else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
                ChangeTheOrderOnThePowerUpsBar(playerTwoUI.powerUpBarParentGameObject);
        }
    }
    internal void ChangeTheOrderOnThePowerUpsBar(GameObject powerUpBarGameObject)
    {
        if (powerUpBarGameObject.transform.childCount != 0)
        {
            List<GameObject> myObjectsUnsorted = new List<GameObject>();
            for (int i=0;i<powerUpBarGameObject.transform.childCount;i++)
            {
                if (powerUpBarGameObject.transform.GetChild(i).gameObject.GetComponent<UIPowerUp>().powerUpDurationCounter > 0)
                    myObjectsUnsorted.Add(powerUpBarGameObject.transform.GetChild(i).gameObject);
                else
                    Destroy(powerUpBarGameObject.transform.GetChild(i).gameObject);
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
        if (plane == flightControllerScript.gameModeScript.playerOnePlane)
        {
            if (playerOneUI.powerUpBarParentGameObject.transform.childCount != 0)
            {
                for (int i = 0; i < playerOneUI.powerUpBarParentGameObject.transform.childCount; i++)
                {
                    if (playerOneUI.powerUpBarParentGameObject.transform.GetChild(i).gameObject.name == powerUpUIClockName)
                        return playerOneUI.powerUpBarParentGameObject.transform.GetChild(i).gameObject;
                }
            }
        }
        else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
        {
            if (playerTwoUI.powerUpBarParentGameObject.transform.childCount != 0)
            {
                for (int i = 0; i < playerTwoUI.powerUpBarParentGameObject.transform.childCount; i++)
                {
                    if (playerTwoUI.powerUpBarParentGameObject.transform.GetChild(i).gameObject.name == powerUpUIClockName)
                        return playerTwoUI.powerUpBarParentGameObject.transform.GetChild(i).gameObject;
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
    public void QuitGame()
    {
        Application.Quit();
    }
}
