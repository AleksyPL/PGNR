using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    internal GameModeManager gameModeScript;
    internal DifficultyManager difficultyScript;
    public GameplaySettings gameplaySettings;
    internal InputManager inputManagerScript;
    internal LevelManager levelManagerScript;
    internal EnvironmentManager environmentManagerScript;
    internal RewardAndProgressionManager rewardAndProgressionManagerScript;
    internal AudioManager audioManagerScript;
    internal UIManager uiManagerScript;
    internal PowerUpManager powerUpManagerScript;
    void OnEnable()
    {
        uiManagerScript = GetComponent<UIManager>();
        gameModeScript = GetComponent<GameModeManager>();
        difficultyScript = GetComponent<DifficultyManager>();
        inputManagerScript = GetComponent<InputManager>();
        levelManagerScript = GetComponent<LevelManager>();
        environmentManagerScript = GetComponent<EnvironmentManager>();
        audioManagerScript = GetComponent<AudioManager>();
        rewardAndProgressionManagerScript = GetComponent<RewardAndProgressionManager>();
        powerUpManagerScript = GetComponent<PowerUpManager>();
    }
    private void Start()
    {
        audioManagerScript.UpdateAllSoundsVolume();
        audioManagerScript.PlaySound("TopGunTheme", audioManagerScript.localOtherSounds);
    }
    internal void ThrowBottleOfVodka(Plane plane, bool pressedBeforePauseScreen = false)
    {
        if (plane.currentPlaneState == PlaneState.standard)
        {
            if (plane.attackKeyPressed)
            {
                if (plane.timeToFullyChargeBottleThrowCounter < gameplaySettings.timeToFullyChargeBottleThrow)
                    plane.timeToFullyChargeBottleThrowCounter += Time.deltaTime;
            }
            if (plane.attackKeyReleased || pressedBeforePauseScreen)
            {
                //touch screen hack
                plane.attackKeyReleased = false;
                //end of hack
                float bottleThrowForceCurrent = Mathf.Lerp(gameplaySettings.bottleThrowForceMin, gameplaySettings.bottleThrowForceMax, plane.timeToFullyChargeBottleThrowCounter / gameplaySettings.timeToFullyChargeBottleThrow);
                Vector2 bottleThrowAngleCurrent = Vector2.Lerp(gameplaySettings.bottleThrowAngleMin, gameplaySettings.bottleThrowAngleMax, plane.timeToFullyChargeBottleThrowCounter / gameplaySettings.timeToFullyChargeBottleThrow);
                plane.SpawnBottleOfVodka(bottleThrowForceCurrent, bottleThrowAngleCurrent);
                if(plane.multiShotEnabled)
                {
                    plane.SpawnBottleOfVodka(bottleThrowForceCurrent, bottleThrowAngleCurrent * (1 + gameplaySettings.multishotSpread));
                    plane.SpawnBottleOfVodka(bottleThrowForceCurrent, bottleThrowAngleCurrent * (1 - gameplaySettings.multishotSpread));
                }
                plane.timeToFullyChargeBottleThrowCounter = 0;
                if (plane.difficultyImpulseEnabled == false)
                    plane.difficultyImpulseEnabled = true;
                if (plane.gettingWastedXTimesMoreEnabled)
                {
                    plane.bottlesDrunk += gameplaySettings.gettingWastedXTimesMoreNumber;
                    plane.bottlesDrunkTotal += gameplaySettings.gettingWastedXTimesMoreNumber;
                }
                else
                {
                    plane.bottlesDrunk++;
                    plane.bottlesDrunkTotal++;
                }
                uiManagerScript.UpdateBottlesCounter(plane);
            }
        }
    }
    private void MovePlaneStandardAndWheels(Plane plane)
    {
        //inverted steering power up
        if (plane.invertedSteeringEnabled)
            plane.verticalMovementKeys = -plane.verticalMovementKeys;
        //visually rotating a plane
        if (plane.verticalMovementKeys != 0)
            plane.planeRendererScript.ChangeTilt(plane.currentPlaneState, plane.verticalMovementKeys);
        else if (plane.verticalMovementKeys == 0)
            plane.planeRendererScript.ChangeTilt(plane.currentPlaneState, 0);
        //airport touch hack
        if (plane.isTouchingAirport)
            plane.verticalMovementKeys = 0;
        //end of hack
        //moving a plane
        if (plane.verticalMovementKeys != plane.difficultyImpulseDirection && plane.verticalMovementKeys != 0)
            plane.planeGameObject.transform.position += new Vector3(plane.currentPlaneSpeed * Time.deltaTime, plane.verticalMovementKeys * plane.altitudeChangeForce * gameplaySettings.altitudeChangeForceOverridedMultiplier * Time.deltaTime, 0);
        else
            plane.planeGameObject.transform.position += new Vector3(plane.currentPlaneSpeed * Time.deltaTime, plane.verticalMovementKeys * plane.altitudeChangeForce * Time.deltaTime, 0);
        if (plane.planeGameObject.transform.position.y > plane.topScreenHeight)
            plane.planeGameObject.transform.position = new Vector3(plane.planeGameObject.transform.position.x, plane.topScreenHeight, 0);
        if (plane.planeGameObject.transform.position.y < plane.groundLevelHeight)
            plane.planeGameObject.transform.position = new Vector3(plane.planeGameObject.transform.position.x, plane.groundLevelHeight, 0);
        if (plane.isTouchingAirport)
        {
            uiManagerScript.UpdateLevelProgressBar(plane);
            audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(audioManagerScript.localOneLinersSounds);
            audioManagerScript.StopPlayingSound("EngineSound", audioManagerScript.localSFX);
            if(!plane.tiresSFXPlayed)
            {
                audioManagerScript.PlaySound("Tires", audioManagerScript.localSFX);
                plane.tiresSFXPlayed = true;
            }
            plane.planeRendererScript.ChangeTilt(plane.currentPlaneState, 0);
            plane.currentPlaneSpeed -= gameplaySettings.airportSlowingForce * Time.deltaTime;
            plane.difficultyImpulseEnabled = false;
            if (plane.currentPlaneSpeed <= 0)
            {
                plane.currentPlaneSpeed = 0;
                if (!plane.rewardForLandingAdded)
                {
                    plane.rewardForLandingAdded = true;
                    plane.gameScore += gameplaySettings.rewardForLanding;
                }
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
        if(!uiManagerScript.pauseScreenEnabled && !uiManagerScript.timerBeforeTheFlightEnabled)
        {
            if (gameModeScript.playerOnePlane.currentPlaneState == PlaneState.standard || gameModeScript.playerOnePlane.currentPlaneState == PlaneState.wheelsOn)
            {
                MovePlaneStandardAndWheels(gameModeScript.playerOnePlane);
                if (gameModeScript.playerOnePlane.currentPlaneState != PlaneState.wheelsOn)
                    ThrowBottleOfVodka(gameModeScript.playerOnePlane);
            }
            if (gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless && (gameModeScript.playerTwoPlane.currentPlaneState == PlaneState.standard || gameModeScript.playerTwoPlane.currentPlaneState == PlaneState.wheelsOn))
            {
                MovePlaneStandardAndWheels(gameModeScript.playerTwoPlane);
                if (gameModeScript.playerTwoPlane.currentPlaneState != PlaneState.wheelsOn)
                    ThrowBottleOfVodka(gameModeScript.playerTwoPlane);
            }
            if (gameModeScript.playerOnePlane.currentPlaneState == PlaneState.damaged)
                MovePlaneDamaged(gameModeScript.playerOnePlane);
            if ((gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless) && gameModeScript.playerTwoPlane.currentPlaneState == PlaneState.damaged)
                MovePlaneDamaged(gameModeScript.playerTwoPlane);
        }
    }
}
