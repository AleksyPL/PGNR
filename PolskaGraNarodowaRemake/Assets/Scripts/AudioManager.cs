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
    public MasterSoundBank masterSoundBank;
    public GameplaySettings gameplaySettings;
    public GameObject UIManagerGameObject;
    internal List<Sound> pausedSounds;
    internal bool landingSpeechPlayed;
    internal int lastPlayedOneLiner;
    internal int lastPlayedLandingSound;

    void OnEnable()
    {
        masterSoundBank.audioManagerScript = GetComponent<AudioManager>();
        masterSoundBank.gameplaySettings = gameplaySettings;
        masterSoundBank.IdentifyWhichTracksAreOKToLoad();
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
    void OnApplicationPause(bool pause)
    {
        PausePlayingAllSounds();
    }
    void OnApplicationFocus(bool focus)
    {
        ResumeAllPausedSounds();
    }
    private void LoadSounds(Sound [] sounds)
    {
        if (sounds.Length != 0)
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                if (UnityEngine.Device.Application.isMobilePlatform)
                    s.source.volume = s.volumeMobile;
                else
                    s.source.volume = s.volumePC;
                s.source.pitch = s.pitch;
                s.source.loop = s.looping;
            }
    }
    public void UpdateAllSoundsVolume()
    {
        if(localOneLinersSounds != null)
            foreach (Sound s in localOneLinersSounds)
            {
                if(UnityEngine.Device.Application.isMobilePlatform)
                    s.source.volume = gameplaySettings.volumeQuotes * s.volumeMobile;
                else
                    s.source.volume = gameplaySettings.volumeQuotes * s.volumePC;
            }
        if(localHitReactionSounds != null)
            foreach (Sound s in localHitReactionSounds)
            {
                if (UnityEngine.Device.Application.isMobilePlatform)
                    s.source.volume = gameplaySettings.volumeQuotes * s.volumeMobile;
                else
                    s.source.volume = gameplaySettings.volumeQuotes * s.volumePC;
            }
        if (localLandingSounds != null)
            foreach (Sound s in localLandingSounds)
            {
                if (UnityEngine.Device.Application.isMobilePlatform)
                    s.source.volume = gameplaySettings.volumeQuotes * s.volumeMobile;
                else
                    s.source.volume = gameplaySettings.volumeQuotes * s.volumePC;
            }
        if (localSFX != null)
            foreach (Sound s in localSFX)
            {
                if (UnityEngine.Device.Application.isMobilePlatform)
                    s.source.volume = gameplaySettings.volumeSFX * s.volumeMobile;
                else
                    s.source.volume = gameplaySettings.volumeSFX * s.volumePC;
            }
        if (localOtherSounds != null)
            foreach (Sound s in localOtherSounds)
            {
                if (UnityEngine.Device.Application.isMobilePlatform)
                    s.source.volume = gameplaySettings.volumeMusic * s.volumeMobile;
                else
                    s.source.volume = gameplaySettings.volumeMusic * s.volumePC;
            }
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
        if (localOneLinersSounds != null)
            foreach (Sound s in localOneLinersSounds)
                if (s.source.isPlaying)
                    s.source.Stop();
        if (localHitReactionSounds != null)
            foreach (Sound s in localHitReactionSounds)
                if (s.source.isPlaying)
                    s.source.Stop();
        if (localLandingSounds != null)
            foreach (Sound s in localLandingSounds)
                if (s.source.isPlaying)
                    s.source.Stop();
        if (localSFX != null)
            foreach (Sound s in localSFX)
                if (s.source.isPlaying)
                    s.source.Stop();
        if (localOtherSounds != null)
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
        if (localOneLinersSounds != null)
            foreach (Sound s in localOneLinersSounds)
                if (s.source.isPlaying)
                {
                    s.source.Pause();
                    pausedSounds.Add(s);
                }
        if (localHitReactionSounds != null)
            foreach (Sound s in localHitReactionSounds)
                if (s.source.isPlaying)
                {
                    s.source.Pause();
                    pausedSounds.Add(s);
                }
        if (localLandingSounds != null)
            foreach (Sound s in localLandingSounds)
                if (s.source.isPlaying)
                {
                    s.source.Pause();
                    pausedSounds.Add(s);
                }
        if (localSFX != null)
            foreach (Sound s in localSFX)
                if (s.source.isPlaying)
                {
                    s.source.Pause();
                    pausedSounds.Add(s);
                }
        if (localOtherSounds != null)
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
    internal void StopPlayingAllPausedSounds()
    {
        if (pausedSounds.Count != 0)
            pausedSounds.Clear();
    }
}
