using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAndProgressionManager : MonoBehaviour
{
    internal FlightController flightControllerScript;
    internal int levelCounter;
    internal float currentlevelDistance;
    internal float levelProgressPlayerOneCounter;
    internal float levelProgressPlayerTwoCounter;
    private float scorePointsCounterPlayerOneCounter;
    private float scorePointsCounterPlayerTwoCounter;
    internal int totalBottlesDrunkPlayerOne;
    internal int totalBottlesDrunkPlayerTwo;
    internal int totalScorePlayerOne;
    internal int totalScorePlayerTwo;
    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        currentlevelDistance = 100 + levelCounter * 10;
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
                if(plane.playerNumber==0)
                    totalScorePlayerOne += flightControllerScript.gameplaySettings.rewardPerSecond;
                else
                    totalScorePlayerTwo += flightControllerScript.gameplaySettings.rewardPerSecond;
            }
        }
    }
    private void CalculateLevelProgress(Plane plane, ref float counter)
    {
        if (plane.currentPlaneSpeed > 0 && plane.currentPlaneState == PlaneState.standard)
        {
            if (counter < currentlevelDistance)
                counter += plane.currentPlaneSpeed * Time.deltaTime;
            if (counter >= currentlevelDistance)
            {
                counter = currentlevelDistance;
                plane.currentPlaneState = PlaneState.wheelsOn;
                plane.planeRendererScript.ChangePlaneSprite(PlaneState.wheelsOn);
            }
        }
    }
    public void RestartGame()
    {
        levelCounter = 1;
        levelProgressPlayerOneCounter = 0;
        levelProgressPlayerTwoCounter = 0;
        scorePointsCounterPlayerOneCounter = 0;
        scorePointsCounterPlayerTwoCounter = 0;
        flightControllerScript.gameModeScript.playerOnePlane.ResetPlaneData();
        flightControllerScript.gameModeScript.playerTwoPlane.ResetPlaneData();
        //PlaneScriptScript.flightControllScript.waitingTimeAfterLandingCurrent = 0;
        flightControllerScript.levelManagerScript.LoadLevel();
    }
    void Update()
    {
        CalculateScore(flightControllerScript.gameModeScript.playerOnePlane, ref scorePointsCounterPlayerOneCounter);
        CalculateScore(flightControllerScript.gameModeScript.playerTwoPlane, ref scorePointsCounterPlayerTwoCounter);
        CalculateLevelProgress(flightControllerScript.gameModeScript.playerOnePlane, ref levelProgressPlayerOneCounter);
        CalculateLevelProgress(flightControllerScript.gameModeScript.playerTwoPlane, ref levelProgressPlayerTwoCounter);
    }
}
