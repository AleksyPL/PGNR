using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    internal Sound[] localOneLinersSounds;
    internal Sound[] localHitReactionSounds;
    internal Sound[] localLandingSounds;
    internal Sound[] localSFX;
    internal Sound[] localOtherSounds; 
    public SoundBank[] oneLiners;
    public SoundBank[] oneLinersSafe;
    public SoundBank[] hitReactions;
    public SoundBank[] hitReactionsSafe;
    public SoundBank[] landingSounds;
    public SoundBank[] landingSoundsSafe;
    public SoundBank otherSounds;
    public SoundBank otherSoundsSafe;
    public SoundBank SFX;
    public GameplaySettings gameplaySettings;
    public GameObject UIManagerGameObject;
    internal List<Sound> pausedSounds;
    internal bool landingSpeechPlayed;
    internal int lastPlayedOneLiner;
    internal int lastPlayedLandingSound;

    void OnEnable()
    {
        IdentifyWhichTracksAreOKToLoad();
        LoadSounds(localSFX);
        LoadSounds(localOtherSounds);
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            LoadSounds(localOneLinersSounds);
            LoadSounds(localHitReactionSounds);
            LoadSounds(localLandingSounds);
        }
        if (gameplaySettings.waitingTimeForOneLiner == 0)
            gameplaySettings.waitingTimeForOneLiner = 5f;
        lastPlayedLandingSound = -1;
        lastPlayedOneLiner = -1;
        landingSpeechPlayed = false;
        pausedSounds = new List<Sound>();
    }
    private void LoadSounds(Sound [] sounds)
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volumeSetInEditor;
            s.source.pitch = s.pitch;
            s.source.loop = s.looping;
        }
    }
    public void UpdateAllSoundsVolume()
    {
        foreach (Sound s in localOneLinersSounds)
            s.source.volume = gameplaySettings.volumeQuotes * s.volumeSetInEditor;
        foreach (Sound s in localHitReactionSounds)
            s.source.volume = gameplaySettings.volumeQuotes * s.volumeSetInEditor;
        foreach (Sound s in localLandingSounds)
            s.source.volume = gameplaySettings.volumeQuotes * s.volumeSetInEditor;
        foreach (Sound s in localSFX)
            s.source.volume = gameplaySettings.volumeSFX * s.volumeSetInEditor;
        foreach (Sound s in localOtherSounds)
            s.source.volume = gameplaySettings.volumeMusic * s.volumeSetInEditor;
    }
    public void PlaySound(string soundName, Sound[] localSoundsBank)
    {
        Sound s = System.Array.Find(localSoundsBank, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning(soundName + " sound is missing in the " + localSoundsBank.ToString() + " soundsBank");
            return;
        }
        s.source.Play();
    }
    public void DrawAndPlayASound(Sound[] localSoundsBank, string fileNameBeginning, ref int excludedNumber)
    {
        if(!IsAnySoundFromTheSoundBankCurrentyPlaying(localSoundsBank, fileNameBeginning))
        {
            int randomSoundEffect = Random.Range(0, localSoundsBank.Length);
            if (excludedNumber != -1)
            {
                while (randomSoundEffect == excludedNumber && localSoundsBank.Length != 1)
                    randomSoundEffect = Random.Range(0, localSoundsBank.Length);
            }
            PlaySound(fileNameBeginning + randomSoundEffect.ToString(), localSoundsBank);
            excludedNumber = randomSoundEffect;
        }
    }
    public float ReturnSoundDuration(string soundName, Sound[] soundsBank)
    {
        Sound s = System.Array.Find(soundsBank, sound => sound.name == soundName);
        if(s == null)
        {
            Debug.LogWarning(soundName + " sound is missing in the " + soundsBank.ToString() + " soundsBank");
            return 0;
        }
        return s.clip.length;
    }
    public bool IsTheSoundCurrentlyPlaying(string soundName, Sound[] soundsBank)
    {
        Sound s = System.Array.Find(soundsBank, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning(soundName + " sound is missing in the " + soundsBank.ToString() + " soundsBank");
            return false;
        }
        if (!s.source.isPlaying)
            return false;
        else
            return true;
    }
    public bool IsAnySoundFromTheSoundBankCurrentyPlaying(Sound[] soundsBank, string fileNameBeginning)
    {
        for(int i=0;i<soundsBank.Length;i++)
        {
            if (IsTheSoundCurrentlyPlaying(fileNameBeginning + i.ToString(), soundsBank))
                return true;
        }
        return false;
    }
    public void StopPlayingSound(string soundName, Sound[] soundsBank)
    {
        Sound s = System.Array.Find(soundsBank, sound => sound.name == soundName);
        if (s == null)
            Debug.LogWarning(soundName + " sound is missing in the " + soundsBank.ToString() + " soundsBank");
        else
        {
            if (s.source.isPlaying)
                s.source.Stop();
        }
    }
    public void StopPlayingSoundsFromTheSpecificSoundBank(Sound[] soundsBank)
    {
        foreach (Sound s in soundsBank)
            if (s.source.isPlaying)
                s.source.Stop();
    }
    public void StopPlayingAllSounds()
    {
        foreach(Sound s in localOneLinersSounds)
            if (s.source.isPlaying)
                s.source.Stop();
        foreach (Sound s in localHitReactionSounds)
            if (s.source.isPlaying)
                s.source.Stop();
        foreach (Sound s in localLandingSounds)
            if (s.source.isPlaying)
                s.source.Stop();
        foreach (Sound s in localSFX)
            if (s.source.isPlaying)
                s.source.Stop();
        foreach (Sound s in localOtherSounds)
            if (s.source.isPlaying)
                s.source.Stop();
    }
    public void PausePlayingSoundsFromTheSpecificSoundBank(Sound[] soundsBank)
    {
        foreach (Sound s in soundsBank)
            if (s.source.isPlaying)
            {
                s.source.Pause();
                pausedSounds.Add(s);
            }
    }
    public void PausePlayingAllSounds()
    {
        foreach (Sound s in localOneLinersSounds)
            if (s.source.isPlaying)
            {
                s.source.Pause();
                pausedSounds.Add(s);
            }
        foreach (Sound s in localHitReactionSounds)
            if (s.source.isPlaying)
            {
                s.source.Pause();
                pausedSounds.Add(s);
            }
        foreach (Sound s in localLandingSounds)
            if (s.source.isPlaying)
            {
                s.source.Pause();
                pausedSounds.Add(s);
            }
        foreach (Sound s in localSFX)
            if (s.source.isPlaying)
            {
                s.source.Pause();
                pausedSounds.Add(s);
            }
        foreach (Sound s in localOtherSounds)
            if (s.source.isPlaying)
            {
                s.source.Pause();
                pausedSounds.Add(s);
            }
    }
    public void ResumeAllPausedSounds()
    {
        if(pausedSounds.Count != 0)
        {
            foreach (Sound s in pausedSounds)
                s.source.UnPause();
            pausedSounds.Clear();
        }
    }
    private void IdentifyWhichTracksAreOKToLoad()
    {
        localSFX = SFX.sounds;
        if (gameplaySettings.safeMode)
            localOtherSounds = otherSoundsSafe.sounds;
        else
            localOtherSounds = otherSounds.sounds;
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (gameplaySettings.safeMode)
            {
                localOneLinersSounds = oneLinersSafe[gameplaySettings.langauageIndex].sounds;
                localLandingSounds = landingSoundsSafe[gameplaySettings.langauageIndex].sounds;
                localHitReactionSounds = hitReactionsSafe[gameplaySettings.langauageIndex].sounds;
            }
            else
            {
                localOneLinersSounds = oneLiners[gameplaySettings.langauageIndex].sounds;
                localLandingSounds = landingSounds[gameplaySettings.langauageIndex].sounds;
                localHitReactionSounds = hitReactions[gameplaySettings.langauageIndex].sounds;
            }
        }
    }
}
