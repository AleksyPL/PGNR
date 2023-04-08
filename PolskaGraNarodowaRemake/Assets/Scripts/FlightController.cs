using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    internal GameModeManager gameModeScript;
    internal DifficultyManager difficultyScript;
    public GameplaySettings gameplaySettings;
    internal InputManager inputManagerScript;
    internal LevelManager levelManagerScript;
    internal RewardAndProgressionManager rewardAndProgressionManagerScript;
    internal UIManager uiManagerScript;
    public GameObject uiManagerGameObject;
    //internal float waitingTimeAfterLandingCurrent;
    //internal float waitingTimeAfterLandingCombinedWithSoundLength;
    //internal bool rewardForLandingAdded;
    //private float timeToFullyChargeBottleThrowCounter;

    void OnEnable()
    {
        gameModeScript = GetComponent<GameModeManager>();
        difficultyScript = GetComponent<DifficultyManager>();
        inputManagerScript = GetComponent<InputManager>();
        levelManagerScript = GetComponent<LevelManager>();
        uiManagerScript = uiManagerGameObject.GetComponent<UIManager>();
        rewardAndProgressionManagerScript = GetComponent<RewardAndProgressionManager>();
        //rewardForLandingAdded = false;

        //drunkBottlesInTotal = 0;
        //if (gameplaySettings.waitingTimeAfterLanding <= 0)
        //    gameplaySettings.waitingTimeAfterLanding = 3f;
        //waitingTimeAfterLandingCombinedWithSoundLength = gameplaySettings.waitingTimeAfterLanding;
    }
    private void ThrowBottleOfVodka(Plane plane)
    {
        if (plane.currentPlaneState == PlaneState.standard)
        {
            if (plane.attackKeyPressed)
            {
                if (plane.timeToFullyChargeBottleThrowCounter < gameplaySettings.timeToFullyChargeBottleThrow)
                    plane.timeToFullyChargeBottleThrowCounter += Time.deltaTime;
            }
            if (plane.attackKeyReleased)
            {
                float bottleThrowForceCurrent = Mathf.Lerp(gameplaySettings.bottleThrowForceMin, gameplaySettings.bottleThrowForceMax, plane.timeToFullyChargeBottleThrowCounter / gameplaySettings.timeToFullyChargeBottleThrow);
                Vector2 bottleThrowAngleCurrent = Vector2.Lerp(gameplaySettings.bottleThrowAngleMin, gameplaySettings.bottleThrowAngleMax, plane.timeToFullyChargeBottleThrowCounter / gameplaySettings.timeToFullyChargeBottleThrow);
                plane.SpawnBottleOfVodka(bottleThrowForceCurrent, bottleThrowAngleCurrent);
                plane.timeToFullyChargeBottleThrowCounter = 0;
                if (plane.difficultyImpulseEnabled == false)
                    plane.difficultyImpulseEnabled = true;
            }
        }
    }
    private void MovePlaneStandardAndWheels(Plane plane)
    {
        if (plane.verticalMovementKeys != 0)
            plane.planeRendererScript.ChangeTilt(plane.currentPlaneState, plane.verticalMovementKeys);
        else
            plane.planeRendererScript.ChangeTilt(plane.currentPlaneState, 0);
        plane.planeGameObject.transform.position += new Vector3(plane.currentPlaneSpeed * Time.deltaTime, plane.verticalMovementKeys * plane.altitudeChangeForce * Time.deltaTime, 0);
        if (plane.planeGameObject.transform.position.y > plane.topScreenHeight)
            plane.planeGameObject.transform.position = new Vector3(plane.planeGameObject.transform.position.x, plane.topScreenHeight, 0);
        if (plane.isTouchingAirport)
        {
            plane.planeRendererScript.ChangeTilt(plane.currentPlaneState, 0);
            plane.currentPlaneSpeed -= gameplaySettings.airportSlowingForce * Time.deltaTime;
            plane.difficultyImpulseEnabled = false;
            if (plane.currentPlaneSpeed <= 0)
            {
                plane.currentPlaneSpeed = 0;
                //if (!rewardForLandingAdded)
                //{
                //    rewardForLandingAdded = true;
                //    baseScript.levelManagerScript.gameScore += gameplaySettings.rewardForLanding;
                //}
                //if (!toNewLevel)
                //{
                //    waitingTimeAfterLandingCurrent += Time.deltaTime;
                //    if (waitingTimeAfterLandingCurrent >= waitingTimeAfterLandingCombinedWithSoundLength)
                //    {
                //        toNewLevel = true;
                //        waitingTimeAfterLandingCurrent = 0;
                //        waitingTimeAfterLandingCombinedWithSoundLength = gameplaySettings.waitingTimeAfterLanding;
                //        baseScript.levelManagerScript.LoadLevel();
                //    }
                //}
            }
        }
    }
    private void MovePlaneDamaged(Plane plane)
    {
        if (plane.currentPlaneState == PlaneState.damaged)
        {
            plane.difficultyImpulseEnabled = false;
            plane.planeGameObject.transform.position += new Vector3(plane.currentPlaneSpeed * Time.deltaTime, -gameplaySettings.fallingForce * Time.deltaTime, 0);
        }
    }
    private void Update()
    {
        if(gameModeScript.playerOnePlane.currentPlaneState == PlaneState.standard || gameModeScript.playerOnePlane.currentPlaneState == PlaneState.wheelsOn)
        {
            MovePlaneStandardAndWheels(gameModeScript.playerOnePlane);
            ThrowBottleOfVodka(gameModeScript.playerOnePlane);
        }
        if(gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayer && (gameModeScript.playerTwoPlane.currentPlaneState == PlaneState.standard || gameModeScript.playerTwoPlane.currentPlaneState == PlaneState.wheelsOn))
        {
            MovePlaneStandardAndWheels(gameModeScript.playerTwoPlane);
            ThrowBottleOfVodka(gameModeScript.playerTwoPlane);
        }
        if(gameModeScript.playerOnePlane.currentPlaneState == PlaneState.damaged)
            MovePlaneDamaged(gameModeScript.playerOnePlane);
        if(gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayer && gameModeScript.playerTwoPlane.currentPlaneState == PlaneState.damaged)
            MovePlaneDamaged(gameModeScript.playerTwoPlane);
        //else if (baseScript.currentPlaneState == PlaneScript.StateMachine.crashed)
        //{
        //    waitingTimeAfterLandingCurrent += Time.deltaTime;
        //    if (waitingTimeAfterLandingCurrent >= gameplaySettings.waitingTimeAfterLanding)
        //    {
        //        waitingTimeAfterLandingCurrent = 0;
        //        baseScript.UIScript.EnableGameOverScreen();
        //    }
        //}
    }
}
