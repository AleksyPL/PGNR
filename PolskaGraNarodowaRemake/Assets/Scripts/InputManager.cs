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
        if (!UnityEngine.Device.Application.isMobilePlatform)
        {
            flightControllerScript.gameModeScript.playerOnePlane.verticalMovementKeys = Input.GetAxisRaw("Vertical");
            flightControllerScript.gameModeScript.playerOnePlane.attackKeyPressed = Input.GetButton("Jump");
            flightControllerScript.gameModeScript.playerOnePlane.attackKeyReleased = Input.GetButtonUp("Jump");
            ESCpressed = Input.GetButtonDown("Cancel");
            if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
            {
                flightControllerScript.gameModeScript.playerTwoPlane.verticalMovementKeys = Input.GetAxisRaw("Vertical1");
                flightControllerScript.gameModeScript.playerTwoPlane.attackKeyPressed = Input.GetButton("Jump1");
                flightControllerScript.gameModeScript.playerTwoPlane.attackKeyReleased = Input.GetButtonUp("Jump1");
            }
        }
    }
    public void TouchScreenButtonUpPressed()
    {
        flightControllerScript.gameModeScript.playerOnePlane.verticalMovementKeys = 1;
    }
    public void TouchScreenButtonDownPressed()
    {
        flightControllerScript.gameModeScript.playerOnePlane.verticalMovementKeys = -1;
    }
    public void TouchScreenMovementButtonReleased()
    {
        flightControllerScript.gameModeScript.playerOnePlane.verticalMovementKeys = 0;
    }
    public void TouchScreenFireButtonPressed()
    {
        flightControllerScript.gameModeScript.playerOnePlane.attackKeyPressed = true;
        flightControllerScript.gameModeScript.playerOnePlane.attackKeyReleased = false;
    }
    public void TouchScreenFireButtonReleased()
    {
        flightControllerScript.gameModeScript.playerOnePlane.attackKeyPressed = false;
        flightControllerScript.gameModeScript.playerOnePlane.attackKeyReleased = true;
    }
    public void TouchScreenPauseScreenButtonPressed()
    {
        ESCpressed = true;
    }
}
