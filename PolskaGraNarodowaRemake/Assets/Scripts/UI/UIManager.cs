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
    [Header("Touch Screen")]
    [SerializeField] private GameObject touchScreenMainGameObject;

    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        eventSystem = eventSystemGameObject.GetComponent<EventSystem>();
        pauseScreenEnabled = false;
        optionsMenuGameObject.GetComponent<UIOptionsMenu>().DisableLanguageButtons();
        if (Application.isMobilePlatform || flightControllerScript.gameModeScript.simulateMobileApp)
            TurnOnTouchScreenButtons();
    }
    private ref PlayerUI ReturnPlayersUIObject(Plane plane)
    {
        flightControllerScript = GetComponent<FlightController>();
        if (plane == flightControllerScript.gameModeScript.playerOnePlane)
            return ref playerOneUI;
        return ref playerTwoUI;
    }
    void Update()
    {
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
    private void UpdatePauseScreenHUD()
    {
        gameSummaryBottlesTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenBottlesTitle;
        gameSummaryScoreTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned0;
        gameSummaryYearTitle.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenLevelTitle;
        playerOneUI.gameSummaryBottlesGameObject.GetComponent<TMP_Text>().text = flightControllerScript.gameModeScript.playerOnePlane.bottlesDrunkTotal.ToString();
        playerOneUI.gameSummaryScoreGameObject.GetComponent<TMP_Text>().text = (flightControllerScript.gameModeScript.playerOnePlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
        if (flightControllerScript.gameplaySettings.safeMode)
            playerOneUI.gameSummaryYearGameObject.GetComponent<TMP_Text>().text = flightControllerScript.rewardAndProgressionManagerScript.levelCounter.ToString();
        else
            playerOneUI.gameSummaryYearGameObject.GetComponent<TMP_Text>().text = (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString();
        pauseScreenOptionsButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuOptions;
        pauseScreenResumeGameButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenResumeGame;
        pauseScreenBackToMainMenuButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            playerOneUI.gameSummaryPlayerIndicator.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
            playerTwoUI.gameSummaryPlayerIndicator.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
            playerTwoUI.gameSummaryBottlesGameObject.GetComponent<TMP_Text>().text = flightControllerScript.gameModeScript.playerTwoPlane.bottlesDrunkTotal.ToString();
            playerTwoUI.gameSummaryScoreGameObject.GetComponent<TMP_Text>().text = (flightControllerScript.gameModeScript.playerTwoPlane.gameScore + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudEarned1).ToString();
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
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = (gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression0 + levelProgress + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudProgression1).ToString();
            }
            else if (flightControllerScript.rewardAndProgressionManagerScript.ReturnPlayerProgressObject(plane).levelProgressCounter >= (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) && plane.currentPlaneSpeed > 0)
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudLandingMessage;
            else if (flightControllerScript.rewardAndProgressionManagerScript.ReturnPlayerProgressObject(plane).levelProgressCounter >= (flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) && plane.currentPlaneSpeed == 0 && flightControllerScript.gameModeScript.playerOneState == GameModeManager.PlayerState.landed)
                ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudCongratulationsAfterLanding;
        }
        if (plane.currentPlaneState == PlaneState.damaged)
            ReturnPlayersUIObject(plane).regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].regularHudPlaneHit;
    }
    internal void UpdateBottlesCounter(Plane plane)
    {
        if ((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless) && (plane.bottlesDrunk != plane.bottlesDrunkTotal))
            ReturnPlayersUIObject(plane).regularHUDBottlesGameObject.GetComponent<TMP_Text>().text = plane.bottlesDrunk.ToString() + " (" + plane.bottlesDrunkTotal.ToString() + ")";
        else
            ReturnPlayersUIObject(plane).regularHUDBottlesGameObject.GetComponent<TMP_Text>().text = plane.bottlesDrunk.ToString();
    }
    internal void UpdateScoreCounter(Plane plane)
    {
        ReturnPlayersUIObject(plane).regularHUDScoreGameObject.GetComponent<TMP_Text>().text = plane.gameScore.ToString();
    }
    private void EnablePauseScreen()
    {
        if (((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless) && flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed) || ((flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && (flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed || flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState != PlaneState.crashed)))
        {
            Time.timeScale = 0;
            if (Application.isMobilePlatform || flightControllerScript.gameModeScript.simulateMobileApp)
            {
                TurnOffTouchScreenButtons();
                flightControllerScript.inputManagerScript.ESCpressed = false;
            }
            else
                eventSystem.SetSelectedGameObject(pauseScreenResumeGameButtonGameObject);
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
        if (Application.isMobilePlatform)
            TurnOffTouchScreenButtons();
        else
            eventSystem.SetSelectedGameObject(gameOverScreenTryAgainButtonGameObject);
        UpdatePauseScreenHUD();   
        TurnOffColorPanel(playerOneUI.colorPanelGameObject);
        TurnOffColorPanel(playerTwoUI.colorPanelGameObject);
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
        if (Application.isMobilePlatform)
            TurnOnTouchScreenButtons();
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
        if(!Application.isMobilePlatform)
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
        if (!Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(pauseScreenOptionsButtonGameObject);
        pauseScreenRegularButtonsGameObject.SetActive(true);
        pauseScreenWarningGameObject.SetActive(false);
    }
    public void EnableOptionsMenu()
    {
        if (!Application.isMobilePlatform)
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
        pauseScreenGameObject.SetActive(true);
        gameStatsGameObject.SetActive(true);
        pauseScreenRegularButtonsGameObject.SetActive(true);
        pauseScreenTitleGameObject.GetComponentInChildren<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].pauseScreenPauseMainTitle;
        flightControllerScript = GetComponent<FlightController>();
        flightControllerScript.audioManagerScript.UpdateAllSoundsVolume();
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
            powerUpUIGameObject.transform.SetParent(ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform);
            powerUpUIGameObject.GetComponent<UIPowerUp>().EnableUIPowerUp(currentPowerUp.powerUpDuration, currentPowerUp.powerUpName);
        }
    }
    internal void DeletePowerUpUIClock(Plane plane, string currentPowerUpName)
    {
        if ((plane == flightControllerScript.gameModeScript.playerOnePlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUpName) != null) || (plane == flightControllerScript.gameModeScript.playerTwoPlane && ReturnPowerUpUIClockIfExists(plane, currentPowerUpName) != null))
        {
            Destroy(ReturnPowerUpUIClockIfExists(plane, currentPowerUpName));
            ChangeTheOrderOnThePowerUpsBar(ReturnPlayersUIObject(plane).powerUpBarParentGameObject);
        }
    }
    internal void ClearPowerUpBar(Plane plane)
    {
        if(ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.childCount != 0)
        {
            while (ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.childCount > 0)
                DestroyImmediate(ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.GetChild(0).gameObject);
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
        if(ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.childCount != 0)
        {
            for (int i = 0; i < ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.childCount; i++)
            {
                if (ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.GetChild(i).gameObject.name == powerUpUIClockName)
                    return ReturnPlayersUIObject(plane).powerUpBarParentGameObject.transform.GetChild(i).gameObject;
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
    public void QuitGame()
    {
        Application.Quit();
    }
}
