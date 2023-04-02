using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{

    //public GameObject planeControlPanelGameObject;
    //public GameObject planeGameObject;
    internal GameModeManager gameModeScript;
    internal DifficultyManager difficultyScript;
    public GameplaySettings gameplaySettings;
    internal InputManager inputManagerScript;
    internal LevelManager levelManagerScript;
    internal RewardAndProgressionManager rewardAndProgressionManagerScript;
    //internal PlaneScript baseScript;
    //internal float altitudeChangeForceCurrent;
    //internal float waitingTimeAfterLandingCurrent;
    //internal float waitingTimeAfterLandingCombinedWithSoundLength;
    //internal float currentPlaneSpeed;
    //internal float drunkBottlesInTotal;
    //internal bool isTouchingAirport;
    //internal bool isTouchingGround;
    //internal bool toNewLevel;
    //internal bool rewardForLandingAdded;
    //private float timeToFullyChargeBottleThrowCounter;

    void OnEnable()
    {
        //baseScript = planeControlPanelGameObject.GetComponent<PlaneScript>();
        gameModeScript = GetComponent<GameModeManager>();
        difficultyScript = GetComponent<DifficultyManager>();
        inputManagerScript = GetComponent<InputManager>();
        levelManagerScript = GetComponent<LevelManager>();
        rewardAndProgressionManagerScript = GetComponent<RewardAndProgressionManager>();
        //rewardForLandingAdded = false;

        //drunkBottlesInTotal = 0;
        //if (gameplaySettings.waitingTimeAfterLanding <= 0)
        //    gameplaySettings.waitingTimeAfterLanding = 3f;
        //waitingTimeAfterLandingCombinedWithSoundLength = gameplaySettings.waitingTimeAfterLanding;
    }
    private void CalculateThrowForce(Plane plane, bool attackKeyPressed, bool attackKeyReleased)
    {
        if (plane.currentPlaneState == PlaneState.standard)
        {
            if (attackKeyPressed)
            {
                if (plane.timeToFullyChargeBottleThrowCounter < gameplaySettings.timeToFullyChargeBottleThrow)
                    plane.timeToFullyChargeBottleThrowCounter += Time.deltaTime;
            }
            if (attackKeyReleased)
            {
                float bottleThrowForceCurrent = Mathf.Lerp(gameplaySettings.bottleThrowForceMin, gameplaySettings.bottleThrowForceMax, plane.timeToFullyChargeBottleThrowCounter / gameplaySettings.timeToFullyChargeBottleThrow);
                Vector2 bottleThrowAngleCurrent = Vector2.Lerp(gameplaySettings.bottleThrowAngleMin, gameplaySettings.bottleThrowAngleMax, plane.timeToFullyChargeBottleThrowCounter / gameplaySettings.timeToFullyChargeBottleThrow);
                plane.ThrowBottleOfVodka(bottleThrowForceCurrent, bottleThrowAngleCurrent);
                plane.timeToFullyChargeBottleThrowCounter = 0;
            }
        }
    }
    private void MovePlaneStandardAndWheels(Plane plane, float positionY)
    {
        if (positionY != 0)
            plane.planeRendererScript.ChangeTilt(plane.currentPlaneState, positionY);
        else
            plane.planeRendererScript.ChangeTilt(plane.currentPlaneState, 0);
        plane.planeGameObject.transform.position += new Vector3(plane.currentPlaneSpeed * Time.deltaTime, positionY * plane.altitudeChangeForce * Time.deltaTime, 0);
        if (plane.planeGameObject.transform.position.y > plane.topScreenHeight)
            plane.planeGameObject.transform.position = new Vector3(plane.planeGameObject.transform.position.x, plane.topScreenHeight, 0);
        if (plane.isTouchingAirport)
        {
            plane.planeRendererScript.ChangeTilt(plane.currentPlaneState, 0);
            plane.currentPlaneSpeed -= gameplaySettings.airportSlowingForce * Time.deltaTime;
            if (plane.playerNumber == 0)
                difficultyScript.enableDifficultyImpulsesPlayerOne = false;
            else
                difficultyScript.enableDifficultyImpulsesPlayerTwo = false;
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
        //if (baseScript.currentPlaneState == PlaneScript.StateMachine.standard || baseScript.currentPlaneState == PlaneScript.StateMachine.wheelsOn)
        //{
        //    planeGameObject.transform.position += new Vector3(currentPlaneSpeed * Time.deltaTime, baseScript.inputScript.position.y * altitudeChangeForceCurrent * Time.deltaTime, 0);
        //    if (planeGameObject.transform.position.y > gameplaySettings.topScreenHeight)
        //        planeGameObject.transform.position = new Vector3(planeGameObject.transform.position.x, gameplaySettings.topScreenHeight, 0);
        //    if (isTouchingAirport)
        //    {
        //        baseScript.inputScript.position.y = 0;
        //        currentPlaneSpeed -= gameplaySettings.airportSlowingForce * Time.deltaTime;
        //        baseScript.difficultyScript.enableDifficultyImpulses = false;
        //        if (currentPlaneSpeed <= 0)
        //        {
        //            currentPlaneSpeed = 0;
        //            if (!rewardForLandingAdded)
        //            {
        //                rewardForLandingAdded = true;
        //                baseScript.levelManagerScript.gameScore += gameplaySettings.rewardForLanding;
        //            }
        //            if (!toNewLevel)
        //            {
        //                waitingTimeAfterLandingCurrent += Time.deltaTime;
        //                if (waitingTimeAfterLandingCurrent >= waitingTimeAfterLandingCombinedWithSoundLength)
        //                {
        //                    toNewLevel = true;
        //                    waitingTimeAfterLandingCurrent = 0;
        //                    waitingTimeAfterLandingCombinedWithSoundLength = gameplaySettings.waitingTimeAfterLanding;
        //                    baseScript.levelManagerScript.LoadLevel();
        //                }
        //            }
        //        }
        //    }
        //    if (baseScript.currentPlaneState == PlaneScript.StateMachine.standard)
        //    {
        //        if (baseScript.inputScript.spaceHold)
        //        {
        //            if (timeToFullyChargeBottleThrowCounter < gameplaySettings.timeToFullyChargeBottleThrow)
        //                timeToFullyChargeBottleThrowCounter += Time.deltaTime;
        //        }
        //        if (baseScript.inputScript.spaceReleased)
        //        {
        //            float bottleThrowForceCurrent = Mathf.Lerp(gameplaySettings.bottleThrowForceMin, gameplaySettings.bottleThrowForceMax, timeToFullyChargeBottleThrowCounter / gameplaySettings.timeToFullyChargeBottleThrow);
        //            Vector2 bottleThrowAngleCurrent = Vector2.Lerp(gameplaySettings.bottleThrowAngleMin, gameplaySettings.bottleThrowAngleMax, timeToFullyChargeBottleThrowCounter / gameplaySettings.timeToFullyChargeBottleThrow);
        //            //ThrowBottleOfVodka(bottleThrowForceCurrent, bottleThrowAngleCurrent);
        //            drunkBottlesInTotal++;
        //            timeToFullyChargeBottleThrowCounter = 0;
        //        }
        //    }
        //}
        //else if (baseScript.currentPlaneState == PlaneScript.StateMachine.damaged)
        //{
        //    baseScript.difficultyScript.enableDifficultyImpulses = false;
        //    planeGameObject.transform.position += new Vector3(currentPlaneSpeed * Time.deltaTime, -gameplaySettings.fallingForce * Time.deltaTime, 0);
        //}
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
    private void MovePlaneDamaged(Plane plane)
    {
        if (plane.currentPlaneState == PlaneState.damaged)
        {
            if(plane.playerNumber == 0)
                difficultyScript.enableDifficultyImpulsesPlayerOne = false;
            else
                difficultyScript.enableDifficultyImpulsesPlayerTwo = false;
            plane.planeGameObject.transform.position += new Vector3(plane.currentPlaneSpeed * Time.deltaTime, -gameplaySettings.fallingForce * Time.deltaTime, 0);
        }
    }
    private void Update()
    {
        if(gameModeScript.playerOnePlane.currentPlaneState == PlaneState.standard || gameModeScript.playerOnePlane.currentPlaneState == PlaneState.wheelsOn)
        {
            MovePlaneStandardAndWheels(gameModeScript.playerOnePlane, inputManagerScript.positionPlayerOne);
            CalculateThrowForce(gameModeScript.playerOnePlane, inputManagerScript.spaceHoldPlayerOne, inputManagerScript.spaceReleasedPlayerOne);
        }
        if(gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayer && (gameModeScript.playerTwoPlane.currentPlaneState == PlaneState.standard || gameModeScript.playerTwoPlane.currentPlaneState == PlaneState.wheelsOn))
        {
            MovePlaneStandardAndWheels(gameModeScript.playerTwoPlane, inputManagerScript.positionPlayerTwo);
            CalculateThrowForce(gameModeScript.playerTwoPlane, inputManagerScript.spaceHoldPlayerTwo, inputManagerScript.spaceReleasedPlayerTwo);
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
