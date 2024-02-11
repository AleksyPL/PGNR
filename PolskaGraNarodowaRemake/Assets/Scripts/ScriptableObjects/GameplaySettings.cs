using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    public Localization[] localizationStringsSafe;
    public Localization[] localizationStringsNotSafe;
    internal Localization[] localizationsStrings;
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
    public float camerePositionXOffsetPersentageSingle;
    public float camerePositionXOffsetPersentageMulti;
    internal float cameraPositionXOffset;
    [Header("Fade Tool Settings")]
    public float fadeOutLifeTime;
    public float fadeInLifeTime;
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
    public float durationTimeForPowerUpMessageOnTheScreen;
    public int gettingWastedXTimesMoreNumber;
    public float multishotSpread;
    [Header("Other")]
    public bool scriptableObjectSafeMod;
    internal bool safeMode;

    private void OnEnable()
    {
        safeMode = scriptableObjectSafeMod;
#if !UNITY_EDITOR
        safeMode = true;
        GetArguments();
#endif
        if (currentLanguage == Languages.Polski)
            langauageIndex = 0;
        else if (currentLanguage == Languages.English)
            langauageIndex = 1;
        ResetGameVolume();
        ResetPlayerSkins();
        LoadLocalizationData();
    }

    private void ResetGameVolume()
    {
        volumeSFX = 1;
        volumeQuotes = 1;
        volumeMusic = 1;
    }

    private void LoadLocalizationData()
    {
        if(localizationStringsSafe.Length == localizationStringsNotSafe.Length)
        {
            if (safeMode)
                localizationsStrings = localizationStringsSafe;
            else
                localizationsStrings = localizationStringsNotSafe;
            for(int i=0;i<localizationsStrings.Length;i++)
                localizationsStrings[i].LoadData();
        }
        else
            Application.Quit();
    }
    internal void ResetPlayerSkins()
    {
        playersPlaneSkins[0] = 0;
        playersPlaneSkins[1] = 1;
    }
    private string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }
    private void GetArguments()
    {
#if UNITY_WEBGL
        string parametersInTheWholeLink = Application.absoluteURL.Substring(Application.absoluteURL.IndexOf("?") + 1);
        string[] arguments = parametersInTheWholeLink.Split(new char[] { '?' });
#else
        string[] arguments = Environment.GetCommandLineArgs();
#endif
        if (arguments.Length > 0)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i] = RemoveSpecialCharacters(arguments[i]);
                if (arguments[i] == "TrueGame")
                    safeMode = false;
                if (arguments[i] == "EN")
                    langauageIndex = 1;
            }
        }
    }
}
