using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/GameplaySettings")]
public class GameplaySettings : ScriptableObject
{
    [Header("Language Settings")]
    public int langauageIndex;
    [Header("Audio Settings")]
    public float volumeSFX;
    public float volumeQuotes;
    public float volumeMusic;
    public float waitingTimeForOneLiner;
    [Header("Bottle of Vodka Settings")]
    public float debrisSplashForce;
    [Header("Rewards")]
    public int rewardForLanding;
    public int rewardForHittingATarget;
    [Header("Camera Manager Settings")]
    public float cameraPositionXOffset;
    public float cameraDespawnDisatance;
    [Header("Fade Out Tool Settings")]
    public float fadeOutLifeTime;
    [Header("Level Manager Settings")]
    public float groundLevelHeight;
    public float topScreenHeight;
    [Header("Flight Controller Settings")]
    public float defaultPlaneSpeed;
    public float altitudeChangeForce;
    public float fallingForce;
    public float airportSlowingForce;
    public float waitingTimeAfterLanding;
    public float timeToFullyChargeBottleThrow;
    public float bottleThrowForceMin;
    public float bottleThrowForceMax;
    public Vector2 bottleThrowAngleMin;
    public Vector2 bottleThrowAngleMax;
    [Header("Trotyl Launcher Settings")]
    public float rateOfFire;
    public float maxLaunchDelay;
    public float launchForce;
}
