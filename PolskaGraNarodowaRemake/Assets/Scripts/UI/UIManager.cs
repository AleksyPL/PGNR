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
    [Header("PowerUpsBar")]
    [SerializeField] internal GameObject powerUpBarPlayerOneParentGameObject;
    [SerializeField] internal GameObject powerUpBarPlayerTwoParentGameObject;
    [Header("Options Menu")]
    [SerializeField] private GameObject optionsMenuGameObject;
    [Header("TimerCircle")]
    [SerializeField] private GameObject timerPrefab;
    [SerializeField] private float pauseScreenTimerDuration;
    [Header("ActiveBottleWarning")]
    [SerializeField] private GameObject activeBottleWarningPrefab;
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
        gameSummaryBottlesPlayerOneGameObject.GetComponent<TMP_Text>().text = (flightControllerScript.gameModeScript.playerOnePlane.bottleDrunkCounter + flightControllerScript.rewardAndProgressionManagerScript.totalBottlesDrunkPlayerOne).ToString();
        gameSummaryScorePlayerOneGameObject.GetComponent<TMP_Text>().text = (flightControllerScript.gameModeScript.playerOnePlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
        gameSummaryYearPlayerOneGameObject.GetComponent<TMP_Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
        pasueScreenOptionsButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton2;
        pasueScreenResumeGameButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenButton0;
        pasueScreenBackToMainMenuButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenuButton;
        if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
        {
            gameSummaryPlayerOneIndicator.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
            gameSummaryPlayerTwoIndicator.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
            gameSummaryBottlesPlayerTwoGameObject.GetComponent<TMP_Text>().text = (flightControllerScript.gameModeScript.playerTwoPlane.bottleDrunkCounter + flightControllerScript.rewardAndProgressionManagerScript.totalBottlesDrunkPlayerTwo).ToString();
            gameSummaryScorePlayerTwoGameObject.GetComponent<TMP_Text>().text = (flightControllerScript.gameModeScript.playerTwoPlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
            gameSummaryYearPlayerTwoGameObject.GetComponent<TMP_Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
        }
    }
    private void UpdateRegularHUD(Plane plane, GameObject regularHUDYearGameObject, GameObject regularHUDBottlesGameObject, GameObject regularHUDLevelProgressGameObject, GameObject regularHUDScoreGameObject)
    {
        //current year
        regularHUDYearGameObject.GetComponent<TMP_Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudYear + (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter)).ToString();
        //level progress
        if (plane.currentPlaneState == PlaneState.standard || plane.currentPlaneState == PlaneState.wheelsOn)
        {
            if(plane == flightControllerScript.gameModeScript.playerOnePlane)
            {
                if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter < flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace)
                {
                    int levelProgress = (int)(flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter / (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) * 100);
                    regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression0 + levelProgress + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression1).ToString();
                }
                else
                    regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudLandingMessage;
            }
            else
            {
                if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter < flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace)
                {
                    int levelProgress = (int)(flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter / (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) * 100);
                    regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression0 + levelProgress + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression1).ToString();
                }
                else
                    regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudLandingMessage;
            }
        }
        if (plane.currentPlaneState == PlaneState.damaged)
            regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudPlaneHit;
        //bottles
        regularHUDBottlesGameObject.GetComponent<TMP_Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudBottle + plane.bottleDrunkCounter).ToString();
        //score
        regularHUDScoreGameObject.GetComponent<TMP_Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned0 + plane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
    }
    private void EnablePauseScreen()
    {
        Time.timeScale = 0;
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenPauseMainTitle;
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
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenGameOverMainTitle;
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
    public void SpawnTimerOnTheScreen(float timeToCount)
    {
        GameObject timer = Instantiate(timerPrefab, UIElements.transform);
        timer.transform.name = "CircleTimer";
        timer.GetComponent<UIClock>().TurnOnTheTimer(timeToCount);
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
    internal void DisplayPowerUpDescriptionOnHUD(GameObject planeObject, string[] messageToDisplay)
    {
        if (flightControllerScript.gameModeScript.ReturnAPlaneObject(planeObject.gameObject).playerNumber == 0)
        {
            flightControllerScript.gameModeScript.flightControllerScript.uiManagerScript.regularHUDLevelProgressPlayerOneGameObject.GetComponent<TMP_Text>().text = messageToDisplay[gameplaySettings.langauageIndex];
            ChangeTheOrderOnThePowerUpsBar(powerUpBarPlayerOneParentGameObject);
        }
        else
        {
            flightControllerScript.gameModeScript.flightControllerScript.uiManagerScript.regularHUDLevelProgressPlayerTwoGameObject.GetComponent<TMP_Text>().text = messageToDisplay[gameplaySettings.langauageIndex];
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
    public void QuitGame()
    {
        Application.Quit();
    }
}
