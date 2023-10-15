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
        ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "ShieldPowerUp");
        ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "SpeedPowerUp");
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "ShieldPowerUp");
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "SpeedPowerUp");
        }
    }

    void Update()
    {
        CheckPlayerShield(flightControllerScript.gameModeScript.playerOnePlane);
        CheckPlayerSpeed(flightControllerScript.gameModeScript.playerOnePlane);
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            CheckPlayerShield(flightControllerScript.gameModeScript.playerTwoPlane);
            CheckPlayerSpeed(flightControllerScript.gameModeScript.playerTwoPlane);
        }
    }
    internal void ResetDurationForThePowerUp(Plane plane, string powerUpName)
    {
        float powerUpCounter = 0;
        for (int i = 0; i < flightControllerScript.levelManagerScript.powerUpsPrefabs.Length; i++)
        {
            if(flightControllerScript.levelManagerScript.powerUpsPrefabs[i].name == powerUpName)
            {
                powerUpCounter = flightControllerScript.levelManagerScript.powerUpsPrefabs[i].GetComponent<PowerUpObject>().currentPowerUpScriptableObject.powerUpDuration;
                break;
            }
        }
        if (plane == flightControllerScript.gameModeScript.playerOnePlane)
        {
            if (powerUpName == "ShieldPowerUp")
                shieldPlayerOneCounter = powerUpCounter;
            else if (powerUpName == "SpeedPowerUp")
                speedPlayerOneCounter = powerUpCounter;
        }
        else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
        {
            if (powerUpName == "ShieldPowerUp")
                shieldPlayerTwoCounter = powerUpCounter;
            else if (powerUpName == "SpeedPowerUp")
                speedPlayerTwoCounter = powerUpCounter;
        }
    }
    private void CheckPlayerShield(Plane plane)
    {
        if (plane.shieldEnabled)
        {
            if(plane == flightControllerScript.gameModeScript.playerOnePlane)
            {
                shieldPlayerOneCounter -= Time.deltaTime;
                if (shieldPlayerOneCounter <= 0)
                {
                    plane.planeRendererScript.HideShield();
                    plane.shieldEnabled = false;
                    ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "ShieldPowerUp");
                }
            }
            else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
            {
                shieldPlayerTwoCounter -= Time.deltaTime;
                if (shieldPlayerTwoCounter <= 0)
                {
                    plane.planeRendererScript.HideShield();
                    plane.shieldEnabled = false;
                    ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "ShieldPowerUp");
                }
            }
        }
    }
    private void CheckPlayerSpeed(Plane plane)
    {
        if (plane.speedEnabled)
        {
            if (plane == flightControllerScript.gameModeScript.playerOnePlane)
            {
                speedPlayerOneCounter -= Time.deltaTime;
                if (speedPlayerOneCounter <= 0)
                {
                    plane.speedEnabled = false;
                    plane.currentPlaneSpeed = flightControllerScript.gameplaySettings.defaultPlaneSpeed;
                    ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "SpeedPowerUp");
                }
            }
            else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
            {
                speedPlayerTwoCounter -= Time.deltaTime;
                if (speedPlayerTwoCounter <= 0)
                {
                    plane.speedEnabled = false;
                    plane.currentPlaneSpeed = flightControllerScript.gameplaySettings.defaultPlaneSpeed;
                    ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "SpeedPowerUp");
                }
            }
        }
    }
}
