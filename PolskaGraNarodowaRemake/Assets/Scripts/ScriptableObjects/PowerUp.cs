using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/PowerUp")]
public class PowerUp : ScriptableObject
{
    public string powerUpName;
    public string[] powerUpDescription;
    public float powerUpDuration;
    public Sprite currentPowerUpImageBox;
    public Sprite currentPowerUpUIClockImage;
}
