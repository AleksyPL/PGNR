using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIClock : MonoBehaviour
{
    internal FlightController flightControllerScript;
    private TMP_Text textGameObject;
    private Image imageGameObject;
    private float timerCircleTimeCounter;

    void Start()
    {
        flightControllerScript = GetComponentInParent<FlightController>();
        textGameObject = transform.Find("Text").GetComponent<TMP_Text>();
        imageGameObject = transform.Find("Fill").GetComponent<Image>();
    }
    void Update()
    {
        UpdateTimer();
    }
    internal void TurnOnTheTimer(float time)
    {
        timerCircleTimeCounter = time;
    }
    private void TurnOffTheTimer()
    {
        if (flightControllerScript.gameModeScript.someoneWon)
        {
            flightControllerScript.uiManagerScript.SetTheTextOnTheColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject, "");
            if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
                flightControllerScript.uiManagerScript.SetTheTextOnTheColorPanel(flightControllerScript.uiManagerScript.playerTwoUI.colorPanelGameObject, "");
            flightControllerScript.uiManagerScript.EnableGameOverScreen();
        }
        else if (flightControllerScript.rewardAndProgressionManagerScript.toNewLevel)
        {
            flightControllerScript.levelManagerScript.LoadLevel();
        }
        else if (flightControllerScript.uiManagerScript.pauseScreenEnabled)
        {
             Time.timeScale = 1;
            if (transform.parent.Find("ActiveBottleWarning") != null)
            {
                transform.parent.Find("ActiveBottleWarning").GetComponent<UIActiveBottleWarning>().DestroyBottleWarning();
                //Destroy(transform.parent.Find("ActiveBottleWarning"));
                //hack
                flightControllerScript.gameModeScript.playerOnePlane.attackKeyPressed = Input.GetButton("Jump");
                if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
                    flightControllerScript.gameModeScript.playerTwoPlane.attackKeyPressed = Input.GetButton("Jump1");
                //endOfHack
                if (!flightControllerScript.gameModeScript.playerOnePlane.attackKeyPressed && flightControllerScript.gameModeScript.playerOnePlane.activeBottleWarning)
                {
                    flightControllerScript.ThrowBottleOfVodka(flightControllerScript.gameModeScript.playerOnePlane, true);
                    flightControllerScript.gameModeScript.playerOnePlane.activeBottleWarning = false;
                }
                if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless && !flightControllerScript.gameModeScript.playerTwoPlane.attackKeyPressed && flightControllerScript.gameModeScript.playerTwoPlane.activeBottleWarning)
                {
                    flightControllerScript.ThrowBottleOfVodka(flightControllerScript.gameModeScript.playerTwoPlane, true);
                    flightControllerScript.gameModeScript.playerTwoPlane.activeBottleWarning = false;
                }
            }
            if (Application.isMobilePlatform || flightControllerScript.gameModeScript.simulateMobileApp)
                flightControllerScript.uiManagerScript.TurnOnTouchScreenButtons();
            flightControllerScript.uiManagerScript.playerOneUI.regularHUDMainGameObject.SetActive(true);
            if (flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject != null)
                flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject.SetActive(true);
            flightControllerScript.audioManagerScript.ResumeAllPausedSounds();
            flightControllerScript.uiManagerScript.pauseScreenEnabled = false;
        }
        else
        {
            Time.timeScale = 1;
            flightControllerScript.uiManagerScript.timerBeforeTheFlightEnabled = false;
            flightControllerScript.uiManagerScript.playerOneUI.regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = "";
            flightControllerScript.uiManagerScript.playerOneUI.regularHUDMainGameObject.SetActive(true);
            if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
            {
                flightControllerScript.uiManagerScript.playerTwoUI.regularHUDLevelProgressGameObject.GetComponent<TMP_Text>().text = "";
                flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject.SetActive(true);
            }     
            flightControllerScript.audioManagerScript.ResumeAllPausedSounds();
        }
        Destroy(transform.gameObject);
    }
    private void UpdateTimer()
    {
        timerCircleTimeCounter -= Time.unscaledDeltaTime;
        //mainTimerCircleGameObject.transform.Find("Text").GetComponent<Text>().text = (Mathf.Round(timerCircleTimeCounter * 100) / 100).ToString();
        textGameObject.text = ((int)timerCircleTimeCounter).ToString();
        imageGameObject.fillAmount = Mathf.InverseLerp(0, 1, timerCircleTimeCounter - (int)timerCircleTimeCounter);
        if (timerCircleTimeCounter <= 0)
        {
            timerCircleTimeCounter = 0;
            TurnOffTheTimer();
        }
    }
}
