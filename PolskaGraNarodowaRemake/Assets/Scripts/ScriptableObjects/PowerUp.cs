using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/PowerUp")]
public class PowerUp : ScriptableObject
{
    public string powerUpName;
    public string[] powerUpDescription;
    public string[] powerUpAlternativeDescription;
    public string[] powerUpSafeDescription;
    public string[] powerUpAlternativeSafeDescription;
    public float powerUpDuration;
    public Sprite currentPowerUpImageBox;
    public Sprite currentPowerUpUIClockImage;
    public Sprite currentPowerUpSafeUIClockImage;
}
