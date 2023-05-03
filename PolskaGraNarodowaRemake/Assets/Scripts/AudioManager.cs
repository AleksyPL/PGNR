using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] oneLinersSounds;
    public Sound[] hitReactionSounds;
    public Sound[] landingSounds;
    public Sound[] SFX;
    public Sound[] otherSounds;
    public GameplaySettings gameplaySettings;
    public GameObject UIManagerGameObject;
    internal List<Sound> pausedSounds;
    internal bool landingSpeechPlayed;
    internal int lastPlayedOneLiner;
    internal int lastPlayedLandingSound;

    void OnEnable()
    {
        LoadSounds(oneLinersSounds);
        LoadSounds(hitReactionSounds);
        LoadSounds(landingSounds);
        LoadSounds(SFX);
        LoadSounds(otherSounds);
        PlaySound("TopGunTheme", otherSounds);
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
        foreach (Sound s in oneLinersSounds)
            s.source.volume = gameplaySettings.volumeQuotes * s.volumeSetInEditor;
        foreach (Sound s in hitReactionSounds)
            s.source.volume = gameplaySettings.volumeQuotes * s.volumeSetInEditor;
        foreach (Sound s in landingSounds)
            s.source.volume = gameplaySettings.volumeQuotes * s.volumeSetInEditor;
        foreach (Sound s in SFX)
            s.source.volume = gameplaySettings.volumeSFX * s.volumeSetInEditor;
        foreach (Sound s in otherSounds)
            s.source.volume = gameplaySettings.volumeMusic * s.volumeSetInEditor;
    }
    public void PlaySound(string soundName, Sound[] soundsBank)
    {
        Sound s = System.Array.Find(soundsBank, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning(soundName + " sound is missing in the " + soundsBank.ToString() + " soundsBank");
            return;
        }
        s.source.Play();
    }
    public void DrawAndPlayASound(Sound[] soundsBank, string fileNameBeginning, ref int excludedNumber)
    {
        if(!IsAnySoundFromTheSoundBankCurrentyPlaying(soundsBank, fileNameBeginning))
        {
            int randomSoundEffect = Random.Range(0, soundsBank.Length);
            if (excludedNumber != -1)
            {
                while (randomSoundEffect == excludedNumber)
                    randomSoundEffect = Random.Range(0, soundsBank.Length);
            }
            PlaySound(fileNameBeginning + randomSoundEffect.ToString(), soundsBank);
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
        foreach(Sound s in oneLinersSounds)
            if (s.source.isPlaying)
                s.source.Stop();
        foreach (Sound s in hitReactionSounds)
            if (s.source.isPlaying)
                s.source.Stop();
        foreach (Sound s in landingSounds)
            if (s.source.isPlaying)
                s.source.Stop();
        foreach (Sound s in SFX)
            if (s.source.isPlaying)
                s.source.Stop();
        foreach (Sound s in otherSounds)
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
        foreach (Sound s in oneLinersSounds)
            if (s.source.isPlaying)
            {
                s.source.Pause();
                pausedSounds.Add(s);
            }
        foreach (Sound s in hitReactionSounds)
            if (s.source.isPlaying)
            {
                s.source.Pause();
                pausedSounds.Add(s);
            }
        foreach (Sound s in landingSounds)
            if (s.source.isPlaying)
            {
                s.source.Pause();
                pausedSounds.Add(s);
            }
        foreach (Sound s in SFX)
            if (s.source.isPlaying)
            {
                s.source.Pause();
                pausedSounds.Add(s);
            }
        foreach (Sound s in otherSounds)
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
}
