using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/GameplaySettings")]
public class GameplaySettings : ScriptableObject
{
    public int langauageIndex;
    internal float volumeSFX;
    internal float volumeQuotes;
    internal float volumeMusic;
    private void OnEnable()
    {
        langauageIndex = 0;
        volumeSFX = 1;
        volumeQuotes = 1;
        volumeMusic = 1;
    }
}
