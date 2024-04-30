using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAndProgressionManager : MonoBehaviour
{
    internal class PlayerProgress
    {
        internal float levelProgressCounter;
        internal float scorePointsCounter;
        internal PlayerProgress()
        {
            levelProgressCounter = 0;
            scorePointsCounter = 0;
        }
    };
    internal PlayerProgress playerOneProgress;
    internal PlayerProgress playerTwoProgress;
    internal FlightController flightControllerScript;
    internal int levelCounter;
    internal float currentLevelDistance;
    internal float levelSafeSpace;
    internal float levelLandingSpace;
    internal bool toNewLevel;
    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        playerOneProgress = new PlayerProgress();
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
            playerTwoProgress = new PlayerProgress();
        currentLevelDistance = 100;
        levelSafeSpace = 10;
        levelLandingSpace = 30;
        toNewLevel = false;
        RestartGame();
    }
    internal ref PlayerProgress ReturnPlayerProgressObject(Plane plane)
    {
        if (plane == flightControllerScript.gameModeScript.playerOnePlane)
            return ref playerOneProgress;
        return ref playerTwoProgress;
    }
    private void CalculateScore(Plane plane)
    {
        if (plane.currentPlaneSpeed > 0 && (plane.currentPlaneState == PlaneState.standard || plane.currentPlaneState == PlaneState.wheelsOn))
        {
            ReturnPlayerProgressObject(plane).scorePointsCounter += Time.deltaTime;
            if (ReturnPlayerProgressObject(plane).scorePointsCounter > 1)
            {
                ReturnPlayerProgressObject(plane).scorePointsCounter = 0;
                plane.gameScore += flightControllerScript.gameplaySettings.rewardPerSecond;
            }
        }
        flightControllerScript.uiManagerScript.UpdateScoreCounter(plane);
    }
    private void CalculateLevelProgress(Plane plane)
    {
        if(flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic)
        {
            if (plane.currentPlaneSpeed > 0 && plane.currentPlaneState == PlaneState.standard)
            {
                if (ReturnPlayerProgressObject(plane).levelProgressCounter < currentLevelDistance + levelSafeSpace)
                    ReturnPlayerProgressObject(plane).levelProgressCounter += plane.currentPlaneSpeed * Time.deltaTime;
                if (ReturnPlayerProgressObject(plane).levelProgressCounter >= currentLevelDistance + levelSafeSpace)
                {
                    ReturnPlayerProgressObject(plane).levelProgressCounter = currentLevelDistance + levelSafeSpace;
                    plane.currentPlaneState = PlaneState.wheelsOn;
                    plane.planeRendererScript.ChangePlaneSprite(PlaneState.wheelsOn);
                }
            }
        }
        else
            ReturnPlayerProgressObject(plane).levelProgressCounter += plane.currentPlaneSpeed * Time.deltaTime;
        flightControllerScript.uiManagerScript.UpdateLevelProgressBar(plane);
    }
    public void RestartGame()
    {
        flightControllerScript = GetComponent<FlightController>();
        levelCounter = 1;
        playerOneProgress.levelProgressCounter = 0;
        playerOneProgress.scorePointsCounter = 0;
        flightControllerScript.gameModeScript.someoneWon = false;
        flightControllerScript.gameModeScript.playerOnePlane.gameScore = 0;
        flightControllerScript.gameModeScript.playerOnePlane.ResetPlaneData();
        flightControllerScript.uiManagerScript.TurnOffColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject);
        flightControllerScript.uiManagerScript.UpdateScoreCounter(flightControllerScript.gameModeScript.playerOnePlane);
        flightControllerScript.uiManagerScript.UpdateBottlesCounter(flightControllerScript.gameModeScript.playerOnePlane);
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            flightControllerScript.gameModeScript.playerTwoPlane.gameScore = 0;
            flightControllerScript.gameModeScript.playerTwoPlane.ResetPlaneData();
            playerTwoProgress.levelProgressCounter = 0;
            playerTwoProgress.scorePointsCounter = 0;
            flightControllerScript.uiManagerScript.TurnOffColorPanel(flightControllerScript.uiManagerScript.playerTwoUI.colorPanelGameObject);
            flightControllerScript.uiManagerScript.UpdateScoreCounter(flightControllerScript.gameModeScript.playerTwoPlane);
            flightControllerScript.uiManagerScript.UpdateBottlesCounter(flightControllerScript.gameModeScript.playerTwoPlane);
        }
        flightControllerScript.powerUpManagerScript.ResetPowerUpManager();
        flightControllerScript.uiManagerScript.DisableOptionsMenu();
        flightControllerScript.uiManagerScript.DisableGameOverScreen();
        flightControllerScript.levelManagerScript.numberOfObstacles = 3;
        flightControllerScript.levelManagerScript.numberOfFogInstances = 0;
        flightControllerScript.gameModeScript.waitingTimeForOneLinerCurrent = 0;
        flightControllerScript.audioManagerScript.StopPlayingAllPausedSounds();
        flightControllerScript.powerUpManagerScript.ResetPowerUpManager();
        flightControllerScript.levelManagerScript.LoadLevel();
    }
    void Update()
    {
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless)
        {
            CalculateScore(flightControllerScript.gameModeScript.playerOnePlane);
            CalculateLevelProgress(flightControllerScript.gameModeScript.playerOnePlane);
        }
        else if(flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.tutorial && flightControllerScript.tutorialManagerScript.currentState == TutorialManager.TutorialPlayerState.Flying)
        {
            CalculateScore(flightControllerScript.gameModeScript.playerOnePlane);
        }
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            CalculateScore(flightControllerScript.gameModeScript.playerTwoPlane);
            CalculateLevelProgress(flightControllerScript.gameModeScript.playerTwoPlane);
        }
    }
}
