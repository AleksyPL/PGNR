using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    internal FlightController flightControllerScript;
    internal bool ESCpressed;
    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        ESCpressed = false;
    }
    void Update()
    {
        if(!flightControllerScript.uiManagerScript.pauseScreenEnabled && !flightControllerScript.gameModeScript.playerOnePlane.isTouchingAirport && flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.damaged && flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed)
        {
            if (((flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState != PlaneState.crashed) || (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless))
            {
                flightControllerScript.gameModeScript.playerOnePlane.verticalMovementKeys = Input.GetAxisRaw("Vertical");
                flightControllerScript.gameModeScript.playerOnePlane.attackKeyPressed = Input.GetButton("Jump");
                flightControllerScript.gameModeScript.playerOnePlane.attackKeyReleased = Input.GetButtonUp("Jump");
            }
        }
        if(!flightControllerScript.uiManagerScript.pauseScreenEnabled && (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && !flightControllerScript.gameModeScript.playerTwoPlane.isTouchingAirport && flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState != PlaneState.damaged && flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState != PlaneState.crashed && flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed)
        {
            flightControllerScript.gameModeScript.playerTwoPlane.verticalMovementKeys = Input.GetAxisRaw("Vertical1");
            flightControllerScript.gameModeScript.playerTwoPlane.attackKeyPressed = Input.GetButton("Jump1");
            flightControllerScript.gameModeScript.playerTwoPlane.attackKeyReleased = Input.GetButtonUp("Jump1");
        }
        if(((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless) && flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed) || ((flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && (flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed || flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState != PlaneState.crashed)))
        {
            ESCpressed = Input.GetButtonDown("Cancel");
        }
    }
}
