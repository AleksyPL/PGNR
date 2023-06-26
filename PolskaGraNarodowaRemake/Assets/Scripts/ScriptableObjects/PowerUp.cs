using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/PowerUp")]
public class PowerUp : ScriptableObject
{
    public string powerUpDescriptionPL;
    public string powerUpDescriptionEN;
    public string powerUpDuration;
    public Image powerUpImage;
}
