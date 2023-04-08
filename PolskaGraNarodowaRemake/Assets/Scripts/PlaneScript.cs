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
    public GameObject bottlePrefab;
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
    internal float currentPlaneSpeed;
    internal float altitudeChangeForce;
    internal float timeToFullyChargeBottleThrowCounter;
    //Input
    internal float verticalMovementKeys;
    internal bool attackKeyPressed;
    internal bool attackKeyReleased;
    //Score
    internal int gameScore;
    internal void LoadPlaneData(int numberOfThePlayer)
    {
        planeRendererScript = planeRendererGameObject.GetComponent<PlaneRenderer>();
        playerNumber = numberOfThePlayer;
        ResetPlaneData();
    }
    internal void ResetPlaneData()
    {
        attackKeyPressed = false;
        attackKeyReleased = false;
        verticalMovementKeys = 0;
        isTouchingAirport = false;
        isTouchingGround = false;
        currentPlaneSpeed = gameplaySettings.defaultPlaneSpeed;
        altitudeChangeForce = gameplaySettings.altitudeChangeForce;
        bottleDrunkCounter = 0;
        difficultyImpulseEnabled = false;
        difficultyImpulseDirection = 1;
        difficultyImpulsTimeCurrent = 0;
        gameScore = 0;
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
            GameObject bottle = Object.Instantiate(bottlePrefab, bottleSpawnerGameObject.transform.position, Quaternion.identity, projectilesParentGameObject.transform);
            bottle.GetComponent<Rigidbody2D>().AddForce(bottleThrowAngle * bottleThrowForce);
            bottleDrunkCounter++;
        }
    }
    internal void DamageThePlane()
    {
        currentPlaneState = PlaneState.damaged;
        planeRendererScript.ChangePlaneSprite(currentPlaneState);
        planeRendererScript.ChangeTilt(currentPlaneState, -1);
        //baseScript.audioScript.StopPlayingSoundsFromTheSpecificSoundBank(baseScript.audioScript.oneLinersSounds);
        //baseScript.audioScript.PlaySound("Whistle", baseScript.audioScript.SFX);
        if (smokePrefab != null)
            Object.Instantiate(smokePrefab, smokeSpawnerGameObject.transform.position, Quaternion.Euler(270, 0, 0), smokeSpawnerGameObject.transform);
        if (explosionPrefab != null)
        {
            Object.Instantiate(explosionPrefab, planeGameObject.transform.position, Quaternion.identity, planeGameObject.transform);
            //baseScript.audioScript.PlaySound("Explosion", baseScript.audioScript.SFX);
        }
        //int randomSoundEffect = Random.Range(0, baseScript.audioScript.hitReactionSounds.Length);
        //baseScript.audioScript.PlaySound("HitReaction" + randomSoundEffect, baseScript.audioScript.hitReactionSounds);
    }
    internal void DestroyThePlane()
    {
        currentPlaneState = PlaneState.crashed;
        planeRendererScript.ChangePlaneSprite(currentPlaneState);
        planeRendererScript.ChangeTilt(currentPlaneState, -1);
        //baseScript.audioScript.StopPlayingSound("Whistle", baseScript.audioScript.SFX);
        //baseScript.audioScript.StopPlayingSound("EngineSound", baseScript.audioScript.SFX);
        if (smokePrefab != null && smokeSpawnerGameObject.transform.childCount == 0)
            Object.Instantiate(smokePrefab, smokeSpawnerGameObject.transform.position, Quaternion.Euler(270, 0, 0), smokeSpawnerGameObject.transform);
        if (explosionPrefab != null)
        {
            Object.Instantiate(explosionPrefab, planeGameObject.transform.position, Quaternion.identity, planeGameObject.transform);
            //baseScript.audioScript.PlaySound("Explosion", baseScript.audioScript.SFX);
        }
    }
}