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
    internal PlaneState currentPlaneState;
    public GameplaySettings gameplaySettings;
    public GameObject planeGameObject;
    public GameObject bottleSpawnerGameObject;
    public GameObject smokeSpawnerGameObject;
    public GameObject planeRendererGameObject;
    public GameObject projectilesParentGameObject;
    public GameObject cameraGameObject;
    public GameObject bottlePrefab;
    public GameObject smokePrefab;
    public GameObject explosionPrefab;
    internal float groundLevelHeight;
    internal float topScreenHeight;
    internal bool isTouchingAirport;
    internal bool isTouchingGround;
    internal int bottleDrunkCounter;
    internal int playerNumber;
    internal float currentPlaneSpeed;
    internal float altitudeChangeForce;
    internal PlaneRenderer planeRendererScript;
    internal float timeToFullyChargeBottleThrowCounter;
    internal int gameScore;
    internal void LoadPlaneData(int numberOfThePlayer)
    {
        isTouchingAirport = false;
        isTouchingGround = false;
        currentPlaneState = PlaneState.standard;
        bottleDrunkCounter = 0;
        currentPlaneSpeed = gameplaySettings.defaultPlaneSpeed;
        altitudeChangeForce = gameplaySettings.altitudeChangeForce;
        planeRendererScript = planeRendererGameObject.GetComponent<PlaneRenderer>();
        playerNumber = numberOfThePlayer;
        timeToFullyChargeBottleThrowCounter = 0;
        gameScore = 0;
    }
    internal void ThrowBottleOfVodka(float bottleThrowForce, Vector2 bottleThrowAngle)
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
        if (smokePrefab != null)
            Object.Instantiate(smokePrefab, smokeSpawnerGameObject.transform.position, Quaternion.Euler(270, 0, 0), smokeSpawnerGameObject.transform);
        if (explosionPrefab != null)
        {
            Object.Instantiate(explosionPrefab, planeGameObject.transform.position, Quaternion.identity, planeGameObject.transform);
            //baseScript.audioScript.PlaySound("Explosion", baseScript.audioScript.SFX);
        }
    }
}


//public class PlaneScript : MonoBehaviour
//{
//    //[SerializeField] internal Plane plane11 = new Plane();
//    //public GameObject inputManagerGameObject;
//    //public GameObject UIGameObject;
//    //public GameObject levelManagerGameObject;
//    //public GameObject audioManagerGameObject;
//    public GameObject gameModeManagerObject;
//    public GameObject flighControllerGameObject;
    

    
//    internal GameModeManager gameModeScript;
//    internal int playerNumber;
//    //internal InputManager inputScript;
//    //internal AudioManager audioScript;
    
//    //internal LevelManager levelManagerScript;
//    internal FlightController flightControllScript;
//    //internal UIManager UIScript;
//    //internal PlaneRenderer planeRendererScript;
//    void OnEnable()
//    {
//        gameModeScript = gameModeManagerObject.GetComponent<GameModeManager>();
//        //planeRendererScript = planeRendererGameObject.GetComponent<PlaneRenderer>();
//        flightControllScript = flighControllerGameObject.GetComponent<FlightController>();
//        //difficultyScript = GetComponent<DifficultyManager>();
//        //levelManagerScript = levelManagerGameObject.GetComponent<LevelManager>();
//        //audioScript = audioManagerGameObject.GetComponent<AudioManager>();
//        //UIScript = UIGameObject.GetComponent<UIManager>();
//        //inputScript = inputManagerGameObject.GetComponent<InputManager>();
//    }
    
//}
