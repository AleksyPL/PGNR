using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAndProgressionManager : MonoBehaviour
{
    internal FlightController flightControllerScript;
    internal int levelCounter;
    internal float currentlevelDistance;
    private float levelProgressPlayerOne;
    private float levelProgressPlayerTwo;
    private float scorePointsCounterPlayerOne;
    private float scorePointsCounterPlayerTwo;
    void Start()
    {
        
    }
    private void CalculateScore(Plane plane, ref float counter)
    {
        if (plane.currentPlaneSpeed > 0 && (plane.currentPlaneState == PlaneState.standard || plane.currentPlaneState == PlaneState.wheelsOn))
        {
            counter += Time.deltaTime;
            if (counter > 1)
            {
                counter = 0;
                plane.gameScore++;
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
    void Update()
    {
        CalculateScore(flightControllerScript.gameModeScript.playerOnePlane, ref scorePointsCounterPlayerOne);
        CalculateScore(flightControllerScript.gameModeScript.playerTwoPlane, ref scorePointsCounterPlayerTwo);
        CalculateLevelProgress(flightControllerScript.gameModeScript.playerOnePlane, ref levelProgressPlayerOne);
        CalculateLevelProgress(flightControllerScript.gameModeScript.playerTwoPlane, ref levelProgressPlayerTwo);
    }
}
