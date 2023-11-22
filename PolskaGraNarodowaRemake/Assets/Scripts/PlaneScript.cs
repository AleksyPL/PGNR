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
    public GameObject smokeSpawnerInAirGameObject;
    public GameObject smokeSpawnerOnTheGroundGameObject;
    public GameObject fireSpawnerGameObject;
    public GameObject planeRendererGameObject;
    public GameObject projectilesParentGameObject;
    public GameObject cameraGameObject;
    //Scripts
    internal PlaneRenderer planeRendererScript;
    //Difficulty
    internal bool difficultyImpulseEnabled;
    internal int difficultyImpulseDirection;
    internal float difficultyImpulsTimeCurrent;
    internal float difficultuImpulseCounter;
    internal int bottlesDrunk;
    internal int bottlesDrunkTotal;
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
    internal bool activeBottleWarning;
    //Score
    internal int gameScore;
    internal bool rewardForLandingAdded;
    //PowerUps
    internal bool shieldEnabled;
    internal bool speedEnabled;
    internal bool invertedSteeringEnabled;
    internal bool gettingWastedXTimesMoreEnabled;
    internal bool multiShotEnabled;
    //Others
    internal AudioManager audioManagerScript;
    public bool godModeLevelStart;
    internal bool godMode;
    internal void LoadPlaneData(int numberOfThePlayer)
    {
        playerNumber = numberOfThePlayer;
        gameScore = 0;
        planeRendererScript = planeRendererGameObject.GetComponent<PlaneRenderer>();
        audioManagerScript = GameObject.Find("MasterController").GetComponent<AudioManager>();
        planeRendererScript.planeSkin = gameplaySettings.planeSkins[gameplaySettings.playersPlaneSkins[playerNumber]];
        ResetPlaneData();
    }
    internal void ResetPlaneData()
    {
        godMode = godModeLevelStart;
        attackKeyPressed = false;
        attackKeyReleased = false;
        verticalMovementKeys = 0;
        isTouchingAirport = false;
        isTouchingGround = false;
        tiresSFXPlayed = false;
        currentPlaneSpeed = gameplaySettings.defaultPlaneSpeed;
        altitudeChangeForce = gameplaySettings.altitudeChangeForce;
        rewardForLandingAdded = false;
        timeToFullyChargeBottleThrowCounter = 0;
        shieldEnabled = false;
        speedEnabled = false;
        invertedSteeringEnabled = false;
        gettingWastedXTimesMoreEnabled = false;
        multiShotEnabled = false;
        currentPlaneState = PlaneState.standard;
        planeRendererScript.ResetPlaneRenderer(currentPlaneState);
        SoberUp();
        if (smokeSpawnerInAirGameObject.transform.childCount != 0)
            foreach (Transform child in smokeSpawnerInAirGameObject.transform)
                GameObject.Destroy(child.gameObject);
        if (smokeSpawnerOnTheGroundGameObject.transform.childCount != 0)
            foreach (Transform child in smokeSpawnerOnTheGroundGameObject.transform)
                GameObject.Destroy(child.gameObject);
        if (fireSpawnerGameObject.transform.childCount != 0)
            foreach (Transform child in fireSpawnerGameObject.transform)
                GameObject.Destroy(child.gameObject);
    }
    internal void SpawnBottleOfVodka(float bottleThrowForce, Vector2 bottleThrowAngle)
    {
        if (currentPlaneState == PlaneState.standard)
        {
            int bottleVariant = Random.Range(0, gameplaySettings.bottlePrefab.Length);
            GameObject bottle = Object.Instantiate(gameplaySettings.bottlePrefab[bottleVariant], bottleSpawnerGameObject.transform.position, Quaternion.identity, projectilesParentGameObject.transform);
            bottle.GetComponent<BottleOfVodka>().SetParentObject(this);
            bottle.GetComponent<Rigidbody2D>().AddForce(bottleThrowAngle * bottleThrowForce);
            int randomDirection = Random.Range(0, 2);
            if (randomDirection == 0)
                bottle.GetComponent<Rigidbody2D>().AddTorque(180);
            else
                bottle.GetComponent<Rigidbody2D>().AddTorque(-180);
        }
    }
    internal void SoberUp()
    {
        bottlesDrunk = 0;
        difficultyImpulseEnabled = false;
        difficultyImpulseDirection = 1;
        difficultyImpulsTimeCurrent = 0;
    }
    internal void DamageThePlane()
    {
        currentPlaneState = PlaneState.damaged;
        planeRendererScript.ChangePlaneSprite(currentPlaneState);
        planeRendererScript.ChangeTilt(currentPlaneState, -1);
        audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(audioManagerScript.localOneLinersSounds);
        audioManagerScript.PlaySound("Whistle", audioManagerScript.localSFX);
        if (gameplaySettings.smokePrefab != null)
            Object.Instantiate(gameplaySettings.smokePrefab, smokeSpawnerInAirGameObject.transform.position, Quaternion.Euler(270, 0, 0), smokeSpawnerInAirGameObject.transform);
        if (gameplaySettings.explosionPrefab != null)
        {
            Object.Instantiate(gameplaySettings.explosionPrefab, planeGameObject.transform.position, Quaternion.identity, planeGameObject.transform);
            audioManagerScript.PlaySound("Explosion", audioManagerScript.localSFX);
        }
        int randomSoundEffect = Random.Range(0, audioManagerScript.localHitReactionSounds.Length);
        audioManagerScript.PlaySound("HitReaction" + randomSoundEffect, audioManagerScript.localHitReactionSounds);
    }
    internal void DestroyThePlane()
    {
        currentPlaneState = PlaneState.crashed;
        planeRendererScript.ChangePlaneSprite(currentPlaneState);
        planeRendererScript.ChangeTilt(currentPlaneState, -1);
        audioManagerScript.StopPlayingSound("Whistle", audioManagerScript.localSFX);
        audioManagerScript.StopPlayingSound("EngineSound", audioManagerScript.localSFX);
        if (gameplaySettings.smokePrefab != null)
        {
            if (smokeSpawnerInAirGameObject.transform.childCount != 0)
                foreach (Transform child in smokeSpawnerInAirGameObject.transform)
                    GameObject.Destroy(child.gameObject);
            if (smokeSpawnerOnTheGroundGameObject.transform.childCount == 0)
                Object.Instantiate(gameplaySettings.smokePrefab, smokeSpawnerOnTheGroundGameObject.transform.position, Quaternion.Euler(-90, 0, 0), smokeSpawnerOnTheGroundGameObject.transform);
            else
                foreach (Transform child in smokeSpawnerOnTheGroundGameObject.transform)
                    child.rotation = Quaternion.Euler(-90, 0, 0);
        }
        if (gameplaySettings.firePrefab != null && fireSpawnerGameObject.transform.childCount == 0)
        {
            GameObject fire = Object.Instantiate(gameplaySettings.firePrefab, fireSpawnerGameObject.transform.position, Quaternion.Euler(0, 0, 0), fireSpawnerGameObject.transform);
            fire.transform.localScale = new Vector3((float)0.2, (float)0.2, 1);
        }
        if (gameplaySettings.explosionPrefab != null)
        {
            Object.Instantiate(gameplaySettings.explosionPrefab, planeGameObject.transform.position, Quaternion.identity, planeGameObject.transform);
            audioManagerScript.PlaySound("Explosion", audioManagerScript.localSFX);
        }
    }
}