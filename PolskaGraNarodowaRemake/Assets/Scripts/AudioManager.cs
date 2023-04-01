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
    public GameObject planeControlCenterGameObject;
    public GameplaySettings gameplaySettings;
    //internal PlaneScript PlaneScriptScript;
    private float waitingTimeForOneLinerCurrent;
    private bool canPlayOneLiner;
    internal bool tiresSFXPlayed;
    internal bool landingSpeechPlayed;
    internal List<Sound> pausedSounds;
    public GameObject optionsMenuGameObject;
    private int lastPlayedOneLiner;
    private int lastPlayedLandingSound;

    void OnEnable()
    {
        LoadSounds(oneLinersSounds);
        LoadSounds(hitReactionSounds);
        LoadSounds(landingSounds);
        LoadSounds(SFX);
        LoadSounds(otherSounds);
        //if (planeControlCenterGameObject != null)
        //{
        //    PlaneScriptScript = planeControlCenterGameObject.GetComponent<PlaneScript>();
        //}
        if (gameplaySettings.waitingTimeForOneLiner == 0)
            gameplaySettings.waitingTimeForOneLiner = 5f;
        lastPlayedLandingSound = -1;
        lastPlayedOneLiner = -1;
        canPlayOneLiner = false;
        tiresSFXPlayed = false;
        landingSpeechPlayed = false;
        pausedSounds = new List<Sound>();
    }
    void Update()
    {
        //if (optionsMenuGameObject.activeSelf)
        //    UpdateAllSoundsVolume();
        //if (planeControlCenterGameObject != null && PlaneScriptScript != null && (PlaneScriptScript.currentPlaneState == PlaneScript.StateMachine.standard || PlaneScriptScript.currentPlaneState == PlaneScript.StateMachine.wheelsOn))
        //{
        //    if(!PlaneScriptScript.flightControllScript.isTouchingAirport)
        //    {
        //        waitingTimeForOneLinerCurrent += Time.deltaTime;
        //        if (waitingTimeForOneLinerCurrent >= gameplaySettings.waitingTimeForOneLiner)
        //        {
        //            canPlayOneLiner = true;
        //            waitingTimeForOneLinerCurrent = 0;
        //            for (int i = 0; i < oneLinersSounds.Length; i++)
        //            {
        //                if (IsTheSoundCurrentlyPlaying("OneLiner" + i, oneLinersSounds))
        //                {
        //                    canPlayOneLiner = false;
        //                    break;
        //                }
        //            }
        //            if (canPlayOneLiner)
        //            {
        //                int randomSoundEffect = Random.Range(0, oneLinersSounds.Length);
        //                while (randomSoundEffect == lastPlayedOneLiner)
        //                    randomSoundEffect = Random.Range(0, oneLinersSounds.Length);
        //                lastPlayedOneLiner = randomSoundEffect;
        //                PlaySound("OneLiner" + randomSoundEffect.ToString(), oneLinersSounds);
        //                waitingTimeForOneLinerCurrent -= ReturnSoundDuration("OneLiner" + randomSoundEffect, oneLinersSounds);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if(!tiresSFXPlayed)
        //        {
        //            tiresSFXPlayed = true;
        //            StopPlayingSoundsFromTheSpecificSoundBank(oneLinersSounds);
        //            StopPlayingSound("EngineSound", SFX);
        //            PlaySound("Tires", SFX);
        //        }
        //        if(PlaneScriptScript.flightControllScript.currentPlaneSpeed == 0 && !landingSpeechPlayed)
        //        {
        //            landingSpeechPlayed = true;
        //            StopPlayingSound("Tires", SFX);
        //            StopPlayingSoundsFromTheSpecificSoundBank(oneLinersSounds);
        //            int randomSoundEffect = Random.Range(0, landingSounds.Length);
        //            while (randomSoundEffect == lastPlayedLandingSound)
        //                randomSoundEffect = Random.Range(0, landingSounds.Length);
        //            lastPlayedLandingSound = randomSoundEffect;
        //            PlaySound("Landing" + randomSoundEffect.ToString(), landingSounds);
        //            PlaneScriptScript.flightControllScript.waitingTimeAfterLandingCombinedWithSoundLength = ReturnSoundDuration("Landing" + randomSoundEffect.ToString(), landingSounds);
        //        }
        //    }
        //}
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
        if(s == null)
        {
            Debug.LogWarning(soundName + " sound is missing in the " + soundsBank.ToString() + " soundsBank");
            return;
        }
        s.source.Play();
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
