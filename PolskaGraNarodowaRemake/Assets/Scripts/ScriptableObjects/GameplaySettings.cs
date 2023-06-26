using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Languages
{
    Polski,
    English
}

[CreateAssetMenu(menuName ="ScriptableObjects/GameplaySettings")]
public class GameplaySettings : ScriptableObject
{
    [Header("Language Settings")]
    public Languages currentLanguage;
    internal int langauageIndex;
    public Localization[] localizationsStrings;
    [Header("Skins Settings")]
    public PlaneSkin[] planeSkins;
    internal int [] playersPlaneSkins = new int[2];
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
    public float cameraPositionXOffsetSingle;
    public float cameraPositionXOffsetMulti;
    public float cameraDespawnDisatance;
    internal float cameraPositionXOffset;
    [Header("Fade Out Tool Settings")]
    public float fadeOutLifeTime;
    [Header("Flight Controller Settings")]
    public float defaultPlaneSpeed;
    public float altitudeChangeForce;
    public float fallingForce;
    public float airportSlowingForce;
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
    [Header("Prefabs")]
    public GameObject[] bottlePrefab;
    public GameObject smokePrefab;
    public GameObject firePrefab;
    public GameObject explosionPrefab;
    [Header("PowerUps")]
    public PowerUp[] powerUps;
    private void OnEnable()
    {
        if (currentLanguage == Languages.Polski)
            langauageIndex = 0;
        else if (currentLanguage == Languages.English)
            langauageIndex = 1;
        ResetPlayerSkins();
    }
    internal void ResetPlayerSkins()
    {
        playersPlaneSkins[0] = 0;
        playersPlaneSkins[1] = 1;
    }
}
