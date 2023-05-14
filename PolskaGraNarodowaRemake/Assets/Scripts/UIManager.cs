using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public GameplaySettings gameplaySettings;
    internal FlightController flightControllerScript;
    internal bool pauseScreenEnabled;
    [Header("Regular HUD")]
    [SerializeField] internal GameObject regularHUDMainGameObject;
    [SerializeField] private GameObject regularHUDYearPlayerOneGameObject;
    [SerializeField] private GameObject regularHUDLevelProgressPlayerOneGameObject;
    [SerializeField] private GameObject regularHUDScorePlayerOneGameObject;
    [SerializeField] private GameObject regularHUDBottlesPlayerOneGameObject;
    [SerializeField] private GameObject regularHUDYearPlayerTwoGameObject;
    [SerializeField] private GameObject regularHUDLevelProgressPlayerTwoGameObject;
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
    [Header("Options Menu")]
    [SerializeField] private GameObject optionsMenuGameObject;
    [Header("TimerCircle")]
    [SerializeField] internal GameObject mainTimerCircleGameObject;
    private float timerCircleTimeCounter;
    [Header("ActiveBottleWarning")]
    [SerializeField] private GameObject activeBottleWarningMainGameObject;
    [SerializeField] private GameObject activeBottleWarningPlayerOneGameObject;
    [SerializeField] private GameObject activeBottleWarningPlayerTwoGameObject;
    private bool bottleWarningPlayerOne;
    private bool bottleWarningPlayerTwo;
    private float bottleWarningTimeCounter;
    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverScreenButtonsGameObject;

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
            UpdateRegularHUD(flightControllerScript.gameModeScript.playerOnePlane, regularHUDYearPlayerOneGameObject, regularHUDBottlesPlayerOneGameObject, regularHUDLevelProgressPlayerOneGameObject, regularHUDScorePlayerOneGameObject);
            if(flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
                UpdateRegularHUD(flightControllerScript.gameModeScript.playerTwoPlane, regularHUDYearPlayerTwoGameObject, regularHUDBottlesPlayerTwoGameObject, regularHUDLevelProgressPlayerTwoGameObject, regularHUDScorePlayerTwoGameObject);
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
                bottleWarningPlayerOne = flightControllerScript.gameModeScript.playerOnePlane.attackKeyPressed;
                if(flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
                    bottleWarningPlayerTwo = flightControllerScript.gameModeScript.playerTwoPlane.attackKeyPressed;
            }
            else if (pauseScreenEnabled && pauseScreenGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed)
                DisablePauseScreen();
        }
        if (mainTimerCircleGameObject.activeSelf)
            UpdateTimer();
        if (activeBottleWarningMainGameObject)
            UpdateBottleWarning();
    }
    private void MoveUiElementsSinglePlayer()
    {
        gameSummaryPlayerOneIndicator.SetActive(false);
        gameSummaryPlayerTwoIndicator.SetActive(false);
        gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position = new Vector3(gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position.x + 75, gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position.y, gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position.z);
        gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position = new Vector3(gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.x + 75, gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.y, gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.z);
        gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position = new Vector3(gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position.x + 75, gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position.y, gameSummaryYearPlayerOneGameObject.GetComponent<RectTransform>().position.z);
        gameSummaryBottlesTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryBottlesTitle.GetComponent<RectTransform>().position.x + 75, gameSummaryBottlesTitle.GetComponent<RectTransform>().position.y, gameSummaryBottlesTitle.GetComponent<RectTransform>().position.z);
        gameSummaryScoreTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryScoreTitle.GetComponent<RectTransform>().position.x + 75, gameSummaryScoreTitle.GetComponent<RectTransform>().position.y, gameSummaryScoreTitle.GetComponent<RectTransform>().position.z);
        gameSummaryYearTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryYearTitle.GetComponent<RectTransform>().position.x + 75, gameSummaryYearTitle.GetComponent<RectTransform>().position.y, gameSummaryYearTitle.GetComponent<RectTransform>().position.z);
    }
    internal void TurnOnTheTimer(float time)
    {
        mainTimerCircleGameObject.SetActive(true);
        ResetTimer(time);
    }
    internal void TurnOffTheTimer()
    {
        flightControllerScript = GetComponent<FlightController>();
        mainTimerCircleGameObject.SetActive(false);
        if(flightControllerScript.gameModeScript.someoneWon)
        {
            SetTheTextOnTheColorPanel(colorPanelPlayerOneGameObject, "");
            if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
                SetTheTextOnTheColorPanel(colorPanelPlayerTwoGameObject, "");
            EnableGameOverScreen();
        }
        else if(flightControllerScript.rewardAndProgressionManagerScript.toNewLevel)
        {
            flightControllerScript.levelManagerScript.LoadLevel();
        }
        else
        {
            Time.timeScale = 1;
            if (activeBottleWarningMainGameObject.activeSelf)
            {
                activeBottleWarningMainGameObject.SetActive(false);
                //hack
                flightControllerScript.gameModeScript.playerOnePlane.attackKeyPressed = Input.GetButton("Jump");
                if(flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
                    flightControllerScript.gameModeScript.playerTwoPlane.attackKeyPressed = Input.GetButton("Jump1");
                //endOfHack
                if (!flightControllerScript.gameModeScript.playerOnePlane.attackKeyPressed && bottleWarningPlayerOne)
                {
                    flightControllerScript.ThrowBottleOfVodka(flightControllerScript.gameModeScript.playerOnePlane, true);
                    bottleWarningPlayerOne = false;
                }
                if ((flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && !flightControllerScript.gameModeScript.playerTwoPlane.attackKeyPressed && bottleWarningPlayerTwo)
                {
                    flightControllerScript.ThrowBottleOfVodka(flightControllerScript.gameModeScript.playerTwoPlane, true);
                    bottleWarningPlayerTwo = false;
                }
            }
            regularHUDMainGameObject.SetActive(true);
            flightControllerScript.audioManagerScript.ResumeAllPausedSounds();
            pauseScreenEnabled = false;
        }
    }
    internal void TurnOnBottleWarning()
    {
        activeBottleWarningMainGameObject.SetActive(true);
        if(bottleWarningPlayerOne)
        {
            activeBottleWarningPlayerOneGameObject.SetActive(true);
            activeBottleWarningPlayerOneGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].activeBottleWarning;
        }
        if(bottleWarningPlayerTwo)
        {
            activeBottleWarningPlayerTwoGameObject.SetActive(true);
            activeBottleWarningPlayerTwoGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].activeBottleWarning;
        }
    }
    internal void UpdateBottleWarning()
    {
        bottleWarningTimeCounter += Time.unscaledDeltaTime;
        if (bottleWarningTimeCounter > 0.5f)
        {
            bottleWarningTimeCounter = 0;
            if (bottleWarningPlayerOne)
                activeBottleWarningPlayerOneGameObject.SetActive(!activeBottleWarningPlayerOneGameObject.activeSelf);
            if (bottleWarningPlayerTwo)
                activeBottleWarningPlayerTwoGameObject.SetActive(!activeBottleWarningPlayerTwoGameObject.activeSelf);
        }
    }
    private void UpdateTimer()
    {
        timerCircleTimeCounter -= Time.unscaledDeltaTime;
        //mainTimerCircleGameObject.transform.Find("Text").GetComponent<Text>().text = (Mathf.Round(timerCircleTimeCounter * 100) / 100).ToString();
        mainTimerCircleGameObject.transform.Find("Text").GetComponent<Text>().text = ((int)timerCircleTimeCounter).ToString();
        mainTimerCircleGameObject.transform.Find("Fill").GetComponent<Image>().fillAmount = Mathf.InverseLerp(0, 1, timerCircleTimeCounter - (int)timerCircleTimeCounter);
        if (timerCircleTimeCounter <= 0)
        {
            timerCircleTimeCounter = 0;
            TurnOffTheTimer();
        }
    }
    private void ResetTimer(float time)
    {
        timerCircleTimeCounter = time;
    }
    private void UpdatePauseScreenHUD()
    {
        gameSummaryBottlesTitle.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenBottlesTitle;
        gameSummaryScoreTitle.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned0;
        gameSummaryYearTitle.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenYearTitle;
        gameSummaryBottlesPlayerOneGameObject.GetComponent<Text>().text = (flightControllerScript.gameModeScript.playerOnePlane.bottleDrunkCounter + flightControllerScript.rewardAndProgressionManagerScript.totalBottlesDrunkPlayerOne).ToString();
        gameSummaryScorePlayerOneGameObject.GetComponent<Text>().text = (flightControllerScript.gameModeScript.playerOnePlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
        gameSummaryYearPlayerOneGameObject.GetComponent<Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
        pasueScreenOptionsButtonGameObject.transform.Find("Text").GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenButton0;
        pasueScreenResumeGameButtonGameObject.transform.Find("Text").GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenButton1;
        pasueScreenBackToMainMenuButtonGameObject.transform.Find("Text").GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenuButton;
        if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
        {
            gameSummaryPlayerOneIndicator.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
            gameSummaryPlayerTwoIndicator.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
            gameSummaryBottlesPlayerTwoGameObject.GetComponent<Text>().text = (flightControllerScript.gameModeScript.playerTwoPlane.bottleDrunkCounter + flightControllerScript.rewardAndProgressionManagerScript.totalBottlesDrunkPlayerTwo).ToString();
            gameSummaryScorePlayerTwoGameObject.GetComponent<Text>().text = (flightControllerScript.gameModeScript.playerTwoPlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
            gameSummaryYearPlayerTwoGameObject.GetComponent<Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
        }
    }
    private void UpdateRegularHUD(Plane plane, GameObject regularHUDYearGameObject, GameObject regularHUDBottlesGameObject, GameObject regularHUDLevelProgressGameObject, GameObject regularHUDScoreGameObject)
    {
        //current year
        regularHUDYearGameObject.GetComponent<Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudYear + (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter)).ToString();
        //level progress
        if (plane.currentPlaneState == PlaneState.standard || plane.currentPlaneState == PlaneState.wheelsOn)
        {
            if(plane == flightControllerScript.gameModeScript.playerOnePlane)
            {
                if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter < flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace)
                {
                    int levelProgress = (int)(flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter / (flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) * 100);
                    regularHUDLevelProgressGameObject.GetComponent<Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression0 + levelProgress + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression1).ToString();
                }
                else
                    regularHUDLevelProgressGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudLandingMessage;
            }
            else
            {
                if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter < flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace)
                {
                    int levelProgress = (int)(flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter / (flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) * 100);
                    regularHUDLevelProgressGameObject.GetComponent<Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression0 + levelProgress + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression1).ToString();
                }
                else
                    regularHUDLevelProgressGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudLandingMessage;
            }
        }
        if (plane.currentPlaneState == PlaneState.damaged)
            regularHUDLevelProgressGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudPlaneHit;
        else if (plane.currentPlaneState == PlaneState.crashed)
            regularHUDLevelProgressGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudPlaneDestroyed;
        //bottles
        regularHUDBottlesGameObject.GetComponent<Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudBottle + plane.bottleDrunkCounter).ToString();
        //score
        regularHUDScoreGameObject.GetComponent<Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned0 + plane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
    }
    private void EnablePauseScreen()
    {
        Time.timeScale = 0;
        pauseScreenTitleGameObject.GetComponentInChildren<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenPauseMainTitle;
        fadePanelGameObject.SetActive(true);
        pauseScreenGameObject.SetActive(true);
        regularHUDMainGameObject.SetActive(false);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.SFX);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.oneLinersSounds);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.hitReactionSounds);
        flightControllerScript.audioManagerScript.PausePlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.landingSounds);
        pauseScreenEnabled = true;
        UpdatePauseScreenHUD();
    }
    internal void EnableGameOverScreen()
    {
        UpdatePauseScreenHUD();
        TurnOffColorPanel(colorPanelPlayerOneGameObject);
        TurnOffColorPanel(colorPanelPlayerTwoGameObject);
        pauseScreenTitleGameObject.GetComponentInChildren<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenGameOverMainTitle;
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
        if (bottleWarningPlayerOne || bottleWarningPlayerTwo)
            TurnOnBottleWarning();
        TurnOnTheTimer(3f);
    }
    public void EnableExitWarning()
    {
        gameStatsGameObject.SetActive(false);
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
        pauseScreenTitleGameObject.GetComponentInChildren<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenButton0;
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
        pauseScreenTitleGameObject.GetComponentInChildren<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenPauseMainTitle;
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
        colorPanel.transform.Find("Text").gameObject.GetComponent<Text>().text = text;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
