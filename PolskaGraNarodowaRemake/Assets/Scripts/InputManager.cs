using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    internal float positionPlayerOne;
    internal float positionPlayerTwo;
    internal bool spaceHoldPlayerOne;
    internal bool spaceHoldPlayerTwo;
    internal bool spaceReleasedPlayerOne;
    internal bool spaceReleasedPlayerTwo;
    internal FlightController flightControllerScript;
    internal bool ESCpressed;
    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        positionPlayerOne = 0;
        positionPlayerTwo = 0;
        spaceHoldPlayerOne = false;
        spaceHoldPlayerTwo = false;
        ESCpressed = false;
    }
    void Update()
    {
        if(!flightControllerScript.gameModeScript.playerOnePlane.isTouchingAirport && flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.damaged && flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed)
        {
            positionPlayerOne = Input.GetAxisRaw("Vertical");
            spaceHoldPlayerOne = Input.GetButton("Jump");
            spaceReleasedPlayerOne = Input.GetButtonUp("Jump");
        }
        if(flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayer && !flightControllerScript.gameModeScript.playerTwoPlane.isTouchingAirport && flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState != PlaneState.damaged && flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState != PlaneState.crashed)
        {
            positionPlayerTwo = Input.GetAxisRaw("Vertical1");
            spaceHoldPlayerTwo = Input.GetButton("Jump1");
            spaceReleasedPlayerTwo = Input.GetButtonUp("Jump1");
        }
        if (flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState != PlaneState.crashed || flightControllerScript.gameModeScript.playerTwoPlane.currentPlaneState != PlaneState.crashed)
        {
            ESCpressed = Input.GetButtonDown("Cancel");
        }
    }
}
