using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public GameplaySettings gameplaySettings;
    public GameObject UIElements;
    internal FlightController flightControllerScript;
    internal bool pauseScreenEnabled;
    internal bool timerBeforeTheFlightEnabled;
    private float powerUpMessageOnScreenPlayerOneCounter;
    private float powerUpMessageOnScreenPlayerTwoCounter;
    private bool powerUpMessageOnScreenEnabledPlayerOne;
    private bool powerUpMessageOnScreenEnabledPlayerTwo;
    [Header("Regular HUD")]
    [SerializeField] internal GameObject regularHUDMainGameObject;
    [SerializeField] private GameObject regularHUDPlayerTwoMainGameObject;
    [SerializeField] internal GameObject regularHUDLevelProgressPlayerOneGameObject;
    [SerializeField] private GameObject regularHUDScorePlayerOneGameObject;
    [SerializeField] private GameObject regularHUDBottlesPlayerOneGameObject;
    [SerializeField] internal GameObject regularHUDLevelProgressPlayerTwoGameObject;
    [SerializeField] private GameObject regularHUDScorePlayerTwoGameObject;
    [SerializeField] private GameObject regularHUDBottlesPlayerTwoGameObject;
    [Header("Color Panels")]
    [SerializeField] internal Color winColor;
    [SerializeField] internal Color loseColor;
    [SerializeField] internal GameObject colorPanelPlayerOneGameObject;
    [SerializeField] internal GameObject colorPanelPlayerTwoGameObject;
    [Header("Pause Screen")]
    [SerializeField] private GameObject fadePanelGameObject;
    [SerializeField] private GameObject pauseScreenGameObject;
    [SerializeField] private GameObject pauseScreenRegularButtonsGameObject;
    [SerializeField] private GameObject pauseScreenWarningGameObject;
    [SerializeField] private GameObject pauseScreenTitleGameObject;
    [SerializeField] private GameObject pasueScreenOptionsButtonGameObject;
    [SerializeField] private GameObject pasueScreenBackToMainMenuButtonGameObject;
    [SerializeField] private GameObject pasueScreenResumeGameButtonGameObject;
    [Header("Game Statistics")]
    [SerializeField] private GameObject gameStatsGameObject;
    [SerializeField] private GameObject gameSummaryYearTitle;
    [SerializeField] private GameObject gameSummaryBottlesTitle;
    [SerializeField] private GameObject gameSummaryScoreTitle;
    [SerializeField] private GameObject gameSummaryPlayerOneIndicator;
    [SerializeField] private GameObject gameSummaryPlayerTwoIndicator;
    [SerializeField] private GameObject gameSummaryYearPlayerOneGameObject;
    [SerializeField] private GameObject gameSummaryScorePlayerOneGameObject;
    [SerializeField] private GameObject gameSummaryBottlesPlayerOneGameObject;
    [SerializeField] private GameObject gameSummaryYearPlayerTwoGameObject;
    [SerializeField] private GameObject gameSummaryScorePlayerTwoGameObject;
    [SerializeField] private GameObject gameSummaryBottlesPlayerTwoGameObject;
    [Header("PowerUps")]
    [SerializeField] internal GameObject powerUpBarPlayerOneParentGameObject;
    [SerializeField] internal GameObject powerUpBarPlayerTwoParentGameObject;
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
        pauseScreenEnabled = false;
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless)
            MoveUiElementsSinglePlayer();
    }
    void Update()
    {
        if (optionsMenuGameObject.activeSelf)
            flightControllerScript.audioManagerScript.UpdateAllSoundsVolume();
        if (!pauseScreenEnabled)
        {
            UpdateRegularHUD(flightControllerScript.gameModeScript.playerOnePlane, regularHUDBottlesPlayerOneGameObject, regularHUDLevelProgressPlayerOneGameObject, regularHUDScorePlayerOneGameObject);
            if(flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
                UpdateRegularHUD(flightControllerScript.gameModeScript.playerTwoPlane, regularHUDBottlesPlayerTwoGameObject, regularHUDLevelProgressPlayerTwoGameObject, regularHUDScorePlayerTwoGameObject);
        }
        if (pauseScreenWarningGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed)
            DisableExitWarning();
        else if (optionsMenuGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed)
            DisableOptionsMenu();
        else if (((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless) && flightControllerScript.gameModeScript.playerOneState != GameModeManager.PlayerState.crashed) || ((flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && flightControllerScript.gameModeScript.playerOneState != GameModeManager.PlayerState.crashed && flightControllerScript.gameModeScript.playerTwoState != GameModeManager.PlayerState.crashed))
        {
            if (!pauseScreenEnabled && !pauseScreenGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed)
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
    private void MoveUiElementsSinglePlayer()
    {
        gameSummaryPlayerOneIndicator.SetActive(false);
        gameSummaryPlayerTwoIndicator.SetActive(false);
        regularHUDPlayerTwoMainGameObject.SetActive(false);
        powerUpBarPlayerOneParentGameObject.GetComponent<RectTransform>().position = new Vector3(powerUpBarPlayerOneParentGameObject.GetComponent<RectTransform>().position.x, powerUpBarPlayerOneParentGameObject.GetComponent<RectTransform>().position.y - 230f, powerUpBarPlayerOneParentGameObject.GetComponent<RectTransform>().position.z);
        powerUpBarPlayerOneParentGameObject.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
        gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position = new Vector3(gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position.x + 75, gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position.y, gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position.z);
        gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position = new Vector3(gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.x + 75, gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.y, gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.z);
        gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position = new Vector3(gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position.x + 75, gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position.y, gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position.z);
        gameSummaryBottlesTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryBottlesTitle.GetComponent<RectTransform>().position.x + 75, gameSummaryBottlesTitle.GetComponent<RectTransform>().position.y, gameSummaryBottlesTitle.GetComponent<RectTransform>().position.z);
        gameSummaryScoreTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryScoreTitle.GetComponent<RectTransform>().position.x + 75, gameSummaryScoreTitle.GetComponent<RectTransform>().position.y, gameSummaryScoreTitle.GetComponent<RectTransform>().position.z);
        gameSummaryYearTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryYearTitle.GetComponent<RectTransform>().position.x + 75, gameSummaryYearTitle.GetComponent<RectTransform>().position.y, gameSummaryYearTitle.GetComponent<RectTransform>().position.z);
    }
    private void UpdatePauseScreenHUD()
    {
        gameSummaryBottlesTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenBottlesTitle;
        gameSummaryScoreTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned0;
        gameSummaryYearTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenYearTitle;
        gameSummaryBottlesPlayerOneGameObject.GetComponent<TMP_Text>().text = flightControllerScript.gameModeScript.playerOnePlane.bottlesDrunkTotal.ToString();
        gameSummaryScorePlayerOneGameObject.GetComponent<TMP_Text>().text = (flightControllerScript.gameModeScript.playerOnePlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
        gameSummaryYearPlayerOneGameObject.GetComponent<TMP_Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
        pasueScreenOptionsButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton2;
        pasueScreenResumeGameButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenButton0;
        pasueScreenBackToMainMenuButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenuButton;
        if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
        {
            gameSummaryPlayerOneIndicator.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
            gameSummaryPlayerTwoIndicator.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
            gameSummaryBottlesPlayerTwoGameObject.GetComponent<TMP_Text>().text = flightControllerScript.gameModeScript.playerTwoPlane.bottlesDrunkTotal.ToString();
            gameSummaryScorePlayerTwoGameObject.GetComponent<TMP_Text>().text = (flightControllerScript.gameModeScript.playerTwoPlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
            gameSummaryYearPlayerTwoGameObject.GetComponent<TMP_Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
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
        Time.timeScale = 0;
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenPauseMainTitle;
        fadePanelGameObject.SetActive(true);
        pauseScreenGameObject.SetActive(true);
        regularHUDMainGameObject.SetActive(false);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localSFX);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localOneLinersSounds);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localHitReactionSounds);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localLandingSounds);
        pauseScreenEnabled = true;
        UpdatePauseScreenHUD();
    }
    internal void EnableGameOverScreen()
    {
        UpdatePauseScreenHUD();
        TurnOffColorPanel(colorPanelPlayerOneGameObject);
        TurnOffColorPanel(colorPanelPlayerTwoGameObject);
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenGameOverMainTitle;
        gameOverScreenTryAgainButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].gameOverScreenButton0;
        gameOverScreenExitButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenuButton;
        pauseScreenRegularButtonsGameObject.SetActive(false);
        gameOverScreenButtonsGameObject.SetActive(true);
        regularHUDMainGameObject.SetActive(false);
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
        regularHUDLevelProgressPlayerOneGameObject.GetComponent<TMP_Text>().text = message;
        GameObject timer = SpawnTimerOnTheScreen(timeToCount);
        if(flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            timer.GetComponent<RectTransform>().position = new Vector3(timer.GetComponent<RectTransform>().position.x, timer.GetComponent<RectTransform>().position.y + 100f, 0);
            timer.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            timer = SpawnTimerOnTheScreen(timeToCount);
            timer.GetComponent<RectTransform>().position = new Vector3(timer.GetComponent<RectTransform>().position.x, timer.GetComponent<RectTransform>().position.y - 150f, 0);
            timer.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            regularHUDLevelProgressPlayerTwoGameObject.GetComponent<TMP_Text>().text = message;
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
        pauseScreenWarningGameObject.SetActive(false);
    }
    public void EnableOptionsMenu()
    {
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenButton0;
        optionsMenuGameObject.SetActive(true);
        pauseScreenRegularButtonsGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(false);
        regularHUDMainGameObject.SetActive(false);
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
            flightControllerScript.gameModeScript.flightControllerScript.uiManagerScript.regularHUDLevelProgressPlayerOneGameObject.GetComponent<TMP_Text>().text = messageToDisplay;
            if(powerUpMessageOnScreenEnabledPlayerOne)
                powerUpMessageOnScreenPlayerOneCounter = 0;
            else
                powerUpMessageOnScreenEnabledPlayerOne = true;
            ChangeTheOrderOnThePowerUpsBar(powerUpBarPlayerOneParentGameObject);
        }
        else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
        {
            flightControllerScript.gameModeScript.flightControllerScript.uiManagerScript.regularHUDLevelProgressPlayerTwoGameObject.GetComponent<TMP_Text>().text = messageToDisplay;
            if (powerUpMessageOnScreenEnabledPlayerTwo)
                powerUpMessageOnScreenPlayerTwoCounter = 0;
            else
                powerUpMessageOnScreenEnabledPlayerTwo = true;
            ChangeTheOrderOnThePowerUpsBar(powerUpBarPlayerTwoParentGameObject);
        }
    }
    private void DisablePowerUpMessage()
    {
        if(powerUpMessageOnScreenEnabledPlayerOne)
        {
            powerUpMessageOnScreenPlayerOneCounter += Time.deltaTime;
            if (powerUpMessageOnScreenPlayerOneCounter > gameplaySettings.durationTimeForPowerUpMessageOnTheScreen)
            {
                regularHUDLevelProgressPlayerOneGameObject.GetComponent<TMP_Text>().text = "";
                powerUpMessageOnScreenPlayerOneCounter = 0;
                powerUpMessageOnScreenEnabledPlayerOne = false;
            } 
        }
        if (powerUpMessageOnScreenEnabledPlayerTwo)
        {
            powerUpMessageOnScreenPlayerTwoCounter += Time.deltaTime;
            if (powerUpMessageOnScreenPlayerTwoCounter > gameplaySettings.durationTimeForPowerUpMessageOnTheScreen)
            {
                regularHUDLevelProgressPlayerTwoGameObject.GetComponent<TMP_Text>().text = "";
                powerUpMessageOnScreenPlayerTwoCounter = 0;
                powerUpMessageOnScreenEnabledPlayerTwo = false;
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
                powerUpUIGameObject.gameObject.transform.SetParent(powerUpBarPlayerOneParentGameObject.transform);
            else if(plane == flightControllerScript.gameModeScript.playerTwoPlane)
                powerUpUIGameObject.gameObject.transform.SetParent(powerUpBarPlayerTwoParentGameObject.transform);
            powerUpUIGameObject.GetComponent<UIPowerUp>().EnableUIPowerUp(currentPowerUp.powerUpDuration, currentPowerUp.powerUpName);
        }
    }
    internal void DeletePowerUpUIClock(Plane plane, PowerUp currentPowerUp)
    {
        if ((plane == flightControllerScript.gameModeScript.playerOnePlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUp.powerUpName) != null) || (plane == flightControllerScript.gameModeScript.playerTwoPlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUp.powerUpName) != null))
        {
            Destroy(ReturnPowerUpUIClockIfExists(plane, currentPowerUp.powerUpName));
            if (plane == flightControllerScript.gameModeScript.playerOnePlane)
                ChangeTheOrderOnThePowerUpsBar(powerUpBarPlayerOneParentGameObject);
            else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
                ChangeTheOrderOnThePowerUpsBar(powerUpBarPlayerTwoParentGameObject);
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
            if (powerUpBarPlayerOneParentGameObject.transform.childCount != 0)
            {
                for (int i = 0; i < powerUpBarPlayerOneParentGameObject.transform.childCount; i++)
                {
                    if (powerUpBarPlayerOneParentGameObject.transform.GetChild(i).gameObject.name == powerUpUIClockName)
                        return powerUpBarPlayerOneParentGameObject.transform.GetChild(i).gameObject;
                }
            }
        }
        else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
        {
            if (powerUpBarPlayerTwoParentGameObject.transform.childCount != 0)
            {
                for (int i = 0; i < powerUpBarPlayerTwoParentGameObject.transform.childCount; i++)
                {
                    if (powerUpBarPlayerTwoParentGameObject.transform.GetChild(i).gameObject.name == powerUpUIClockName)
                        return powerUpBarPlayerTwoParentGameObject.transform.GetChild(i).gameObject;
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
