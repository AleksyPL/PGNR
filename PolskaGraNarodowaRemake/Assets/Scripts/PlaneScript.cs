using System.Collections;
using System.Collections.Generic;
using UnityEngine;


internal enum PlaneState
{
    standard,
    wheelsOn,
    damaged,
    crashed
}

[System.Serializable]
internal class Plane
{
    //Plane State
    internal PlaneState currentPlaneState;
    //Settings
    public GameplaySettings gameplaySettings;
    //Game Objects - Spawners etc
    internal int playerNumber;
    public GameObject planeGameObject;
    public GameObject bottleSpawnerGameObject;
    public GameObject smokeSpawnerGameObject;
    public GameObject planeRendererGameObject;
    public GameObject projectilesParentGameObject;
    public GameObject cameraGameObject;
    //Prefabs
    public GameObject[] bottlePrefab;
    public GameObject smokePrefab;
    public GameObject explosionPrefab;
    //Scripts
    internal PlaneRenderer planeRendererScript;
    //Difficulty
    internal bool difficultyImpulseEnabled;
    internal int difficultyImpulseDirection;
    internal float difficultyImpulsTimeCurrent;
    internal float difficultuImpulseCounter;
    internal int bottleDrunkCounter;
    //Camera borders
    internal float groundLevelHeight;
    internal float topScreenHeight;
    //Flight controller settings
    internal bool isTouchingAirport;
    internal bool isTouchingGround;
    internal bool tiresSFXPlayed;
    internal float currentPlaneSpeed;
    internal float altitudeChangeForce;
    internal float timeToFullyChargeBottleThrowCounter;
    //Input
    internal float verticalMovementKeys;
    internal bool attackKeyPressed;
    internal bool attackKeyReleased;
    //Score
    internal int gameScore;
    internal bool rewardForLandingAdded;
    //Others
    internal AudioManager audioManagerScript;
    public bool godMode;
    internal void LoadPlaneData(int numberOfThePlayer)
    {
        planeRendererScript = planeRendererGameObject.GetComponent<PlaneRenderer>();
        audioManagerScript = GameObject.Find("MasterController").GetComponent<AudioManager>();
        playerNumber = numberOfThePlayer;
        planeRendererScript.planeSkin = gameplaySettings.planeSkins[gameplaySettings.playersPlaneSkins[playerNumber]];
        planeRendererScript.LoadPlaneSkin();
        gameScore = 0;
        ResetPlaneData();
    }
    internal void ResetPlaneData()
    {
        attackKeyPressed = false;
        attackKeyReleased = false;
        verticalMovementKeys = 0;
        isTouchingAirport = false;
        isTouchingGround = false;
        tiresSFXPlayed = false;
        currentPlaneSpeed = gameplaySettings.defaultPlaneSpeed;
        altitudeChangeForce = gameplaySettings.altitudeChangeForce;
        bottleDrunkCounter = 0;
        difficultyImpulseEnabled = false;
        difficultyImpulseDirection = 1;
        difficultyImpulsTimeCurrent = 0;
        rewardForLandingAdded = false;
        timeToFullyChargeBottleThrowCounter = 0;
        currentPlaneState = PlaneState.standard;
        planeRendererScript.ChangePlaneSprite(currentPlaneState);
        planeRendererScript.ChangeTilt(currentPlaneState, 0);
        if (smokeSpawnerGameObject.transform.childCount != 0)
            foreach (Transform child in smokeSpawnerGameObject.transform)
                GameObject.Destroy(child.gameObject);
    }
    internal void SpawnBottleOfVodka(float bottleThrowForce, Vector2 bottleThrowAngle)
    {
        if (currentPlaneState == PlaneState.standard)
        {
            int bottleVariant = Random.Range(0, bottlePrefab.Length);
            GameObject bottle = Object.Instantiate(bottlePrefab[bottleVariant], bottleSpawnerGameObject.transform.position, Quaternion.identity, projectilesParentGameObject.transform);
            bottle.GetComponent<BottleOfVodka>().SetParentObject(this);
            bottle.GetComponent<Rigidbody2D>().AddForce(bottleThrowAngle * bottleThrowForce);
            int randomDirection = Random.Range(0, 2);
            if (randomDirection == 0)
                bottle.GetComponent<Rigidbody2D>().AddTorque(180);
            else
                bottle.GetComponent<Rigidbody2D>().AddTorque(-180);
            bottleDrunkCounter++;
        }
    }
    internal void DamageThePlane()
    {
        currentPlaneState = PlaneState.damaged;
        planeRendererScript.ChangePlaneSprite(currentPlaneState);
        planeRendererScript.ChangeTilt(currentPlaneState, -1);
        audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(audioManagerScript.oneLinersSounds);
        audioManagerScript.PlaySound("Whistle", audioManagerScript.SFX);
        if (smokePrefab != null)
            Object.Instantiate(smokePrefab, smokeSpawnerGameObject.transform.position, Quaternion.Euler(270, 0, 0), smokeSpawnerGameObject.transform);
        if (explosionPrefab != null)
        {
            Object.Instantiate(explosionPrefab, planeGameObject.transform.position, Quaternion.identity, planeGameObject.transform);
            audioManagerScript.PlaySound("Explosion", audioManagerScript.SFX);
        }
        int randomSoundEffect = Random.Range(0, audioManagerScript.hitReactionSounds.Length);
        audioManagerScript.PlaySound("HitReaction" + randomSoundEffect, audioManagerScript.hitReactionSounds);
    }
    internal void DestroyThePlane()
    {
        currentPlaneState = PlaneState.crashed;
        planeRendererScript.ChangePlaneSprite(currentPlaneState);
        planeRendererScript.ChangeTilt(currentPlaneState, -1);
        audioManagerScript.StopPlayingSound("Whistle", audioManagerScript.SFX);
        audioManagerScript.StopPlayingSound("EngineSound", audioManagerScript.SFX);
        if (smokePrefab != null && smokeSpawnerGameObject.transform.childCount == 0)
            Object.Instantiate(smokePrefab, smokeSpawnerGameObject.transform.position, Quaternion.Euler(270, 0, 0), smokeSpawnerGameObject.transform);
        if (explosionPrefab != null)
        {
            Object.Instantiate(explosionPrefab, planeGameObject.transform.position, Quaternion.identity, planeGameObject.transform);
            audioManagerScript.PlaySound("Explosion", audioManagerScript.SFX);
        }
    }
}