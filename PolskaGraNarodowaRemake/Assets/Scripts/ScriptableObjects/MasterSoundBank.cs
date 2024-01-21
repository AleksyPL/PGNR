using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "ScriptableObjects/MasterSoundBank")]
public class MasterSoundBank : ScriptableObject
{
    public SoundBank[] oneLiners;
    public SoundBank[] oneLinersSafe;
    public SoundBank[] hitReactions;
    public SoundBank[] hitReactionsSafe;
    public SoundBank[] landingSounds;
    public SoundBank[] landingSoundsSafe;
    public SoundBank otherSounds;
    public SoundBank otherSoundsSafe;
    public SoundBank SFX;
    internal AudioManager audioManagerScript;
    internal GameplaySettings gameplaySettings;
    internal void IdentifyWhichTracksAreOKToLoad()
    {
        audioManagerScript.localSFX = SFX.sounds;
        if (gameplaySettings.safeMode)
            audioManagerScript.localOtherSounds = otherSoundsSafe.sounds;
        else
            audioManagerScript.localOtherSounds = otherSounds.sounds;
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (gameplaySettings.safeMode)
            {
                audioManagerScript.localOneLinersSounds = oneLinersSafe[gameplaySettings.langauageIndex].sounds;
                audioManagerScript.localLandingSounds = landingSoundsSafe[gameplaySettings.langauageIndex].sounds;
                audioManagerScript.localHitReactionSounds = hitReactionsSafe[gameplaySettings.langauageIndex].sounds;
            }
            else
            {
                audioManagerScript.localOneLinersSounds = oneLiners[gameplaySettings.langauageIndex].sounds;
                audioManagerScript.localLandingSounds = landingSounds[gameplaySettings.langauageIndex].sounds;
                audioManagerScript.localHitReactionSounds = hitReactions[gameplaySettings.langauageIndex].sounds;
            }
        }
    }
}
