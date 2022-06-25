using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    internal PlaneBase planeBaseScript;
    public GameObject planeGameObject;
    private bool pauseScreenEnabled;
    [SerializeField] private GameObject pauseAndGameSummaryScreen;
    [Header("Regular HUD")]
    [SerializeField] private GameObject regularHUDMainGameObject;
    [SerializeField] private GameObject regularHUDYearGameObject;
    [SerializeField] private GameObject regularHUDLevelProgressGameObject;
    [SerializeField] private GameObject regularHUDScoreGameObject;
    [SerializeField] private GameObject regularHUDBottlesGameObject;
    [Header("Pause Screen")]
    [SerializeField] private GameObject pauseScreenGameObject;
    
    [Header("Game Summary Screen")]
    [SerializeField] private GameObject gameSummaryGameObject;
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
        if (planeBaseScript.currentPlaneState != PlaneBase.StateMachine.crashed && planeBaseScript.inputScript.ESCpressed)
        {
            if (!pauseScreenEnabled)
                EnablePauseScreen();
            else
                DisablePauseScreen();
        }
    }
    internal void UpdateRegularHUD()
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
        pauseAndGameSummaryScreen.gameObject.SetActive(true);
        regularHUDMainGameObject.gameObject.SetActive(false);
        planeBaseScript.audioScript.PausePlayingAllSounds();
        pauseScreenEnabled = true;
    }
    internal void EnableGameSummaryScreen()
    {
        regularHUDMainGameObject.gameObject.SetActive(false);
        pauseAndGameSummaryScreen.gameObject.SetActive(true);
        gameSummaryGameObject.gameObject.SetActive(true);
        pauseScreenEnabled = true;
        gameSummaryBottlesGameObject.GetComponent<Text>().text = ("Wszystkie wypite butelki: " + planeBaseScript.flightControllScript.drunkBottlesInTotal).ToString();
        gameSummaryYearGameObject.GetComponent<Text>().text = ("Dolecia³eœ do roku: " + (2009 + planeBaseScript.levelManagerScript.levelCounter)).ToString();
        gameSummaryScoreGameObject.GetComponent<Text>().text = ("Zarobi³eœ: " + planeBaseScript.levelManagerScript.gameScore + " z³").ToString();
    }
    public void DisablePauseScreen()
    {
        Time.timeScale = 1;
        pauseAndGameSummaryScreen.gameObject.SetActive(false);
        regularHUDMainGameObject.gameObject.SetActive(true);
        planeBaseScript.audioScript.ResumeAllPausedSounds();
        pauseScreenEnabled = false;
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
