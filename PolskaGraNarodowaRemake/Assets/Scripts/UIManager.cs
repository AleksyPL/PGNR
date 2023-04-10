using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    public GameplaySettings gameplaySettings;
    internal FlightController flightControllerScript;
    private bool pauseScreenEnabled;
    [Header("Regular HUD")]
    [SerializeField] private GameObject regularHUDMainGameObject;
    [SerializeField] private GameObject regularHUDYearPlayerOneGameObject;
    [SerializeField] private GameObject regularHUDLevelProgressPlayerOneGameObject;
    [SerializeField] private GameObject regularHUDScorePlayerOneGameObject;
    [SerializeField] private GameObject regularHUDBottlesPlayerOneGameObject;
    [SerializeField] internal GameObject regularHUDColorPanelPlayerOneGameObject;
    [SerializeField] private GameObject regularHUDYearPlayerTwoGameObject;
    [SerializeField] private GameObject regularHUDLevelProgressPlayerTwoGameObject;
    [SerializeField] private GameObject regularHUDScorePlayerTwoGameObject;
    [SerializeField] private GameObject regularHUDBottlesPlayerTwoGameObject;
    [SerializeField] internal GameObject regularHUDColorPanelPlayerTwoGameObject;
    [SerializeField] internal Color winColor;
    [SerializeField] internal Color loseColor;
    [Header("Pause Screen")]
    [SerializeField] private GameObject pauseScreenGameObject;
    [SerializeField] private GameObject fadePanelGameObject;
    [SerializeField] private GameObject pauseScreenRegularButtonsGameObject;
    [SerializeField] private GameObject pauseScreenWarningGameObject;
    [SerializeField] private GameObject pauseScreenTitleGameObject;
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
    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverScreenButtonsGameObject;

    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        pauseScreenEnabled = false;
        if(flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayer)
        {
            regularHUDYearPlayerTwoGameObject = null;
            regularHUDLevelProgressPlayerTwoGameObject = null;
            regularHUDScorePlayerTwoGameObject = null;
            regularHUDBottlesPlayerTwoGameObject = null;
        }
    }
    void Update()
    {
        if (!pauseScreenEnabled)
        {
            UpdateRegularHUD(flightControllerScript.gameModeScript.playerOnePlane, regularHUDYearPlayerOneGameObject, regularHUDBottlesPlayerOneGameObject, regularHUDLevelProgressPlayerOneGameObject, regularHUDScorePlayerOneGameObject);
            if(flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayer)
                UpdateRegularHUD(flightControllerScript.gameModeScript.playerTwoPlane, regularHUDYearPlayerTwoGameObject, regularHUDBottlesPlayerTwoGameObject, regularHUDLevelProgressPlayerTwoGameObject, regularHUDScorePlayerTwoGameObject);
        }
        if (pauseScreenWarningGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed)
            DisableExitWarning();
        else if (optionsMenuGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed)
            DisableOptionsMenu();
        else if (flightControllerScript.gameModeScript.playerOneState != GameModeManager.PlayerState.crashed || (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayer && (flightControllerScript.gameModeScript.playerOneState != GameModeManager.PlayerState.crashed || flightControllerScript.gameModeScript.playerTwoState != GameModeManager.PlayerState.crashed)))
        {
            if (!pauseScreenEnabled && !pauseScreenGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed)
                EnablePauseScreen();
            else if (pauseScreenEnabled && pauseScreenGameObject.activeSelf && flightControllerScript.inputManagerScript.ESCpressed)
                DisablePauseScreen();
        }
    }

    private void UpdatePauseScreenHUD()
    {
        gameSummaryBottlesTitle.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenBottlesTitle;
        gameSummaryScoreTitle.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned0;
        gameSummaryYearTitle.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenYearTitle;
        gameSummaryBottlesPlayerOneGameObject.GetComponent<Text>().text = (flightControllerScript.gameModeScript.playerOnePlane.bottleDrunkCounter + flightControllerScript.rewardAndProgressionManagerScript.totalBottlesDrunkPlayerOne).ToString();
        gameSummaryScorePlayerOneGameObject.GetComponent<Text>().text = (flightControllerScript.gameModeScript.playerOnePlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
        gameSummaryYearPlayerOneGameObject.GetComponent<Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayer)
        {
            gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position = new Vector3(gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position.x + 125, gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position.y, gameSummaryBottlesPlayerOneGameObject.GetComponent<RectTransform>().position.z);
            gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position = new Vector3(gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.x + 125, gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.y, gameSummaryScorePlayerOneGameObject.GetComponent<RectTransform>().position.z);
            gameSummaryYearPlayerTwoGameObject.GetComponent<RectTransform>().position = new Vector3(gameSummaryYearPlayerTwoGameObject.GetComponent<RectTransform>().position.x + 125, gameSummaryYearPlayerTwoGameObject.GetComponent<RectTransform>().position.y, gameSummaryYearPlayerTwoGameObject.GetComponent<RectTransform>().position.z);
            gameSummaryBottlesTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryBottlesTitle.GetComponent<RectTransform>().position.x + 125, gameSummaryBottlesTitle.GetComponent<RectTransform>().position.y, gameSummaryBottlesTitle.GetComponent<RectTransform>().position.z);
            gameSummaryScoreTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryScoreTitle.GetComponent<RectTransform>().position.x + 125, gameSummaryScoreTitle.GetComponent<RectTransform>().position.y, gameSummaryScoreTitle.GetComponent<RectTransform>().position.z);
            gameSummaryYearTitle.GetComponent<RectTransform>().position = new Vector3(gameSummaryYearTitle.GetComponent<RectTransform>().position.x + 125, gameSummaryYearTitle.GetComponent<RectTransform>().position.y, gameSummaryYearTitle.GetComponent<RectTransform>().position.z);
        }
        else
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
                if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter < flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance)
                {
                    int levelProgress = (int)(flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter / flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance * 100);
                    regularHUDLevelProgressGameObject.GetComponent<Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression0 + levelProgress + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression1).ToString();
                }
                else
                    regularHUDLevelProgressGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudLandingMessage;
            }
            else
            {
                if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter < flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance)
                {
                    int levelProgress = (int)(flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter / flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance * 100);
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
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayer)
        {
            gameSummaryPlayerOneIndicator.SetActive(false);
            gameSummaryPlayerTwoIndicator.SetActive(false);
        }
        else
        {

        }
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
        Time.timeScale = 1;
        fadePanelGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(false);
        regularHUDMainGameObject.SetActive(true);
        flightControllerScript.audioManagerScript.ResumeAllPausedSounds();
        pauseScreenEnabled = false;
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
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
