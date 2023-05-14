using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public GameplaySettings gameplaySettings;
    internal FlightController flightControllerScript;
    private float difficultyImpulsTimeMin;
    private float difficultyImpulsTimeMax;

    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        difficultyImpulsTimeMin = flightControllerScript.rewardAndProgressionManagerScript.levelCounter * 0.1f;
        difficultyImpulsTimeMax = 2 * difficultyImpulsTimeMin;
        flightControllerScript.gameModeScript.playerOnePlane.difficultyImpulsTimeCurrent = Random.Range(difficultyImpulsTimeMin, difficultyImpulsTimeMax);
        flightControllerScript.gameModeScript.playerOnePlane.difficultuImpulseCounter = flightControllerScript.gameModeScript.playerOnePlane.difficultyImpulsTimeCurrent;
        if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
        {
            flightControllerScript.gameModeScript.playerTwoPlane.difficultyImpulsTimeCurrent = Random.Range(difficultyImpulsTimeMin, difficultyImpulsTimeMax);
            flightControllerScript.gameModeScript.playerTwoPlane.difficultuImpulseCounter = flightControllerScript.gameModeScript.playerTwoPlane.difficultyImpulsTimeCurrent;
        }
    }

    void Update()
    {
        if(flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState == PlaneState.standard || (flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState == PlaneState.wheelsOn && !flightControllerScript.gameModeScript.playerOnePlane.isTouchingAirport))
            ApplyDifficultyImpulse(flightControllerScript.gameModeScript.playerOnePlane);
        if((flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && (flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState == PlaneState.standard || (flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState == PlaneState.wheelsOn && !flightControllerScript.gameModeScript.playerTwoPlane.isTouchingAirport)))
            ApplyDifficultyImpulse(flightControllerScript.gameModeScript.playerTwoPlane);
    }
    internal void ApplyDifficultyImpulse(Plane plane)
    {
        if (plane.difficultyImpulseEnabled)
        {
            plane.difficultuImpulseCounter -= Time.deltaTime;
            plane.planeGameObject.transform.position += new Vector3(0, plane.difficultyImpulseDirection * gameplaySettings.difficultyImpulseForce * plane.bottleDrunkCounter * Time.deltaTime, 0);
            if (plane.planeGameObject.transform.position.y > plane.topScreenHeight)
                plane.planeGameObject.transform.position = new Vector3(plane.planeGameObject.transform.position.x, plane.topScreenHeight, 0);
            if (plane.difficultuImpulseCounter <= 0)
            {
                plane.difficultyImpulsTimeCurrent = Random.Range(difficultyImpulsTimeMin, difficultyImpulsTimeMax);
                plane.difficultuImpulseCounter = plane.difficultyImpulsTimeCurrent;
                plane.difficultyImpulseDirection = Random.Range(-1, 1);
                if (plane.difficultyImpulseDirection == 0)
                    plane.difficultyImpulseDirection = 1;
            }
        }
    }
}
