using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    //internal PlaneScript PlaneScriptScript;
    public GameObject planeControlCenterGameObject;
    private bool pauseScreenEnabled;
    [Header("Regular HUD")]
    [SerializeField] private GameObject regularHUDMainGameObject;
    [SerializeField] private GameObject regularHUDYearGameObject;
    [SerializeField] private GameObject regularHUDLevelProgressGameObject;
    [SerializeField] private GameObject regularHUDScoreGameObject;
    [SerializeField] private GameObject regularHUDBottlesGameObject;
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
        //PlaneScriptScript = planeControlCenterGameObject.GetComponent<PlaneScript>();
        pauseScreenEnabled = false;
    }
    void Update()
    {
        //if (!pauseScreenEnabled)
        //    UpdateRegularHUD();
        //if (pauseScreenWarningGameObject.activeSelf && PlaneScriptScript.inputScript.ESCpressed)
        //    DisableExitWarning();
        //else if (optionsMenuGameObject.activeSelf && PlaneScriptScript.inputScript.ESCpressed)
        //    DisableOptionsMenu();
        //else if (PlaneScriptScript.currentPlaneState != PlaneScript.StateMachine.crashed && PlaneScriptScript.inputScript.ESCpressed)
        //{
        //    if (!pauseScreenEnabled)
        //        EnablePauseScreen();
        //    else
        //        DisablePauseScreen();
        //}
    }

    private void UpdatePauseScreenHUD()
    {
        //gameSummaryBottlesGameObject.GetComponent<Text>().text = ("Wszystkie wypite butelki: " + PlaneScriptScript.flightControllScript.drunkBottlesInTotal).ToString();
        //gameSummaryYearGameObject.GetComponent<Text>().text = ("Dolecia³eœ do roku: " + (2009 + PlaneScriptScript.levelManagerScript.levelCounter)).ToString();
        //gameSummaryScoreGameObject.GetComponent<Text>().text = ("Zarobi³eœ: " + PlaneScriptScript.levelManagerScript.gameScore + " z³").ToString();
    }
    private void UpdateRegularHUD()
    {
        ////current year
        //regularHUDYearGameObject.GetComponent<Text>().text = ("Rok: " + (2009 + PlaneScriptScript.levelManagerScript.levelCounter)).ToString();
        ////level progress
        //if (PlaneScriptScript.currentPlaneState == PlaneScript.StateMachine.standard || PlaneScriptScript.currentPlaneState == PlaneScript.StateMachine.wheelsOn)
        //{
        //    if (PlaneScriptScript.levelManagerScript.levelProgress < PlaneScriptScript.levelManagerScript.currentlevelDistance)
        //    {
        //        int levelProgress = (int)(PlaneScriptScript.levelManagerScript.levelProgress / PlaneScriptScript.levelManagerScript.currentlevelDistance * 100);
        //        regularHUDLevelProgressGameObject.GetComponent<Text>().text = ("Panie Prezydencie, przelecieliœmy ju¿: " + levelProgress + "% trasy. Jakoœ to bêdzie!").ToString();
        //    }
        //    else if (PlaneScriptScript.levelManagerScript.levelProgress >= PlaneScriptScript.levelManagerScript.currentlevelDistance)
        //        regularHUDLevelProgressGameObject.GetComponent<Text>().text = "Panie Prezydencie, l¹dujemy!";
        //}
        //else if (PlaneScriptScript.currentPlaneState == PlaneScript.StateMachine.damaged)
        //    regularHUDLevelProgressGameObject.GetComponent<Text>().text = "Panie Prezydencie, obawiam siê, ¿e siê rozpierdolimy";
        //else if (PlaneScriptScript.currentPlaneState == PlaneScript.StateMachine.crashed)
        //    regularHUDLevelProgressGameObject.GetComponent<Text>().text = "Niestety, kolejny prezydent zostanie bohaterem";
        ////bottles
        //if (PlaneScriptScript.difficultyScript.difficultyMultiplier == 0)
        //    regularHUDBottlesGameObject.GetComponent<Text>().text = "Wypite butelki: nic";
        //else
        //    regularHUDBottlesGameObject.GetComponent<Text>().text = ("Wypite butelki: " + PlaneScriptScript.difficultyScript.difficultyMultiplier).ToString();
        ////score
        //regularHUDScoreGameObject.GetComponent<Text>().text = ("Zarobi³eœ: " + PlaneScriptScript.levelManagerScript.gameScore + " z³").ToString();
    }
    private void EnablePauseScreen()
    {
        //UpdatePauseScreenHUD();
        //Time.timeScale = 0;
        //pauseScreenTitleGameObject.GetComponentInChildren<Text>().text = "PAUZA";
        //fadePanelGameObject.SetActive(true);
        //pauseScreenGameObject.SetActive(true);
        //regularHUDMainGameObject.SetActive(false);
        //PlaneScriptScript.audioScript.PausePlayingSoundsFromTheSpecificSoundBank(PlaneScriptScript.audioScript.SFX);
        //PlaneScriptScript.audioScript.PausePlayingSoundsFromTheSpecificSoundBank(PlaneScriptScript.audioScript.oneLinersSounds);
        //PlaneScriptScript.audioScript.PausePlayingSoundsFromTheSpecificSoundBank(PlaneScriptScript.audioScript.hitReactionSounds);
        //PlaneScriptScript.audioScript.PausePlayingSoundsFromTheSpecificSoundBank(PlaneScriptScript.audioScript.landingSounds);
        //pauseScreenEnabled = true;
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
        //Time.timeScale = 1;
        //fadePanelGameObject.SetActive(false);
        //pauseScreenGameObject.SetActive(false);
        //regularHUDMainGameObject.SetActive(true);
        //PlaneScriptScript.audioScript.ResumeAllPausedSounds();
        //pauseScreenEnabled = false;
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
