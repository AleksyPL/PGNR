using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    internal FlightController flightControllerScript;
    //shield
    internal float shieldPlayerOneCounter;
    internal float shieldPlayerTwoCounter;
    internal float speedPlayerOneCounter;
    internal float speedPlayerTwoCounter;


    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        ResetDurationForThePowerUp("ShieldPowerUp", ref shieldPlayerOneCounter);
        ResetDurationForThePowerUp("SpeedPowerUp", ref speedPlayerOneCounter);
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            ResetDurationForThePowerUp("ShieldPowerUp", ref shieldPlayerTwoCounter);
            ResetDurationForThePowerUp("SpeedPowerUp", ref speedPlayerTwoCounter);
        }
    }

    void Update()
    {
        CheckPlayersShield(ref flightControllerScript.gameModeScript.playerOnePlane, ref shieldPlayerOneCounter);
        CheckPlayersSpeed(ref flightControllerScript.gameModeScript.playerOnePlane, ref speedPlayerOneCounter);
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            CheckPlayersShield(ref flightControllerScript.gameModeScript.playerTwoPlane, ref shieldPlayerTwoCounter);
            CheckPlayersSpeed(ref flightControllerScript.gameModeScript.playerTwoPlane, ref speedPlayerTwoCounter);
        }
    }
    internal void ResetDurationForThePowerUp(string powerUpName, ref float powerUpCounter)
    {
        for (int i = 0; i < flightControllerScript.levelManagerScript.powerUpsPrefabs.Length; i++)
        {
            if(flightControllerScript.levelManagerScript.powerUpsPrefabs[i].name == powerUpName)
            {
                powerUpCounter = flightControllerScript.levelManagerScript.powerUpsPrefabs[i].GetComponent<PowerUpObject>().currentPowerUpScriptableObject.powerUpDuration;
                break;
            }
        }
    }
    private void CheckPlayersShield(ref Plane plane, ref float shieldDurationCounter)
    {
        if (plane.shieldEnabled)
        {
            shieldDurationCounter -= Time.deltaTime;
            if (shieldPlayerOneCounter <= 0)
            {
                plane.planeRendererScript.HideShield();
                plane.shieldEnabled = false;
                ResetDurationForThePowerUp("ShieldPowerUp", ref shieldDurationCounter);
            }
        }
    }
    private void CheckPlayersSpeed(ref Plane plane, ref float speedDurationCounter)
    {
        if (plane.speedEnabled)
        {
            speedDurationCounter -= Time.deltaTime;
            if (speedPlayerOneCounter <= 0)
            {
                plane.speedEnabled = false;
                plane.currentPlaneSpeed = flightControllerScript.gameplaySettings.defaultPlaneSpeed;
                ResetDurationForThePowerUp("SpeedPowerUp", ref speedDurationCounter);
            }
        }
    }
}
