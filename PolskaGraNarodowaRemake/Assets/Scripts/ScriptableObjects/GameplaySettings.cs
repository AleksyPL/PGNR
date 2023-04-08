using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Languages
{
    Polish,
    English
}

[CreateAssetMenu(menuName ="ScriptableObjects/GameplaySettings")]
public class GameplaySettings : ScriptableObject
{
    [Header("Language Settings")]
    public Languages currentLanguage;
    internal int langauageIndex;
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
    public int rewardPerSecond;
    [Header("Camera Manager Settings")]
    public float cameraPositionXOffset;
    public float cameraDespawnDisatance;
    [Header("Fade Out Tool Settings")]
    public float fadeOutLifeTime;
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
    [Header("Difficulty Manager Settings")]
    public float altitudeChangeForceOverridedMultiplier;
    public float difficultyImpulseForce;
    private void OnEnable()
    {
        if (currentLanguage == Languages.Polish)
            langauageIndex = 0;
        else if (currentLanguage == Languages.English)
            langauageIndex = 1;
    }
}
