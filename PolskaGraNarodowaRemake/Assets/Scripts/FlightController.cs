using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlightController : MonoBehaviour
{
    public GameObject bottleSpawner;
    public GameObject smokeSpawner;
    public GameObject bottlePrefab;
    public GameObject smokePrefab;
    public GameObject explosionPrefab;
    
    internal PlaneBase baseScript;
    [SerializeField] internal float defaultPlaneSpeed;
    [SerializeField] internal float altitudeChangeForce;
    [SerializeField] internal float fallingForce;
    [SerializeField] internal float airportSlowingForce;
    [SerializeField] internal float bottleThrowForceMin;
    [SerializeField] internal float bottleThrowForceMax;
    [SerializeField] internal float bottleThrowForceIncreasmentPerFrame;
    internal float currentPlaneSpeed;
    internal bool isTouchingAirport;
    internal bool isTouchingGround;
    internal bool toNewLevel;
    private float bottleThrowForceCurrent;
    [SerializeField] private float waitingTimeAfterSoundEffect;
    private float waitingTimeForSoundEffectCurrent;
    
    void Start()
    {
        baseScript = GetComponent<PlaneBase>();
        isTouchingAirport = false;
        isTouchingGround = false;
        currentPlaneSpeed = defaultPlaneSpeed;
        bottleThrowForceCurrent = bottleThrowForceMin;
        if (waitingTimeAfterSoundEffect == 0)
            waitingTimeAfterSoundEffect = 3f;
    }
    void Update()
    {
        if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard || baseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
        {
            baseScript.audioScript.PlaySound("EngineSound", baseScript.audioScript.SFX);
            transform.position += new Vector3(currentPlaneSpeed * Time.deltaTime, baseScript.inputScript.position.y * altitudeChangeForce * Time.deltaTime, 0);
            if (isTouchingAirport)
            {
                baseScript.inputScript.position.y = 0;
                currentPlaneSpeed -= airportSlowingForce * Time.deltaTime;
                baseScript.difficultyScript.enableDifficultyImpulses = false;
                if (currentPlaneSpeed <= 0)
                {
                    currentPlaneSpeed = 0;
                    baseScript.audioScript.PlaySound("Tires", baseScript.audioScript.SFX);
                    if (!toNewLevel)
                    {
                        baseScript.audioScript.StopPlayingAllSounds();
                        int randomSoundEffect = Random.Range(0, baseScript.audioScript.landingSounds.Length);
                        float waitingTimeForSoundEffectCombinedWithSound = baseScript.audioScript.PlaySound("Landing" + randomSoundEffect, baseScript.audioScript.landingSounds);
                        waitingTimeForSoundEffectCurrent += Time.deltaTime;
                        if(waitingTimeForSoundEffectCurrent >= waitingTimeForSoundEffectCombinedWithSound)
                        {
                            waitingTimeForSoundEffectCurrent = 0;
                            toNewLevel = true;
                            baseScript.levelManagerScript.LoadLevel();
                        }
                    }
                }
            }  
            if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
            {
                if (baseScript.inputScript.spaceHold)
                {
                    if(bottleThrowForceCurrent < bottleThrowForceMax)
                    {
                        bottleThrowForceCurrent += bottleThrowForceIncreasmentPerFrame * Time.deltaTime;
                        if (bottleThrowForceCurrent > bottleThrowForceMax)
                            bottleThrowForceCurrent = bottleThrowForceMax;
                    }
                }
                if (baseScript.inputScript.spaceReleased)
                {
                    ThrowBottleOfVodka();
                    bottleThrowForceCurrent = bottleThrowForceMin;
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
            waitingTimeForSoundEffectCurrent += Time.deltaTime;
            if(waitingTimeForSoundEffectCurrent >= waitingTimeAfterSoundEffect)
            {
                waitingTimeForSoundEffectCurrent = 0;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
    internal void ThrowBottleOfVodka()
    {
        if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
        {
            GameObject bottle = Instantiate(bottlePrefab, bottleSpawner.transform.position, Quaternion.identity, transform);
            bottle.GetComponent<Rigidbody2D>().AddForce(new Vector2(1,-1) * bottleThrowForceCurrent);
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
        baseScript.audioScript.StopPlayingAllSounds();
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
        baseScript.audioScript.StopPlayingAllSounds();
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
            baseScript.audioScript.PlaySound("Explosion", baseScript.audioScript.SFX);
        }   
    }
}
