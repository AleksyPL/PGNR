using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    public GameObject bottleSpawner;
    public GameObject smokeSpawner;
    public GameObject bottlePrefab;
    public GameObject smokePrefab;
    public GameObject explosionPrefab;
    public GameObject planeControlPanelGameObject;
    public GameObject planeGameObject;
    public GameplaySettings gameplaySettings;
    internal PlaneBase baseScript;
    internal float altitudeChangeForceCurrent;
    internal float waitingTimeAfterLandingCurrent;
    internal float waitingTimeAfterLandingCombinedWithSoundLength;
    internal float currentPlaneSpeed;
    internal float drunkBottlesInTotal;
    internal bool isTouchingAirport;
    internal bool isTouchingGround;
    internal bool toNewLevel;
    internal bool rewardForLandingAdded;
    private float timeToFullyChargeBottleThrowCounter;

    void Start()
    {
        baseScript = planeControlPanelGameObject.GetComponent<PlaneBase>();
        isTouchingAirport = false;
        isTouchingGround = false;
        rewardForLandingAdded = false;
        currentPlaneSpeed = gameplaySettings.defaultPlaneSpeed;
        altitudeChangeForceCurrent = gameplaySettings.altitudeChangeForce;
        drunkBottlesInTotal = 0;
        if (gameplaySettings.waitingTimeAfterLanding <= 0)
            gameplaySettings.waitingTimeAfterLanding = 3f;
        waitingTimeAfterLandingCombinedWithSoundLength = gameplaySettings.waitingTimeAfterLanding;
    }
    void Update()
    {
        if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard || baseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
        {
            planeGameObject.transform.position += new Vector3(currentPlaneSpeed * Time.deltaTime, baseScript.inputScript.position.y * altitudeChangeForceCurrent * Time.deltaTime, 0);
            if (planeGameObject.transform.position.y > gameplaySettings.topScreenHeight)
                planeGameObject.transform.position = new Vector3(planeGameObject.transform.position.x, gameplaySettings.topScreenHeight, 0);
            if (isTouchingAirport)
            {
                baseScript.inputScript.position.y = 0;
                currentPlaneSpeed -= gameplaySettings.airportSlowingForce * Time.deltaTime;
                baseScript.difficultyScript.enableDifficultyImpulses = false;
                if (currentPlaneSpeed <= 0)
                {
                    currentPlaneSpeed = 0;
                    if (!rewardForLandingAdded)
                    {
                        rewardForLandingAdded = true;
                        baseScript.levelManagerScript.gameScore += gameplaySettings.rewardForLanding;
                    }
                    if (!toNewLevel)
                    {
                        waitingTimeAfterLandingCurrent += Time.deltaTime;
                        if (waitingTimeAfterLandingCurrent >= waitingTimeAfterLandingCombinedWithSoundLength)
                        {
                            toNewLevel = true;
                            waitingTimeAfterLandingCurrent = 0;
                            waitingTimeAfterLandingCombinedWithSoundLength = gameplaySettings.waitingTimeAfterLanding;
                            baseScript.levelManagerScript.LoadLevel();
                        }
                    }
                }
            }  
            if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
            {
                if (baseScript.inputScript.spaceHold)
                {
                    if (timeToFullyChargeBottleThrowCounter < gameplaySettings.timeToFullyChargeBottleThrow)
                        timeToFullyChargeBottleThrowCounter += Time.deltaTime;
                }
                if (baseScript.inputScript.spaceReleased)
                {
                    float bottleThrowForceCurrent = Mathf.Lerp(gameplaySettings.bottleThrowForceMin, gameplaySettings.bottleThrowForceMax, timeToFullyChargeBottleThrowCounter / gameplaySettings.timeToFullyChargeBottleThrow);
                    Vector2 bottleThrowAngleCurrent = Vector2.Lerp(gameplaySettings.bottleThrowAngleMin, gameplaySettings.bottleThrowAngleMax, timeToFullyChargeBottleThrowCounter / gameplaySettings.timeToFullyChargeBottleThrow);
                    ThrowBottleOfVodka(bottleThrowForceCurrent, bottleThrowAngleCurrent);
                    drunkBottlesInTotal++;
                    timeToFullyChargeBottleThrowCounter = 0;
                }
            }
        }
        else if (baseScript.currentPlaneState == PlaneBase.StateMachine.damaged)
        {
            baseScript.difficultyScript.enableDifficultyImpulses = false;
            planeGameObject.transform.position += new Vector3(currentPlaneSpeed * Time.deltaTime, -gameplaySettings.fallingForce * Time.deltaTime, 0);
        }
        else if (baseScript.currentPlaneState == PlaneBase.StateMachine.crashed)
        {
            waitingTimeAfterLandingCurrent += Time.deltaTime;
            if(waitingTimeAfterLandingCurrent >= gameplaySettings.waitingTimeAfterLanding)
            {
                waitingTimeAfterLandingCurrent = 0;
                baseScript.UIScript.EnableGameOverScreen();
            }
        }
    }
    internal void ThrowBottleOfVodka(float bottleThrowForce, Vector2 bottleThrowAngle)
    {
        if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
        {
            GameObject bottle = Instantiate(bottlePrefab, bottleSpawner.transform.position, Quaternion.identity, baseScript.levelManagerGameObject.transform);
            bottle.GetComponent<Rigidbody2D>().AddForce(bottleThrowAngle * bottleThrowForce);
            baseScript.difficultyScript.difficultyMultiplier++;
            if (!baseScript.difficultyScript.enableDifficultyImpulses)
                baseScript.difficultyScript.enableDifficultyImpulses = true;
        }
    }
    internal void DamageThePlane()
    {
        baseScript.currentPlaneState = PlaneBase.StateMachine.damaged;
        baseScript.planeRendererScript.ChangePlaneSprite();
        baseScript.planeRendererScript.ChangeTilt();
        baseScript.audioScript.StopPlayingSoundsFromTheSpecificSoundBank(baseScript.audioScript.oneLinersSounds);
        baseScript.audioScript.PlaySound("Whistle", baseScript.audioScript.SFX);
        if (smokePrefab != null)
            Instantiate(smokePrefab, smokeSpawner.transform.position, Quaternion.Euler(270,0,0), smokeSpawner.transform);
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, planeGameObject.transform.position, Quaternion.identity, transform);
            baseScript.audioScript.PlaySound("Explosion", baseScript.audioScript.SFX);
        }
        int randomSoundEffect = Random.Range(0, baseScript.audioScript.hitReactionSounds.Length);
        baseScript.audioScript.PlaySound("HitReaction" + randomSoundEffect, baseScript.audioScript.hitReactionSounds);
    }
    internal void DestroyThePlane()
    {
        baseScript.currentPlaneState = PlaneBase.StateMachine.crashed;
        baseScript.planeRendererScript.ChangePlaneSprite();
        baseScript.planeRendererScript.ChangeTilt();
        baseScript.audioScript.StopPlayingSound("Whistle", baseScript.audioScript.SFX);
        baseScript.audioScript.StopPlayingSound("EngineSound", baseScript.audioScript.SFX);
        if (smokePrefab != null)
            Instantiate(smokePrefab, smokeSpawner.transform.position, Quaternion.Euler(270, 0, 0), smokeSpawner.transform);
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, planeGameObject.transform.position, Quaternion.identity, transform);
            baseScript.audioScript.PlaySound("Explosion", baseScript.audioScript.SFX);
        }
    }
}
