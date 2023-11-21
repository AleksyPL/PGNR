using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    internal FlightController flightControllerScript;
    //shield
    internal float shieldPlayerOneCounter;
    internal float shieldPlayerTwoCounter;
    //speed
    internal float speedPlayerOneCounter;
    internal float speedPlayerTwoCounter;
    //inverted steering
    internal float steeringPlayerOneCounter;
    internal float steeringPlayerTwoCounter;
    //getting wasted X times more
    internal float gettingWastedXTimesMorePlayerOneCounter;
    internal float gettingWastedXTimesMorePlayerTwoCounter;
    //multiShot
    internal float multiShotPlayerOneCounter;
    internal float multiShotPlayerTwoCounter;
    void Start()
    {
        
        flightControllerScript = GetComponent<FlightController>();
        ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "ShieldPowerUp");
        ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "SpeedPowerUp");
        ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "InvertedSteeringPowerUp");
        ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "GettingWastedXTimesMorePowerUp");
        ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "MultiShotPowerUp");
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "ShieldPowerUp");
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "SpeedPowerUp");
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "InvertedSteeringPowerUp");
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "GettingWastedXTimesMorePowerUp");
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "MultiShotPowerUp");
        }
    }

    void Update()
    {
        if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.versusClassic)
        {
            CheckPlayerShield(flightControllerScript.gameModeScript.playerOnePlane);
            CheckPlayerSpeed(flightControllerScript.gameModeScript.playerOnePlane);
            CheckInvertedSteering(flightControllerScript.gameModeScript.playerOnePlane);
            CheckGettingWastedXTimesMore(flightControllerScript.gameModeScript.playerOnePlane);
            CheckMultiShot(flightControllerScript.gameModeScript.playerOnePlane);
        }
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            CheckPlayerShield(flightControllerScript.gameModeScript.playerTwoPlane);
            CheckPlayerSpeed(flightControllerScript.gameModeScript.playerTwoPlane);
            CheckInvertedSteering(flightControllerScript.gameModeScript.playerTwoPlane);
            CheckGettingWastedXTimesMore(flightControllerScript.gameModeScript.playerTwoPlane);
            CheckMultiShot(flightControllerScript.gameModeScript.playerTwoPlane);
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
            else if (powerUpName == "InvertedSteeringPowerUp")
                steeringPlayerOneCounter = powerUpCounter;
            else if (powerUpName == "GettingWastedXTimesMorePowerUp")
                gettingWastedXTimesMorePlayerOneCounter = powerUpCounter;
            else if (powerUpName == "MultiShotPowerUp")
                multiShotPlayerOneCounter = powerUpCounter;
        }
        else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
        {
            if (powerUpName == "ShieldPowerUp")
                shieldPlayerTwoCounter = powerUpCounter;
            else if (powerUpName == "SpeedPowerUp")
                speedPlayerTwoCounter = powerUpCounter;
            else if (powerUpName == "InvertedSteeringPowerUp")
                steeringPlayerTwoCounter = powerUpCounter;
            else if (powerUpName == "GettingWastedXTimesMorePowerUp")
                gettingWastedXTimesMorePlayerTwoCounter = powerUpCounter;
            else if (powerUpName == "MultiShotPowerUp")
                multiShotPlayerTwoCounter = powerUpCounter;
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
    private void CheckInvertedSteering(Plane plane)
    {
        if (plane.invertedSteeringEnabled)
        {
            if (plane == flightControllerScript.gameModeScript.playerOnePlane)
            {
                steeringPlayerOneCounter -= Time.deltaTime;
                if (steeringPlayerOneCounter <= 0)
                {
                    plane.invertedSteeringEnabled = false;
                    ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "InvertedSteeringPowerUp");
                }
            }
            else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
            {
                steeringPlayerTwoCounter -= Time.deltaTime;
                if (steeringPlayerTwoCounter <= 0)
                {
                    plane.invertedSteeringEnabled = false;
                    ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "InvertedSteeringPowerUp");
                }
            }
        }
    }
    private void CheckGettingWastedXTimesMore(Plane plane)
    {
        if (plane.gettingWastedXTimesMoreEnabled)
        {
            if (plane == flightControllerScript.gameModeScript.playerOnePlane)
            {
                gettingWastedXTimesMorePlayerOneCounter -= Time.deltaTime;
                if (gettingWastedXTimesMorePlayerOneCounter <= 0)
                {
                    plane.gettingWastedXTimesMoreEnabled = false;
                    ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "GettingWastedXTimesMorePowerUp");
                }
            }
            else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
            {
                gettingWastedXTimesMorePlayerTwoCounter -= Time.deltaTime;
                if (gettingWastedXTimesMorePlayerTwoCounter <= 0)
                {
                    plane.gettingWastedXTimesMoreEnabled = false;
                    ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "GettingWastedXTimesMorePowerUp");
                }
            }
        }
    }
    private void CheckMultiShot(Plane plane)
    {
        if (plane.multiShotEnabled)
        {
            if (plane == flightControllerScript.gameModeScript.playerOnePlane)
            {
                multiShotPlayerOneCounter -= Time.deltaTime;
                if (multiShotPlayerOneCounter <= 0)
                {
                    plane.multiShotEnabled = false;
                    ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "MultiShotPowerUp");
                }
            }
            else if (plane == flightControllerScript.gameModeScript.playerTwoPlane)
            {
                multiShotPlayerTwoCounter -= Time.deltaTime;
                if (multiShotPlayerTwoCounter <= 0)
                {
                    plane.multiShotEnabled = false;
                    ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "MultiShotPowerUp");
                }
            }
        }
    }
}
