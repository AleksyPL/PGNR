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
    public int rewardForLanding;
    internal PlaneBase baseScript;
    [SerializeField] internal float defaultPlaneSpeed;
    [SerializeField] internal float altitudeChangeForce;
    [SerializeField] internal float fallingForce;
    [SerializeField] internal float airportSlowingForce;
    [SerializeField] private float bottleThrowForceMin;
    [SerializeField] private float bottleThrowForceMax;
    [SerializeField] internal float timeToFullyChargeBottleThrow;
    [SerializeField] internal float waitingTimeAfterLanding;
    [SerializeField] private Vector2 bottleThrowAngleMin;
    [SerializeField] private Vector2 bottleThrowAngleMax;
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
        baseScript = GetComponent<PlaneBase>();
        isTouchingAirport = false;
        isTouchingGround = false;
        rewardForLandingAdded = false;
        currentPlaneSpeed = defaultPlaneSpeed;
        altitudeChangeForceCurrent = altitudeChangeForce;
        drunkBottlesInTotal = 0;
        if (waitingTimeAfterLanding <= 0)
            waitingTimeAfterLanding = 3f;
        waitingTimeAfterLandingCombinedWithSoundLength = waitingTimeAfterLanding;
    }
    void Update()
    {
        if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard || baseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
        {
            transform.position += new Vector3(currentPlaneSpeed * Time.deltaTime, baseScript.inputScript.position.y * altitudeChangeForceCurrent * Time.deltaTime, 0);
            if (transform.position.y > baseScript.levelManagerScript.topScreenHeight)
                transform.position = new Vector3(transform.position.x, baseScript.levelManagerScript.topScreenHeight, 0);
            if (isTouchingAirport)
            {
                baseScript.inputScript.position.y = 0;
                currentPlaneSpeed -= airportSlowingForce * Time.deltaTime;
                baseScript.difficultyScript.enableDifficultyImpulses = false;
                if (currentPlaneSpeed <= 0)
                {
                    currentPlaneSpeed = 0;
                    if (!rewardForLandingAdded)
                    {
                        rewardForLandingAdded = true;
                        baseScript.levelManagerScript.gameScore += rewardForLanding;
                    }
                    if (!toNewLevel)
                    {
                        waitingTimeAfterLandingCurrent += Time.deltaTime;
                        if (waitingTimeAfterLandingCurrent >= waitingTimeAfterLandingCombinedWithSoundLength)
                        {
                            toNewLevel = true;
                            waitingTimeAfterLandingCurrent = 0;
                            waitingTimeAfterLandingCombinedWithSoundLength = waitingTimeAfterLanding;
                            baseScript.levelManagerScript.LoadLevel();
                        }
                    }
                }
            }  
            if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
            {
                if (baseScript.inputScript.spaceHold)
                {
                    if (timeToFullyChargeBottleThrowCounter < timeToFullyChargeBottleThrow)
                        timeToFullyChargeBottleThrowCounter += Time.deltaTime;
                }
                if (baseScript.inputScript.spaceReleased)
                {
                    float bottleThrowForceCurrent = Mathf.Lerp(bottleThrowForceMin, bottleThrowForceMax, timeToFullyChargeBottleThrowCounter / timeToFullyChargeBottleThrow);
                    Vector2 bottleThrowAngleCurrent = Vector2.Lerp(bottleThrowAngleMin, bottleThrowAngleMax, timeToFullyChargeBottleThrowCounter / timeToFullyChargeBottleThrow);
                    ThrowBottleOfVodka(bottleThrowForceCurrent, bottleThrowAngleCurrent);
                    drunkBottlesInTotal++;
                    timeToFullyChargeBottleThrowCounter = 0;
                }
            }
        }
        else if (baseScript.currentPlaneState == PlaneBase.StateMachine.damaged)
        {
            baseScript.difficultyScript.enableDifficultyImpulses = false;
            transform.position += new Vector3(currentPlaneSpeed * Time.deltaTime, -fallingForce * Time.deltaTime, 0);
        }
        else if (baseScript.currentPlaneState == PlaneBase.StateMachine.crashed)
        {
            waitingTimeAfterLandingCurrent += Time.deltaTime;
            if(waitingTimeAfterLandingCurrent >= waitingTimeAfterLanding)
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
            GameObject bottle = Instantiate(bottlePrefab, bottleSpawner.transform.position, Quaternion.identity);
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
            Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
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
            Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
            baseScript.audioScript.PlaySound("Explosion", baseScript.audioScript.SFX);
        }
    }
}
