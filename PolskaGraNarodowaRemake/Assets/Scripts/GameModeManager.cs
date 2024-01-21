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
        versusEndless
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
    public bool gameModeSettingsOverride;
    [SerializeField] internal GameMode currentGameMode;
    internal PlayerState playerOneState;
    internal PlayerState playerTwoState;
    internal bool someoneWon;
    private float waitingTimeForOneLinerCurrent;
    
    private void OnEnable()
    {
        Application.targetFrameRate = 144;
        flightControllerScript = GetComponent<FlightController>();
        playerOnePlane.LoadPlaneData(0);
        SetUpGameMode();
        if (currentGameMode != GameMode.singleplayerClassic && currentGameMode != GameMode.singleplayerEndless)
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
            if (playerOneState == PlayerState.crashed)
            {
                flightControllerScript.uiManagerScript.TurnOnColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject, flightControllerScript.uiManagerScript.loseColor);
                flightControllerScript.uiManagerScript.TurnOnColorPanel(flightControllerScript.uiManagerScript.playerTwoUI.colorPanelGameObject, flightControllerScript.uiManagerScript.loseColor);
                flightControllerScript.uiManagerScript.SetTheTextOnTheColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject, flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerLosesSinglePlayer);
                flightControllerScript.audioManagerScript.StopPlayingSound("EngineSound", flightControllerScript.audioManagerScript.localSFX);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localOneLinersSounds);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localHitReactionSounds);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localLandingSounds);
                flightControllerScript.uiManagerScript.playerOneUI.regularHUDMainGameObject.SetActive(false);
                if (flightControllerScript.uiManagerScript.playerTwoUI != null)
                    flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject.SetActive(false);
                flightControllerScript.uiManagerScript.SpawnTimerOnTheScreen(gameOverTimer);
            } 
        }
        else
        {
            if (playerOneState != PlayerState.crashed && playerTwoState == PlayerState.crashed)
            {
                playerOnePlane.currentPlaneSpeed = 0;
                playerOnePlane.verticalMovementKeys = 0;
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
                if (flightControllerScript.uiManagerScript.playerTwoUI != null)
                    flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject.SetActive(false);
                flightControllerScript.uiManagerScript.SpawnTimerOnTheScreen(gameOverTimer);
            }
            else if (playerTwoState != PlayerState.crashed && playerOneState == PlayerState.crashed)
            {
                playerTwoPlane.currentPlaneSpeed = 0;
                playerTwoPlane.verticalMovementKeys = 0;
                playerTwoPlane.difficultyImpulseEnabled = false;
                playerOnePlane.godMode = false;
                flightControllerScript.uiManagerScript.TurnOnColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject, flightControllerScript.uiManagerScript.winColor);
                flightControllerScript.uiManagerScript.SetTheTextOnTheColorPanel(flightControllerScript.uiManagerScript.playerTwoUI.colorPanelGameObject, flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].playerTwoIndicator + " " + flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerWins);
                flightControllerScript.uiManagerScript.TurnOnColorPanel(flightControllerScript.uiManagerScript.playerOneUI.colorPanelGameObject, flightControllerScript.uiManagerScript.loseColor);
                flightControllerScript.uiManagerScript.SetTheTextOnTheColorPanel(flightControllerScript.uiManagerScript.playerTwoUI.colorPanelGameObject, flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].playerOneIndicator + " " + flightControllerScript.uiManagerScript.gameplaySettings.localizationsStrings[flightControllerScript.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerLoses);
                flightControllerScript.audioManagerScript.StopPlayingSound("EngineSound", flightControllerScript.audioManagerScript.localSFX);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localOneLinersSounds);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localHitReactionSounds);
                flightControllerScript.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightControllerScript.audioManagerScript.localLandingSounds);
                flightControllerScript.uiManagerScript.playerOneUI.regularHUDMainGameObject.SetActive(false);
                if (flightControllerScript.uiManagerScript.playerTwoUI != null)
                    flightControllerScript.uiManagerScript.playerTwoUI.regularHUDMainGameObject.SetActive(false);
                flightControllerScript.uiManagerScript.SpawnTimerOnTheScreen(gameOverTimer);
            }
        }
    }
    private void CheckAndPlayOneLinerSound()
    {
        if (((currentGameMode == GameMode.singleplayerClassic || currentGameMode == GameMode.singleplayerEndless) && playerOneState == PlayerState.flying) || ((currentGameMode == GameMode.versusClassic || currentGameMode == GameMode.versusEndless) && playerOneState == PlayerState.flying && playerTwoState == PlayerState.flying))
        {
            waitingTimeForOneLinerCurrent += Time.deltaTime;
            if (waitingTimeForOneLinerCurrent >= flightControllerScript.gameplaySettings.waitingTimeForOneLiner)
            {
                flightControllerScript.audioManagerScript.DrawAndPlayASound(flightControllerScript.audioManagerScript.localOneLinersSounds, "OneLiner", ref flightControllerScript.audioManagerScript.lastPlayedOneLiner);
                waitingTimeForOneLinerCurrent -= (flightControllerScript.audioManagerScript.ReturnSoundDuration("OneLiner" + flightControllerScript.audioManagerScript.lastPlayedOneLiner, flightControllerScript.audioManagerScript.localOneLinersSounds) + flightControllerScript.gameplaySettings.waitingTimeForOneLiner);
            }
        }
    }
    private void Update()
    {
        SetProgressionFlags(playerOnePlane);
        if (currentGameMode != GameMode.singleplayerClassic && currentGameMode != GameMode.singleplayerEndless)
            SetProgressionFlags(playerTwoPlane);
        CheckAndPlayOneLinerSound();
        if(!flightControllerScript.rewardAndProgressionManagerScript.toNewLevel && (((currentGameMode == GameMode.singleplayerClassic || currentGameMode == GameMode.singleplayerEndless) && playerOneState == PlayerState.landed) || ((currentGameMode == GameMode.versusClassic || currentGameMode == GameMode.versusEndless) && playerOneState == PlayerState.landed && playerTwoState == PlayerState.landed)))
        {
            flightControllerScript.rewardAndProgressionManagerScript.toNewLevel = true;
            playerOneState = PlayerState.flying;
            if (currentGameMode != GameMode.singleplayerClassic && currentGameMode != GameMode.singleplayerEndless)
                playerTwoState = PlayerState.flying;
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
            ReturnPlayerStateObject(plane) = PlayerState.flying;
        if(plane.isTouchingAirport && plane.currentPlaneState == PlaneState.wheelsOn && plane.currentPlaneSpeed > 0)
            ReturnPlayerStateObject(plane) = PlayerState.landing;
        if(plane.isTouchingAirport && plane.currentPlaneState == PlaneState.wheelsOn && plane.currentPlaneSpeed <= 0)
            ReturnPlayerStateObject(plane) = PlayerState.landed;
        if (plane.currentPlaneState == PlaneState.damaged)
            ReturnPlayerStateObject(plane) = PlayerState.damaged;
        if (!someoneWon && plane.currentPlaneState == PlaneState.crashed)
        {
            ReturnPlayerStateObject(plane) = PlayerState.crashed;
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
        if (currentGameMode == GameMode.singleplayerClassic || currentGameMode == GameMode.singleplayerEndless)
            flightControllerScript.gameplaySettings.cameraPositionXOffset = offsetX * flightControllerScript.gameplaySettings.camerePositionXOffsetPersentageSingle;
        else if (currentGameMode == GameMode.versusClassic || currentGameMode == GameMode.versusEndless)
            flightControllerScript.gameplaySettings.cameraPositionXOffset = offsetX * flightControllerScript.gameplaySettings.camerePositionXOffsetPersentageMulti;
    }
}
