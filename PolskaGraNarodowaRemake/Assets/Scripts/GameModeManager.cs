using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public enum GameMode
    {
        singleplayer,
        versus
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

    private void OnEnable()
    {
        Application.targetFrameRate = 144;
    }
    private void Start()
    {
        playerOnePlane.LoadPlaneData(0);
        playerTwoPlane.LoadPlaneData(1);
        flightController = GetComponent<FlightController>();
    }
    private void Update()
    {
        SetProgressionFlags(playerOnePlane);
        if (currentGameMode != GameMode.singleplayer)
            SetProgressionFlags(playerTwoPlane);
        if(playerOneState != PlayerState.crashed && playerTwoState == PlayerState.crashed)
        {
            flightController.uiManagerScript.TurnOnColorPanel(flightController.uiManagerScript.regularHUDColorPanelPlayerOneGameObject, flightController.uiManagerScript.winColor);
            flightController.uiManagerScript.TurnOnColorPanel(flightController.uiManagerScript.regularHUDColorPanelPlayerTwoGameObject, flightController.uiManagerScript.loseColor);
        }        
        else if(playerTwoState != PlayerState.crashed && playerOneState == PlayerState.crashed)
        {
            flightController.uiManagerScript.TurnOnColorPanel(flightController.uiManagerScript.regularHUDColorPanelPlayerTwoGameObject, flightController.uiManagerScript.winColor);
            flightController.uiManagerScript.TurnOnColorPanel(flightController.uiManagerScript.regularHUDColorPanelPlayerOneGameObject, flightController.uiManagerScript.loseColor);
        }

        //if(currentGameMode == GameMode.versus)
        //{
        //    if(playerOnePlane.currentPlaneState == PlaneState.crashed && playerTwoPlane.currentPlaneState == PlaneState.crashed)
        //    {
        //        currentPlaythrough = Playthrough.finished;
        //    }
        //}
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
        if (plane.currentPlaneState == PlaneState.crashed)
            ReturnPlayerStateObject(plane) = PlayerState.crashed;
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
    internal void AudioFunction()
    {
        //if (optionsMenuGameObject.activeSelf)
        //    UpdateAllSoundsVolume();
        //if (planeControlCenterGameObject != null && PlaneScriptScript != null && (PlaneScriptScript.currentPlaneState == PlaneScript.StateMachine.standard || PlaneScriptScript.currentPlaneState == PlaneScript.StateMachine.wheelsOn))
        //{
        //    if(!PlaneScriptScript.flightControllScript.isTouchingAirport)
        //    {
        //        waitingTimeForOneLinerCurrent += Time.deltaTime;
        //        if (waitingTimeForOneLinerCurrent >= gameplaySettings.waitingTimeForOneLiner)
        //        {
        //            canPlayOneLiner = true;
        //            waitingTimeForOneLinerCurrent = 0;
        //            for (int i = 0; i < oneLinersSounds.Length; i++)
        //            {
        //                if (IsTheSoundCurrentlyPlaying("OneLiner" + i, oneLinersSounds))
        //                {
        //                    canPlayOneLiner = false;
        //                    break;
        //                }
        //            }
        //            if (canPlayOneLiner)
        //            {
        //                int randomSoundEffect = Random.Range(0, oneLinersSounds.Length);
        //                while (randomSoundEffect == lastPlayedOneLiner)
        //                    randomSoundEffect = Random.Range(0, oneLinersSounds.Length);
        //                lastPlayedOneLiner = randomSoundEffect;
        //                PlaySound("OneLiner" + randomSoundEffect.ToString(), oneLinersSounds);
        //                waitingTimeForOneLinerCurrent -= ReturnSoundDuration("OneLiner" + randomSoundEffect, oneLinersSounds);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if(!tiresSFXPlayed)
        //        {
        //            tiresSFXPlayed = true;
        //            StopPlayingSoundsFromTheSpecificSoundBank(oneLinersSounds);
        //            StopPlayingSound("EngineSound", SFX);
        //            PlaySound("Tires", SFX);
        //        }
        //        if(PlaneScriptScript.flightControllScript.currentPlaneSpeed == 0 && !landingSpeechPlayed)
        //        {
        //            landingSpeechPlayed = true;
        //            StopPlayingSound("Tires", SFX);
        //            StopPlayingSoundsFromTheSpecificSoundBank(oneLinersSounds);
        //            int randomSoundEffect = Random.Range(0, landingSounds.Length);
        //            while (randomSoundEffect == lastPlayedLandingSound)
        //                randomSoundEffect = Random.Range(0, landingSounds.Length);
        //            lastPlayedLandingSound = randomSoundEffect;
        //            PlaySound("Landing" + randomSoundEffect.ToString(), landingSounds);
        //            PlaneScriptScript.flightControllScript.waitingTimeAfterLandingCombinedWithSoundLength = ReturnSoundDuration("Landing" + randomSoundEffect.ToString(), landingSounds);
        //        }
        //    }
        //}
    }
}
