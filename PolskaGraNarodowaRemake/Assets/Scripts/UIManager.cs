using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    internal FlightController flightControllerScript;
    //internal PlaneScript PlaneScriptScript;
    //public GameObject planeControlCenterGameObject;
    private bool pauseScreenEnabled;
    [Header("Regular HUD")]
    [SerializeField] private GameObject regularHUDMainGameObject;
    [SerializeField] private GameObject regularHUDYearPlayerOneGameObject;
    [SerializeField] private GameObject regularHUDLevelProgressPlayerOneGameObject;
    [SerializeField] private GameObject regularHUDScorePlayerOneGameObject;
    [SerializeField] private GameObject regularHUDBottlesPlayerOneGameObject;
    [SerializeField] private GameObject regularHUDYearPlayerTwoGameObject;
    [SerializeField] private GameObject regularHUDLevelProgressPlayerTwoGameObject;
    [SerializeField] private GameObject regularHUDScorePlayerTwoGameObject;
    [SerializeField] private GameObject regularHUDBottlesPlayerTwoGameObject;
    [Header("Pause Screen")]
    [SerializeField] private GameObject pauseScreenGameObject;
    [SerializeField] private GameObject fadePanelGameObject;
    [SerializeField] private GameObject pauseScreenRegularButtonsGameObject;
    [SerializeField] private GameObject pauseScreenWarningGameObject;
    [SerializeField] private GameObject pauseScreenTitleGameObject;
    [Header("Game Statistics")]
    [SerializeField] private GameObject gameStatsGameObject;
    [SerializeField] private GameObject gameSummaryYearGameObject;
    [SerializeField] private GameObject gameSummaryScoreGameObject;
    [SerializeField] private GameObject gameSummaryBottlesGameObject;
    [Header("Options Menu")]
    [SerializeField] private GameObject optionsMenuGameObject;
    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverScreenButtonsGameObject;

    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        //PlaneScriptScript = planeControlCenterGameObject.GetComponent<PlaneScript>();
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
        else if (flightControllerScript.gameModeScript.currentPlaythrough != GameModeManager.Playthrough.finished)
        {
            if (!pauseScreenEnabled)
                EnablePauseScreen();
            else
                DisablePauseScreen();
        }
    }

    private void UpdatePauseScreenHUD()
    {
        if(flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayer)
        {

        }
        else
        {

        }
        //gameSummaryBottlesGameObject.GetComponent<Text>().text = ("Wszystkie wypite butelki: " + PlaneScriptScript.flightControllScript.drunkBottlesInTotal).ToString();
        //gameSummaryYearGameObject.GetComponent<Text>().text = ("Dolecia³eœ do roku: " + (2009 + PlaneScriptScript.levelManagerScript.levelCounter)).ToString();
        //gameSummaryScoreGameObject.GetComponent<Text>().text = ("Zarobi³eœ: " + PlaneScriptScript.levelManagerScript.gameScore + " z³").ToString();
    }
    private void UpdateRegularHUD(Plane plane, GameObject regularHUDYearGameObject, GameObject regularHUDBottlesGameObject, GameObject regularHUDLevelProgressGameObject, GameObject regularHUDScoreGameObject)
    {
        //current year
        regularHUDYearGameObject.GetComponent<Text>().text = ("Rok: " + (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter)).ToString();
        //level progress
        if (plane.currentPlaneState == PlaneState.standard || plane.currentPlaneState == PlaneState.wheelsOn)
        {
            if(plane == flightControllerScript.gameModeScript.playerOnePlane)
            {
                if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter < flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance)
                {
                    int levelProgress = (int)(flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter / flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance * 100);
                    regularHUDLevelProgressGameObject.GetComponent<Text>().text = ("Panie Prezydencie, przelecieliœmy ju¿: " + levelProgress + "% trasy. Jakoœ to bêdzie!").ToString();
                }
                else
                    regularHUDLevelProgressGameObject.GetComponent<Text>().text = "Panie Prezydencie, l¹dujemy!";
            }
            else
            {
                if (flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter < flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance)
                {
                    int levelProgress = (int)(flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter / flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance * 100);
                    regularHUDLevelProgressGameObject.GetComponent<Text>().text = ("Panie Prezydencie, przelecieliœmy ju¿: " + levelProgress + "% trasy. Jakoœ to bêdzie!").ToString();
                }
                else
                    regularHUDLevelProgressGameObject.GetComponent<Text>().text = "Panie Prezydencie, l¹dujemy!";
            }
        }
        if (plane.currentPlaneState == PlaneState.damaged)
            regularHUDLevelProgressGameObject.GetComponent<Text>().text = "Panie Prezydencie, obawiam siê, ¿e siê rozpierdolimy";
        else if (plane.currentPlaneState == PlaneState.crashed)
            regularHUDLevelProgressGameObject.GetComponent<Text>().text = "Niestety, kolejny prezydent zostanie bohaterem";
        //bottles
        if (plane.bottleDrunkCounter == 0)
            regularHUDBottlesGameObject.GetComponent<Text>().text = "Wypite butelki: nic";
        else
            regularHUDBottlesGameObject.GetComponent<Text>().text = ("Wypite butelki: " + plane.bottleDrunkCounter).ToString();
        //score
        regularHUDScoreGameObject.GetComponent<Text>().text = ("Zarobi³eœ: " + plane.gameScore + " z³").ToString();
    }
    private void EnablePauseScreen()
    {
        UpdatePauseScreenHUD();
        Time.timeScale = 0;
        pauseScreenTitleGameObject.GetComponentInChildren<Text>().text = "PAUZA";
        fadePanelGameObject.SetActive(true);
        pauseScreenGameObject.SetActive(true);
        regularHUDMainGameObject.SetActive(false);
        //PlaneScriptScript.audioScript.PausePlayingSoundsFromTheSpecificSoundBank(PlaneScriptScript.audioScript.SFX);
        //PlaneScriptScript.audioScript.PausePlayingSoundsFromTheSpecificSoundBank(PlaneScriptScript.audioScript.oneLinersSounds);
        //PlaneScriptScript.audioScript.PausePlayingSoundsFromTheSpecificSoundBank(PlaneScriptScript.audioScript.hitReactionSounds);
        //PlaneScriptScript.audioScript.PausePlayingSoundsFromTheSpecificSoundBank(PlaneScriptScript.audioScript.landingSounds);
        pauseScreenEnabled = true;
    }
    internal void EnableGameOverScreen()
    {
        UpdatePauseScreenHUD();
        pauseScreenTitleGameObject.GetComponentInChildren<Text>().text = "KONIEC GRY";
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
        //PlaneScriptScript.audioScript.ResumeAllPausedSounds();
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
        pauseScreenTitleGameObject.GetComponentInChildren<Text>().text = "OPCJE";
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
        pauseScreenTitleGameObject.GetComponentInChildren<Text>().text = "PAUZA";
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
