using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    internal PlaneBase planeBaseScript;
    public GameObject planeGameObject;
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
    [SerializeField] private GameObject gameOverScreenButtonsGameObject;
    [SerializeField] private GameObject pauseScreenWarningGameObject;
    [SerializeField] private GameObject pauseScreenTitleGameObject;
    [Header("Game Statistics")]
    [SerializeField] private GameObject gameStatsGameObject;
    [SerializeField] private GameObject gameSummaryYearGameObject;
    [SerializeField] private GameObject gameSummaryScoreGameObject;
    [SerializeField] private GameObject gameSummaryBottlesGameObject;

    void Start()
    {
        planeBaseScript = planeGameObject.GetComponent<PlaneBase>();
        pauseScreenEnabled = false;
    }
    void Update()
    {
        if (!pauseScreenEnabled)
            UpdateRegularHUD();
        else
            UpdatePauseScreenHUD();
        if (pauseScreenWarningGameObject.activeSelf && planeBaseScript.inputScript.ESCpressed)
            DisableExitWarning();
        else if (planeBaseScript.currentPlaneState != PlaneBase.StateMachine.crashed && planeBaseScript.inputScript.ESCpressed)
        {
            if (!pauseScreenEnabled)
                EnablePauseScreen();
            else
                DisablePauseScreen();
        }
    }

    private void UpdatePauseScreenHUD()
    {
        gameSummaryBottlesGameObject.GetComponent<Text>().text = ("Wszystkie wypite butelki: " + planeBaseScript.flightControllScript.drunkBottlesInTotal).ToString();
        gameSummaryYearGameObject.GetComponent<Text>().text = ("Dolecia³eœ do roku: " + (2009 + planeBaseScript.levelManagerScript.levelCounter)).ToString();
        gameSummaryScoreGameObject.GetComponent<Text>().text = ("Zarobi³eœ: " + planeBaseScript.levelManagerScript.gameScore + " z³").ToString();
    }
    private void UpdateRegularHUD()
    {
        //current year
        regularHUDYearGameObject.GetComponent<Text>().text = ("Rok: " + (2009 + planeBaseScript.levelManagerScript.levelCounter)).ToString();
        //level progress
        if (planeBaseScript.currentPlaneState == PlaneBase.StateMachine.standard || planeBaseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
        {
            if (planeBaseScript.levelManagerScript.levelProgress < planeBaseScript.levelManagerScript.currentlevelDistance)
            {
                int levelProgress = (int)(planeBaseScript.levelManagerScript.levelProgress / planeBaseScript.levelManagerScript.currentlevelDistance * 100);
                regularHUDLevelProgressGameObject.GetComponent<Text>().text = ("Panie Prezydencie, przelecieliœmy ju¿: " + levelProgress + "% trasy. Jakoœ to bêdzie!").ToString();
            }
            else if (planeBaseScript.levelManagerScript.levelProgress >= planeBaseScript.levelManagerScript.currentlevelDistance)
                regularHUDLevelProgressGameObject.GetComponent<Text>().text = "Panie Prezydencie, l¹dujemy!";
        }
        else if (planeBaseScript.currentPlaneState == PlaneBase.StateMachine.damaged)
            regularHUDLevelProgressGameObject.GetComponent<Text>().text = "Panie Prezydencie, obawiam siê, ¿e siê rozpierdolimy";
        else if (planeBaseScript.currentPlaneState == PlaneBase.StateMachine.crashed)
            regularHUDLevelProgressGameObject.GetComponent<Text>().text = "Niestety, kolejny prezydent zostanie bohaterem";
        //bottles
        if (planeBaseScript.difficultyScript.difficultyMultiplier == 0)
            regularHUDBottlesGameObject.GetComponent<Text>().text = "Wypite butelki: nic";
        else
            regularHUDBottlesGameObject.GetComponent<Text>().text = ("Wypite butelki: " + planeBaseScript.difficultyScript.difficultyMultiplier).ToString();
        //score
        regularHUDScoreGameObject.GetComponent<Text>().text = ("Zarobi³eœ: " + planeBaseScript.levelManagerScript.gameScore + " z³").ToString();
    }
    private void EnablePauseScreen()
    {
        Time.timeScale = 0;
        pauseScreenTitleGameObject.GetComponentInChildren<Text>().text = "PAUZA";
        fadePanelGameObject.SetActive(true);
        pauseScreenGameObject.SetActive(true);
        regularHUDMainGameObject.SetActive(false);
        planeBaseScript.audioScript.PausePlayingAllSounds();
        pauseScreenEnabled = true;
    }
    internal void EnableGameOverScreen()
    {
        pauseScreenTitleGameObject.GetComponentInChildren<Text>().text = "KONIEC GRY";
        pauseScreenRegularButtonsGameObject.SetActive(false);
        gameOverScreenButtonsGameObject.SetActive(true);
        regularHUDMainGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(true);
        pauseScreenEnabled = true;
    }
    public void DisablePauseScreen()
    {
        Time.timeScale = 1;
        fadePanelGameObject.SetActive(false);
        pauseScreenGameObject.SetActive(false);
        regularHUDMainGameObject.SetActive(true);
        planeBaseScript.audioScript.ResumeAllPausedSounds();
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
