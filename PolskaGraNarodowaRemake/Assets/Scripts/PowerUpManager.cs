using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    internal class PlayerPowerUpController
    {
        internal float shieldCounter;
        internal float speedCounter;
        internal float steeringCounter;
        internal float gettingWastedXTimesMoreCounter;
        internal float multiShotCounter;
        internal PlayerPowerUpController()
        {
            shieldCounter = 0;
            speedCounter = 0;
            steeringCounter = 0;
            gettingWastedXTimesMoreCounter = 0;
            multiShotCounter = 0;
        }
    };
    internal FlightController flightControllerScript;
    private PlayerPowerUpController playerOnePowerUpController;
    private PlayerPowerUpController playerTwoPowerUpController;
    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
    }

    void Update()
    {
        if ((flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.tutorial) && !flightControllerScript.uiManagerScript.pauseScreenEnabled && !flightControllerScript.uiManagerScript.tutorialScreenEnabled)
        {
            CheckPlayerShield(flightControllerScript.gameModeScript.playerOnePlane);
            CheckPlayerSpeed(flightControllerScript.gameModeScript.playerOnePlane);
            CheckInvertedSteering(flightControllerScript.gameModeScript.playerOnePlane);
            CheckGettingWastedXTimesMore(flightControllerScript.gameModeScript.playerOnePlane);
            CheckMultiShot(flightControllerScript.gameModeScript.playerOnePlane);
            flightControllerScript.uiManagerScript.DisablePowerUpMessage(flightControllerScript.gameModeScript.playerOnePlane);
            if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
            {
                CheckPlayerShield(flightControllerScript.gameModeScript.playerTwoPlane);
                CheckPlayerSpeed(flightControllerScript.gameModeScript.playerTwoPlane);
                CheckInvertedSteering(flightControllerScript.gameModeScript.playerTwoPlane);
                CheckGettingWastedXTimesMore(flightControllerScript.gameModeScript.playerTwoPlane);
                CheckMultiShot(flightControllerScript.gameModeScript.playerTwoPlane);
                flightControllerScript.uiManagerScript.DisablePowerUpMessage(flightControllerScript.gameModeScript.playerTwoPlane);
            }
        }
    }
    private ref PlayerPowerUpController ReturnPlayerPowerUpControllerObject(Plane plane)
    {
        if (plane == flightControllerScript.gameModeScript.playerOnePlane)
            return ref playerOnePowerUpController;
        return ref playerTwoPowerUpController;
    }
    internal void ResetPowerUpManager()
    {
        flightControllerScript = GetComponent<FlightController>();
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            playerOnePowerUpController = new PlayerPowerUpController();
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "ShieldPowerUp");
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "SpeedPowerUp");
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "InvertedSteeringPowerUp");
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "GettingWastedXTimesMorePowerUp");
            ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerOnePlane, "MultiShotPowerUp");
            flightControllerScript.uiManagerScript.ClearPowerUpBar(flightControllerScript.gameModeScript.playerOnePlane);
            if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
            {
                playerTwoPowerUpController = new PlayerPowerUpController();
                ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "ShieldPowerUp");
                ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "SpeedPowerUp");
                ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "InvertedSteeringPowerUp");
                ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "GettingWastedXTimesMorePowerUp");
                ResetDurationForThePowerUp(flightControllerScript.gameModeScript.playerTwoPlane, "MultiShotPowerUp");
                flightControllerScript.uiManagerScript.ClearPowerUpBar(flightControllerScript.gameModeScript.playerTwoPlane);
            }
        }
        else
            GetComponent<PowerUpManager>().enabled = false;
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
        if (powerUpName == "ShieldPowerUp")
            ReturnPlayerPowerUpControllerObject(plane).shieldCounter = powerUpCounter;
        else if (powerUpName == "SpeedPowerUp")
            ReturnPlayerPowerUpControllerObject(plane).speedCounter = powerUpCounter;
        else if (powerUpName == "InvertedSteeringPowerUp")
            ReturnPlayerPowerUpControllerObject(plane).steeringCounter = powerUpCounter;
        else if (powerUpName == "GettingWastedXTimesMorePowerUp")
            ReturnPlayerPowerUpControllerObject(plane).gettingWastedXTimesMoreCounter = powerUpCounter;
        else if (powerUpName == "MultiShotPowerUp")
            ReturnPlayerPowerUpControllerObject(plane).multiShotCounter = powerUpCounter;
    }
    private void CheckPlayerShield(Plane plane)
    {
        if (plane.shieldEnabled)
        {
            ReturnPlayerPowerUpControllerObject(plane).shieldCounter -= Time.deltaTime;
            if (ReturnPlayerPowerUpControllerObject(plane).shieldCounter <= 0)
            {
                plane.planeRendererScript.HideShield();
                plane.shieldEnabled = false;
                ResetDurationForThePowerUp(plane, "ShieldPowerUp");
            }
        }
    }
    private void CheckPlayerSpeed(Plane plane)
    {
        if (plane.speedEnabled)
        {
            ReturnPlayerPowerUpControllerObject(plane).speedCounter -= Time.deltaTime;
            if (ReturnPlayerPowerUpControllerObject(plane).speedCounter <= 0)
            {
                plane.speedEnabled = false;
                plane.currentPlaneSpeed = flightControllerScript.gameplaySettings.defaultPlaneSpeed;
                ResetDurationForThePowerUp(plane, "SpeedPowerUp");
            }
        }
    }
    private void CheckInvertedSteering(Plane plane)
    {
        if (plane.invertedSteeringEnabled)
        {
            ReturnPlayerPowerUpControllerObject(plane).steeringCounter -= Time.deltaTime;
            if (ReturnPlayerPowerUpControllerObject(plane).steeringCounter <= 0)
            {
                plane.invertedSteeringEnabled = false;
                ResetDurationForThePowerUp(plane, "InvertedSteeringPowerUp");
            }
        }
    }
    private void CheckGettingWastedXTimesMore(Plane plane)
    {
        if (plane.gettingWastedXTimesMoreEnabled)
        {
            ReturnPlayerPowerUpControllerObject(plane).gettingWastedXTimesMoreCounter -= Time.deltaTime;
            if (ReturnPlayerPowerUpControllerObject(plane).gettingWastedXTimesMoreCounter <= 0)
            {
                plane.gettingWastedXTimesMoreEnabled = false;
                ResetDurationForThePowerUp(plane, "GettingWastedXTimesMorePowerUp");
            }
        }
    }
    private void CheckMultiShot(Plane plane)
    {
        if (plane.multiShotEnabled)
        {
            ReturnPlayerPowerUpControllerObject(plane).multiShotCounter -= Time.deltaTime;
            if (ReturnPlayerPowerUpControllerObject(plane).multiShotCounter <= 0)
            {
                plane.multiShotEnabled = false;
                ResetDurationForThePowerUp(plane, "MultiShotPowerUp");
            }
        }
    }
}
