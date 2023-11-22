using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SoundBank")]
public class SoundBank : ScriptableObject
{
    public Sound[] sounds;
}
