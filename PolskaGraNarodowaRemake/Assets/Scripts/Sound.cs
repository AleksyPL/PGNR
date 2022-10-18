using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string name;
    [Range(0,1)]
    public float volumeSetInEditor;
    [Range(-3f, 3f)]
    public float pitch;
    public bool looping;
    [HideInInspector]
    public AudioSource source;
}
