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
    public GameObject planeGameObject;
    internal PlaneBase planeBaseScript;
    [SerializeField] internal float waitingTimeForOneLiner;
    private float waitingTimeForOneLinerCurrent;
    private bool canPlayOneLiner;
    internal bool tiresSFXPlayed;
    internal bool landingSpeechPlayed;
    void Awake()
    {
        LoadSounds(oneLinersSounds);
        LoadSounds(hitReactionSounds);
        LoadSounds(landingSounds);
        LoadSounds(SFX);
        LoadSounds(otherSounds);
    }
    void Start()
    {
        if (planeGameObject != null)
        {
            planeBaseScript = planeGameObject.GetComponent<PlaneBase>();
        }
        if (waitingTimeForOneLiner == 0)
            waitingTimeForOneLiner = 5f;
        canPlayOneLiner = false;
        tiresSFXPlayed = false;
        landingSpeechPlayed = false;
    }
    void Update()
    {
        if (planeGameObject != null && planeBaseScript != null && (planeBaseScript.currentPlaneState == PlaneBase.StateMachine.standard || planeBaseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn))
        {
            if(!planeBaseScript.flightControllScript.isTouchingAirport)
            {
                waitingTimeForOneLinerCurrent += Time.deltaTime;
                if (waitingTimeForOneLinerCurrent >= waitingTimeForOneLiner)
                {
                    canPlayOneLiner = true;
                    waitingTimeForOneLinerCurrent = 0;
                    for (int i = 0; i < oneLinersSounds.Length; i++)
                    {
                        if (IsTheSoundCurrentlyPlaying("OneLiner" + i, oneLinersSounds))
                        {
                            canPlayOneLiner = false;
                            break;
                        }
                    }
                    if (canPlayOneLiner)
                    {
                        int randomSoundEffect = Random.Range(0, oneLinersSounds.Length);
                        PlaySound("OneLiner" + randomSoundEffect.ToString(), oneLinersSounds);
                        waitingTimeForOneLinerCurrent -= ReturnSoundDuration("OneLiner" + randomSoundEffect, oneLinersSounds);
                    }
                }
            }
            else
            {
                if(!tiresSFXPlayed)
                {
                    tiresSFXPlayed = true;
                    StopPlayingSoundsFromTheSpecificSoundBank(oneLinersSounds);
                    StopPlayingSound("EngineSound", SFX);
                    PlaySound("Tires", SFX);
                }
                if(planeBaseScript.flightControllScript.currentPlaneSpeed == 0 && !landingSpeechPlayed)
                {
                    landingSpeechPlayed = true;
                    StopPlayingAllSounds();
                    int randomSoundEffect = Random.Range(0, landingSounds.Length);
                    PlaySound("Landing" + randomSoundEffect.ToString(), landingSounds);
                    planeBaseScript.flightControllScript.waitingTimeAfterLandingCombinedWithSoundLength = ReturnSoundDuration("Landing" + randomSoundEffect.ToString(), landingSounds);
                }
            }
        }
    }
    private void LoadSounds(Sound [] sounds)
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.looping;
        }
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
}
