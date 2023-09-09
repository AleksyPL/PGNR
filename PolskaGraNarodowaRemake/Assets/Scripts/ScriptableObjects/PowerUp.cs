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
    //public Sprite onlyPositivepowerUpImage;
    //public Sprite uknownPowerUpImage;
    //public Sprite fightAgainstEnemyPowerUpImage;
    //public enum PowerUpType
    //{
    //    onlyPositive,
    //    unknown,
    //    fightAgainstEnemy
    //}
    //public PowerUpType currentPowerUpType;
    //public void OnEnable()
    //{
    //    if (currentPowerUpType == PowerUpType.onlyPositive)
    //        currentPowerUpImage = onlyPositivepowerUpImage;
    //    else if (currentPowerUpType == PowerUpType.unknown)
    //        currentPowerUpImage = uknownPowerUpImage;
    //    else if (currentPowerUpType == PowerUpType.fightAgainstEnemy)
    //        currentPowerUpImage = fightAgainstEnemyPowerUpImage;
    //}
}
