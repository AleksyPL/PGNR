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
    //internal PlaneScript baseScript;
    //internal float altitudeChangeForceCurrent;
    internal float waitingTimeAfterLandingCurrent;
    internal float waitingTimeAfterLandingCombinedWithSoundLength;
    //internal float currentPlaneSpeed;
    internal float drunkBottlesInTotal;
    //internal bool isTouchingAirport;
    //internal bool isTouchingGround;
    internal bool toNewLevel;
    internal bool rewardForLandingAdded;
    private float timeToFullyChargeBottleThrowCounter;

    void Start()
    {
        //baseScript = planeControlPanelGameObject.GetComponent<PlaneScript>();
        gameModeScript = GetComponent<GameModeManager>();
        difficultyScript = GetComponent<DifficultyManager>();
        //isTouchingAirport = false;
        //isTouchingGround = false;
        rewardForLandingAdded = false;

        drunkBottlesInTotal = 0;
        if (gameplaySettings.waitingTimeAfterLanding <= 0)
            gameplaySettings.waitingTimeAfterLanding = 3f;
        waitingTimeAfterLandingCombinedWithSoundLength = gameplaySettings.waitingTimeAfterLanding;
    }
    private void MovePlane(Plane plane, float positionY, bool attackKeyPressed, bool attackKeyReleased)
    {
        if (positionY != 0)
            plane.planeRendererScript.ChangeTilt(plane.currentPlaneState, positionY);
        plane.planeGameObject.transform.position += new Vector3(plane.currentPlaneSpeed * Time.deltaTime, positionY * plane.altitudeChangeForce * Time.deltaTime, 0);
        if (plane.planeGameObject.transform.position.y > gameplaySettings.topScreenHeight)
            plane.planeGameObject.transform.position = new Vector3(plane.planeGameObject.transform.position.x, gameplaySettings.topScreenHeight, 0);
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
    private void Update()
    {
        if (gameModeScript.playerOnePlane.currentPlaneState == PlaneState.standard || gameModeScript.playerOnePlane.currentPlaneState == PlaneState.wheelsOn)
        {
            //MovePlane(gameModeScript.playerOnePlane, )
        }
    }
}
