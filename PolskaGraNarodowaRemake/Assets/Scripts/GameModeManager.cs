using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public enum GameMode
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
    internal FlightController flightController;
    public GameMode currentGameMode;
    internal PlayerState playerOneState;
    internal PlayerState playerTwoState;
    internal bool someoneWon;
    private float waitingTimeForOneLinerCurrent;
    
    private void OnEnable()
    {
        Application.targetFrameRate = 144;
        flightController = GetComponent<FlightController>();
        playerOnePlane.LoadPlaneData(0);
        if (currentGameMode != GameMode.singleplayerClassic && currentGameMode != GameMode.singleplayerEndless)
        {
            playerTwoPlane.LoadPlaneData(1);
            flightController.gameplaySettings.cameraPositionXOffset = flightController.gameplaySettings.cameraPositionXOffsetMulti;
        }
        else
            flightController.gameplaySettings.cameraPositionXOffset = flightController.gameplaySettings.cameraPositionXOffsetSingle;
        waitingTimeForOneLinerCurrent = 0;
        someoneWon = false;
    }
    private void CalculateWiningAndLosing(float gameOverTimer)
    {
        if(currentGameMode == GameMode.singleplayerClassic || currentGameMode == GameMode.singleplayerEndless)
        {
            if (playerOneState == PlayerState.crashed)
            {
                flightController.uiManagerScript.TurnOnColorPanel(flightController.uiManagerScript.colorPanelPlayerOneGameObject, flightController.uiManagerScript.loseColor);
                flightController.uiManagerScript.TurnOnColorPanel(flightController.uiManagerScript.colorPanelPlayerTwoGameObject, flightController.uiManagerScript.loseColor);
                flightController.uiManagerScript.SetTheTextOnTheColorPanel(flightController.uiManagerScript.colorPanelPlayerOneGameObject, flightController.uiManagerScript.gameplaySettings.localizationsStrings[flightController.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerLosesSinglePlayer);
                flightController.audioManagerScript.StopPlayingSound("EngineSound", flightController.audioManagerScript.SFX);
                flightController.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightController.audioManagerScript.oneLinersSounds);
                flightController.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightController.audioManagerScript.hitReactionSounds);
                flightController.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightController.audioManagerScript.landingSounds);
                flightController.uiManagerScript.regularHUDMainGameObject.SetActive(false);
                flightController.uiManagerScript.TurnOnTheTimer(gameOverTimer);
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
                flightController.uiManagerScript.TurnOnColorPanel(flightController.uiManagerScript.colorPanelPlayerOneGameObject, flightController.uiManagerScript.winColor);
                flightController.uiManagerScript.SetTheTextOnTheColorPanel(flightController.uiManagerScript.colorPanelPlayerOneGameObject, flightController.uiManagerScript.gameplaySettings.localizationsStrings[flightController.uiManagerScript.gameplaySettings.langauageIndex].playerOneIndicator + " " + flightController.uiManagerScript.gameplaySettings.localizationsStrings[flightController.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerWins);
                flightController.uiManagerScript.TurnOnColorPanel(flightController.uiManagerScript.colorPanelPlayerTwoGameObject, flightController.uiManagerScript.loseColor);
                flightController.uiManagerScript.SetTheTextOnTheColorPanel(flightController.uiManagerScript.colorPanelPlayerTwoGameObject, flightController.uiManagerScript.gameplaySettings.localizationsStrings[flightController.uiManagerScript.gameplaySettings.langauageIndex].playerTwoIndicator + " " + flightController.uiManagerScript.gameplaySettings.localizationsStrings[flightController.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerLoses);
                flightController.audioManagerScript.StopPlayingSound("EngineSound", flightController.audioManagerScript.SFX);
                flightController.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightController.audioManagerScript.oneLinersSounds);
                flightController.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightController.audioManagerScript.hitReactionSounds);
                flightController.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightController.audioManagerScript.landingSounds);
                flightController.uiManagerScript.regularHUDMainGameObject.SetActive(false);
                flightController.uiManagerScript.TurnOnTheTimer(gameOverTimer);
            }
            else if (playerTwoState != PlayerState.crashed && playerOneState == PlayerState.crashed)
            {
                playerTwoPlane.currentPlaneSpeed = 0;
                playerTwoPlane.verticalMovementKeys = 0;
                playerTwoPlane.difficultyImpulseEnabled = false;
                playerOnePlane.godMode = false;
                flightController.uiManagerScript.TurnOnColorPanel(flightController.uiManagerScript.colorPanelPlayerTwoGameObject, flightController.uiManagerScript.winColor);
                flightController.uiManagerScript.SetTheTextOnTheColorPanel(flightController.uiManagerScript.colorPanelPlayerTwoGameObject, flightController.uiManagerScript.gameplaySettings.localizationsStrings[flightController.uiManagerScript.gameplaySettings.langauageIndex].playerTwoIndicator + " " + flightController.uiManagerScript.gameplaySettings.localizationsStrings[flightController.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerWins);
                flightController.uiManagerScript.TurnOnColorPanel(flightController.uiManagerScript.colorPanelPlayerOneGameObject, flightController.uiManagerScript.loseColor);
                flightController.uiManagerScript.SetTheTextOnTheColorPanel(flightController.uiManagerScript.colorPanelPlayerOneGameObject, flightController.uiManagerScript.gameplaySettings.localizationsStrings[flightController.uiManagerScript.gameplaySettings.langauageIndex].playerOneIndicator + " " + flightController.uiManagerScript.gameplaySettings.localizationsStrings[flightController.uiManagerScript.gameplaySettings.langauageIndex].colorPanelPlayerLoses);
                flightController.audioManagerScript.StopPlayingSound("EngineSound", flightController.audioManagerScript.SFX);
                flightController.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightController.audioManagerScript.oneLinersSounds);
                flightController.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightController.audioManagerScript.hitReactionSounds);
                flightController.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightController.audioManagerScript.landingSounds);
                flightController.uiManagerScript.regularHUDMainGameObject.SetActive(false);
                flightController.uiManagerScript.TurnOnTheTimer(gameOverTimer);
            }
        }
    }
    private void CheckAndPlayOneLinerSound()
    {
        if (((currentGameMode == GameMode.singleplayerClassic || currentGameMode == GameMode.singleplayerEndless) && playerOneState == PlayerState.flying) || ((currentGameMode == GameMode.versusClassic || currentGameMode == GameMode.versusEndless) && playerOneState == PlayerState.flying && playerTwoState == PlayerState.flying))
        {
            waitingTimeForOneLinerCurrent += Time.deltaTime;
            if (waitingTimeForOneLinerCurrent >= flightController.gameplaySettings.waitingTimeForOneLiner)
            {
                flightController.audioManagerScript.DrawAndPlayASound(flightController.audioManagerScript.oneLinersSounds, "OneLiner", ref flightController.audioManagerScript.lastPlayedOneLiner);
                waitingTimeForOneLinerCurrent -= (flightController.audioManagerScript.ReturnSoundDuration("OneLiner" + flightController.audioManagerScript.lastPlayedOneLiner, flightController.audioManagerScript.oneLinersSounds) + flightController.gameplaySettings.waitingTimeForOneLiner);
            }
        }
    }
    private void Update()
    {
        SetProgressionFlags(playerOnePlane);
        if (currentGameMode != GameMode.singleplayerClassic && currentGameMode != GameMode.singleplayerEndless)
            SetProgressionFlags(playerTwoPlane);
        CheckAndPlayOneLinerSound();
        if(!flightController.rewardAndProgressionManagerScript.toNewLevel && (((currentGameMode == GameMode.singleplayerClassic || currentGameMode == GameMode.singleplayerEndless) && playerOneState == PlayerState.landed) || ((currentGameMode == GameMode.versusClassic || currentGameMode == GameMode.versusEndless) && playerOneState == PlayerState.landed && playerTwoState == PlayerState.landed)))
        {
            flightController.rewardAndProgressionManagerScript.toNewLevel = true;
            playerOneState = PlayerState.flying;
            if (currentGameMode != GameMode.singleplayerClassic && currentGameMode != GameMode.singleplayerEndless)
                playerTwoState = PlayerState.flying;
            flightController.audioManagerScript.StopPlayingSoundsFromTheSpecificSoundBank(flightController.audioManagerScript.oneLinersSounds);
            flightController.audioManagerScript.DrawAndPlayASound(flightController.audioManagerScript.landingSounds, "Landing", ref flightController.audioManagerScript.lastPlayedLandingSound);
            if (flightController.audioManagerScript.ReturnSoundDuration("Landing" + flightController.audioManagerScript.lastPlayedLandingSound, flightController.audioManagerScript.landingSounds) > 5f)
                flightController.uiManagerScript.TurnOnTheTimer(flightController.audioManagerScript.ReturnSoundDuration("Landing" + flightController.audioManagerScript.lastPlayedLandingSound, flightController.audioManagerScript.landingSounds));
            else
                flightController.uiManagerScript.TurnOnTheTimer(5f);
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
}
