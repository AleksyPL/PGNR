using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    internal enum GameMode
    {
        singleplayerClassic,
        singleplayerEndless,
        versusClassic,
        versusEndless,
        tutorial
    }
    internal enum PlayerState
    {
        flying,
        landing,
        landed,
        damaged,
        crashed
    }
    [SerializeField] internal Plane playerOnePlane;
    [SerializeField] internal Plane playerTwoPlane;
    internal FlightController flightControllerScript;
    internal bool gameModeSettingsOverride;
    [SerializeField] internal GameMode currentGameMode;
    internal GameModeManager.PlayerState playerOneState;
    internal GameModeManager.PlayerState playerTwoState;
    internal bool someoneWon;
    internal float waitingTimeForOneLinerCurrent;
    void OnEnable()
    {
        Application.targetFrameRate = 144;
        gameModeSettingsOverride = true;
        flightControllerScript = GetComponent<FlightController>();
        playerOnePlane.LoadPlaneData(0);
        SetUpGameMode();
        if (currentGameMode == GameMode.versusClassic || currentGameMode == GameMode.versusEndless)
            playerTwoPlane.LoadPlaneData(1);
        CalculateCameraOffset();
        waitingTimeForOneLinerCurrent = 0;
        someoneWon = false;
    }
    private void SetUpGameMode()
    {
        if (MainMenuManager.fromMainMenu)
            gameModeSettingsOverride = false;
        if (!gameModeSettingsOverride)
            if(MainMenuManager.currentGameMode == global::GameMode.SinglePlayerClassic)
                currentGameMode = GameMode.singleplayerClassic;
            else if (MainMenuManager.currentGameMode == global::GameMode.SinglePlayerEndless)
                currentGameMode = GameMode.singleplayerEndless;
            if (MainMenuManager.currentGameMode == global::GameMode.VersusClassic)
                currentGameMode = GameMode.versusClassic;
            else if (MainMenuManager.currentGameMode == global::GameMode.VersusEndless)
                currentGameMode = GameMode.versusEndless;
    }
    private void CalculateWiningAndLosing(float gameOverTimer)
    {
        if(currentGameMode == GameMode.singleplayerClassic || currentGameMode == GameMode.singleplayerEndless)
        {
            if (playerOneState == GameModeManager.PlayerState.crashed)
            {
                flightControllerScript.uiManagerScript.TurnOnColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject, flightControllerScript.uiManagerScript.loseColor);
                flightControllerScript.uiManagerScript.TurnOnColorPanel(flightControllerScript.uiManagerScript.playerTwoUI.colorPanelGameObject, flightControllerScript.uiManagerScript.loseColor);
                flightControllerScript.uiManagerScript.SetTheTextOnTheColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject, flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerLosesSinglePlayer);
                flightControllerScript.audioManagerScript.StopPlayingSound("EngineSound", flightControllerScript.audioManagerScript.localSFX);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localOneLinersSounds);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localHitReactionSounds);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localLandingSounds);
                flightControllerScript.uiManagerScript.playerOneUI.regularHUDMainGameObject.SetActive(false);
                if (flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject != null)
                    flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject.SetActive(false);
                flightControllerScript.uiManagerScript.SpawnTimerOnTheScreen(gameOverTimer);
            } 
        }
        else if (currentGameMode == GameMode.versusClassic || currentGameMode == GameMode.versusEndless)
        {
            if (playerOneState != GameModeManager.PlayerState.crashed && playerTwoState == GameModeManager.PlayerState.crashed)
            {
                playerOnePlane.difficultyImpulseEnabled = false;
                playerOnePlane.godMode = true;
                flightControllerScript.uiManagerScript.TurnOnColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject, flightControllerScript.uiManagerScript.winColor);
                flightControllerScript.uiManagerScript.SetTheTextOnTheColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject, flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].playerOneIndicator + " " + flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerWins);
                flightControllerScript.uiManagerScript.TurnOnColorPanel(flightControllerScript.uiManagerScript.playerTwoUI.colorPanelGameObject, flightControllerScript.uiManagerScript.loseColor);
                flightControllerScript.uiManagerScript.SetTheTextOnTheColorPanel(flightControllerScript.uiManagerScript.playerTwoUI.colorPanelGameObject, flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].playerTwoIndicator + " " + flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerLoses);
                flightControllerScript.audioManagerScript.StopPlayingSound("EngineSound", flightControllerScript.audioManagerScript.localSFX);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localOneLinersSounds);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localHitReactionSounds);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localLandingSounds);
                flightControllerScript.uiManagerScript.playerOneUI.regularHUDMainGameObject.SetActive(false);
                if (flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject != null)
                    flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject.SetActive(false);
                flightControllerScript.uiManagerScript.SpawnTimerOnTheScreen(gameOverTimer);
            }
            else if (playerOneState == GameModeManager.PlayerState.crashed && playerTwoState != GameModeManager.PlayerState.crashed)
            {
                playerTwoPlane.difficultyImpulseEnabled = false;
                playerTwoPlane.godMode = true;
                flightControllerScript.uiManagerScript.TurnOnColorPanel(flightControllerScript.uiManagerScript.playerTwoUI.colorPanelGameObject, flightControllerScript.uiManagerScript.winColor);
                flightControllerScript.uiManagerScript.SetTheTextOnTheColorPanel(flightControllerScript.uiManagerScript.playerTwoUI.colorPanelGameObject, flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].playerTwoIndicator + " " + flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerWins);
                flightControllerScript.uiManagerScript.TurnOnColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject, flightControllerScript.uiManagerScript.loseColor);
                flightControllerScript.uiManagerScript.SetTheTextOnTheColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject, flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].playerOneIndicator + " " + flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerLoses);
                flightControllerScript.audioManagerScript.StopPlayingSound("EngineSound", flightControllerScript.audioManagerScript.localSFX);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localOneLinersSounds);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localHitReactionSounds);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localLandingSounds);
                flightControllerScript.uiManagerScript.playerOneUI.regularHUDMainGameObject.SetActive(false);
                if (flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject != null)
                    flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject.SetActive(false);
                flightControllerScript.uiManagerScript.SpawnTimerOnTheScreen(gameOverTimer);
            }
        }
    }
    private void CheckAndPlayOneLinerSound()
    {
        if (((currentGameMode == GameMode.singleplayerClassic || currentGameMode == GameMode.singleplayerEndless) && playerOneState == GameModeManager.PlayerState.flying) || ((currentGameMode == GameMode.versusClassic || currentGameMode == GameMode.versusEndless) && playerOneState == GameModeManager.PlayerState.flying && playerTwoState == GameModeManager.PlayerState.flying))
        {
            waitingTimeForOneLinerCurrent += Time.deltaTime;
            if (waitingTimeForOneLinerCurrent >= flightControllerScript.gameplaySettings.waitingTimeForOneLiner)
            {
                flightControllerScript.audioManagerScript.DrawAndPlayASound(flightControllerScript.audioManagerScript.localOneLinersSounds, "OneLiner", ref flightControllerScript.audioManagerScript.lastPlayedOneLiner);
                waitingTimeForOneLinerCurrent -= (flightControllerScript.audioManagerScript.ReturnSoundDuration("OneLiner" + flightControllerScript.audioManagerScript.lastPlayedOneLiner, flightControllerScript.audioManagerScript.localOneLinersSounds) + flightControllerScript.gameplaySettings.waitingTimeForOneLiner);
            }
        }
    }
    void Update()
    {
        SetProgressionFlags(playerOnePlane);
        if (currentGameMode != GameMode.singleplayerClassic && currentGameMode != GameMode.singleplayerEndless)
            SetProgressionFlags(playerTwoPlane);
        if(currentGameMode != GameMode.tutorial)
            CheckAndPlayOneLinerSound();
        if(!flightControllerScript.rewardAndProgressionManagerScript.toNewLevel && (((currentGameMode == GameMode.singleplayerClassic || currentGameMode == GameMode.singleplayerEndless) && playerOneState == PlayerState.landed) || ((currentGameMode == GameMode.versusClassic || currentGameMode == GameMode.versusEndless) && playerOneState == GameModeManager.PlayerState.landed && playerTwoState == GameModeManager.PlayerState.landed)))
        {
            flightControllerScript.rewardAndProgressionManagerScript.toNewLevel = true;
            playerOneState = GameModeManager.PlayerState.flying;
            if (currentGameMode != GameMode.singleplayerClassic && currentGameMode != GameMode.singleplayerEndless)
                playerTwoState = GameModeManager.PlayerState.flying;
            flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localOneLinersSounds);
            flightControllerScript.audioManagerScript.DrawAndPlayASound(flightControllerScript.audioManagerScript.localLandingSounds, "Landing", ref flightControllerScript.audioManagerScript.lastPlayedLandingSound);
            if (flightControllerScript.audioManagerScript.ReturnSoundDuration("Landing" + flightControllerScript.audioManagerScript.lastPlayedLandingSound, flightControllerScript.audioManagerScript.localLandingSounds) > 5f)
                flightControllerScript.uiManagerScript.SpawnTimerOnTheScreen(flightControllerScript.audioManagerScript.ReturnSoundDuration("Landing" + flightControllerScript.audioManagerScript.lastPlayedLandingSound, flightControllerScript.audioManagerScript.localLandingSounds));
            else
                flightControllerScript.uiManagerScript.SpawnTimerOnTheScreen(5f);
        }
    }
    private void SetProgressionFlags(Plane plane)
    {
        if(!plane.isTouchingAirport && (plane.currentPlaneState == PlaneState.standard || plane.currentPlaneState == PlaneState.wheelsOn))
            ReturnPlayerStateObject(plane) = GameModeManager.PlayerState.flying;
        if(plane.isTouchingAirport && plane.currentPlaneState == PlaneState.wheelsOn && plane.currentPlaneSpeed > 0)
            ReturnPlayerStateObject(plane) = GameModeManager.PlayerState.landing;
        if(plane.isTouchingAirport && plane.currentPlaneState == PlaneState.wheelsOn && plane.currentPlaneSpeed <= 0)
            ReturnPlayerStateObject(plane) = GameModeManager.PlayerState.landed;
        if (plane.currentPlaneState == PlaneState.damaged)
            ReturnPlayerStateObject(plane) = GameModeManager.PlayerState.damaged;
        if (!someoneWon && plane.currentPlaneState == PlaneState.crashed)
        {
            ReturnPlayerStateObject(plane) = GameModeManager.PlayerState.crashed;
            someoneWon = true;
            CalculateWiningAndLosing(3f);
        }
    }
    internal ref PlayerState ReturnPlayerStateObject(Plane plane)
    {
        if (plane == playerOnePlane)
            return ref playerOneState;
        else
            return ref playerTwoState;
    }
    internal ref Plane ReturnAPlaneObject(GameObject plane)
    {
        if (plane == playerOnePlane.planeGameObject)
            return ref playerOnePlane;
        else
            return ref playerTwoPlane;
    }
    private void CalculateCameraOffset()
    {
        float offsetX = playerOnePlane.cameraGameObject.GetComponent<Camera>().aspect * 2 * playerOnePlane.cameraGameObject.GetComponent<Camera>().orthographicSize;
        if (!UnityEngine.Device.Application.isMobilePlatform && (currentGameMode == GameMode.singleplayerClassic || currentGameMode == GameMode.singleplayerEndless || currentGameMode == GameMode.tutorial))
            flightControllerScript.gameplaySettings.cameraPositionXOffset = offsetX * flightControllerScript.gameplaySettings.camerePositionXOffsetPersentageSingle;
        else if (UnityEngine.Device.Application.isMobilePlatform && (currentGameMode == GameMode.singleplayerClassic || currentGameMode == GameMode.singleplayerEndless || currentGameMode == GameMode.tutorial))
            flightControllerScript.gameplaySettings.cameraPositionXOffset = offsetX * flightControllerScript.gameplaySettings.cameraPositionXOffsetPersentageSingleMobile;
        else if (currentGameMode == GameMode.versusClassic || currentGameMode == GameMode.versusEndless)
            flightControllerScript.gameplaySettings.cameraPositionXOffset = offsetX * flightControllerScript.gameplaySettings.camerePositionXOffsetPersentageMulti;
    }
}
