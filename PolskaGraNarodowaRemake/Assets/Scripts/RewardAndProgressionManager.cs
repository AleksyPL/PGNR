using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAndProgressionManager : MonoBehaviour
{
    internal FlightController flightControllerScript;
    internal int levelCounter;
    internal float currentLevelDistance;
    internal float levelSafeSpace;
    internal float levelLandingSpace;
    internal float levelProgressPlayerOneCounter;
    internal float levelProgressPlayerTwoCounter;
    private float scorePointsCounterPlayerOneCounter;
    private float scorePointsCounterPlayerTwoCounter;
    internal bool toNewLevel;
    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        currentLevelDistance = 100;
        levelSafeSpace = 10;
        levelLandingSpace = 30;
        toNewLevel = false;
        RestartGame();
    }
    private void CalculateScore(Plane plane, ref float counter)
    {
        if (plane.currentPlaneSpeed > 0 && (plane.currentPlaneState == PlaneState.standard || plane.currentPlaneState == PlaneState.wheelsOn))
        {
            counter += Time.deltaTime;
            if (counter > 1)
            {
                counter = 0;
                plane.gameScore += flightControllerScript.gameplaySettings.rewardPerSecond;
            }
        }
    }
    private void CalculateLevelProgress(Plane plane, ref float counter)
    {
        if(flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic)
        {
            if (plane.currentPlaneSpeed > 0 && plane.currentPlaneState == PlaneState.standard)
            {
                if (counter < currentLevelDistance + levelSafeSpace)
                    counter += plane.currentPlaneSpeed * Time.deltaTime;
                if (counter >= currentLevelDistance + levelSafeSpace)
                {
                    counter = currentLevelDistance + levelSafeSpace;
                    plane.currentPlaneState = PlaneState.wheelsOn;
                    plane.planeRendererScript.ChangePlaneSprite(PlaneState.wheelsOn);
                }
            }
        }
        else
            counter += plane.currentPlaneSpeed * Time.deltaTime;
    }
    public void RestartGame()
    {
        levelCounter = 1;
        levelProgressPlayerOneCounter = 0;
        scorePointsCounterPlayerOneCounter = 0;
        flightControllerScript.gameModeScript.someoneWon = false;
        flightControllerScript.gameModeScript.playerOnePlane.gameScore = 0;
        flightControllerScript.gameModeScript.playerOnePlane.ResetPlaneData();
        flightControllerScript.uiManagerScript.TurnOffColorPanel(flightControllerScript.uiManagerScript.colorPanelPlayerOneGameObject);
        if(flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
        {
            flightControllerScript.gameModeScript.playerTwoPlane.gameScore = 0;
            flightControllerScript.gameModeScript.playerTwoPlane.ResetPlaneData();
            levelProgressPlayerTwoCounter = 0;
            scorePointsCounterPlayerTwoCounter = 0;
            flightControllerScript.uiManagerScript.TurnOffColorPanel(flightControllerScript.uiManagerScript.colorPanelPlayerTwoGameObject);
        }
        flightControllerScript.uiManagerScript.DisableOptionsMenu();
        flightControllerScript.uiManagerScript.DisableGameOverScreen();
        //flightControllerScript.uiManagerScript.DisablePauseScreen();
        //flightControllerScript.uiManagerScript.TurnOffTheTimer();
        flightControllerScript.levelManagerScript.numberOfObstacles = 3;
        flightControllerScript.levelManagerScript.numberOfFogInstances = 0;
        flightControllerScript.levelManagerScript.LoadLevel();
    }
    void Update()
    {
        //if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic)
        //{
        //    CalculateScore(flightControllerScript.gameModeScript.playerOnePlane, ref scorePointsCounterPlayerOneCounter);
        //    CalculateLevelProgress(flightControllerScript.gameModeScript.playerOnePlane, ref levelProgressPlayerOneCounter);
        //}
        //else if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless)
        //    CalculateScore(flightControllerScript.gameModeScript.playerOnePlane, ref scorePointsCounterPlayerOneCounter);
        //else if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic)
        //{
        //    CalculateScore(flightControllerScript.gameModeScript.playerOnePlane, ref scorePointsCounterPlayerOneCounter);
        //    CalculateLevelProgress(flightControllerScript.gameModeScript.playerOnePlane, ref levelProgressPlayerOneCounter);
        //    CalculateScore(flightControllerScript.gameModeScript.playerTwoPlane, ref scorePointsCounterPlayerTwoCounter);
        //    CalculateLevelProgress(flightControllerScript.gameModeScript.playerTwoPlane, ref levelProgressPlayerTwoCounter);
        //}
        //else if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        //{
        //    CalculateScore(flightControllerScript.gameModeScript.playerOnePlane, ref scorePointsCounterPlayerOneCounter);
        //    CalculateScore(flightControllerScript.gameModeScript.playerTwoPlane, ref scorePointsCounterPlayerTwoCounter);
        //}
        CalculateScore(flightControllerScript.gameModeScript.playerOnePlane, ref scorePointsCounterPlayerOneCounter);
        CalculateLevelProgress(flightControllerScript.gameModeScript.playerOnePlane, ref levelProgressPlayerOneCounter);
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            CalculateScore(flightControllerScript.gameModeScript.playerTwoPlane, ref scorePointsCounterPlayerTwoCounter);
            CalculateLevelProgress(flightControllerScript.gameModeScript.playerTwoPlane, ref levelProgressPlayerTwoCounter);
        }
    }
}
