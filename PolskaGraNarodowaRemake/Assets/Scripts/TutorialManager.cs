using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    internal FlightController flightControllerScript;
    internal enum TutorialPlayerState
    {
        Flying,
        Frozen,
        Reverting
    }
    internal TutorialPlayerState currentState;
    //Tutorial info screens
    public GameObject[] tutorialScreens;
    //Revert Effect
    private List<Vector3> playerPositions;
    private List<PlaneState> playerState;
    private List<float> playerTilt;
    internal float elapsedTime;
    internal float scoreAtTheBeginningOfTheCheckpoint;
    internal float scoreJustBeforeTheRewind;
    //[Range(0.1f, 0.99f)]
    //public float positionRevertMinDuration;
    //[Range(1, 5)]
    //public float positionRevertMaxDuration;
    //[Range(0.01f,1)]
    //public float positionRevertSpeed;
    private float positonRevertTime;
    private float positionRevertLerpValue;
    //Checkpoint facts
    internal int checkpointNumber;
    internal bool checkpointFinished;
    internal bool checkpointGoalAchieved;
    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        playerPositions = new List<Vector3>();
        playerState = new List<PlaneState>();
        playerTilt = new List<float>();
        currentState = TutorialPlayerState.Frozen;
        flightControllerScript.uiManagerScript.EnableTutorialScreen();
        checkpointNumber = 0;
        positionRevertLerpValue = 0;
        checkpointFinished = false;
        checkpointGoalAchieved = false;
        scoreAtTheBeginningOfTheCheckpoint = flightControllerScript.gameModeScript.playerOnePlane.gameScore;
        scoreJustBeforeTheRewind = scoreAtTheBeginningOfTheCheckpoint;
    }
    void Update()
    {
        if (currentState == TutorialPlayerState.Flying)
            SavePlayerRewindData();
        else if (currentState == TutorialPlayerState.Reverting)
        {
            positionRevertLerpValue += Time.deltaTime / positonRevertTime;
            if (positionRevertLerpValue < 1)
            {
                CalculateNewPlanePosition();
                CalculateNewPlayerScore();
            }
            else
                ClearRewindData();
        }
    }
    private void SavePlayerRewindData()
    {
        playerPositions.Add(flightControllerScript.gameModeScript.playerOnePlane.planeGameObject.transform.position);
        playerState.Add(flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState);
        playerTilt.Add(flightControllerScript.gameModeScript.playerOnePlane.verticalMovementKeys);
        elapsedTime += Time.deltaTime;
    }
    internal void CalculateRevertDuration()
    {
        //positonRevertTime = elapsedTime / positionRevertMaxDuration / positionRevertSpeed;
        //positonRevertTime = Mathf.Clamp(positonRevertTime, positionRevertMinDuration, positionRevertMaxDuration);
        if (elapsedTime > 1 && elapsedTime < 2)
            positonRevertTime = 1;
        else if (elapsedTime >= 2)
            positonRevertTime = flightControllerScript.audioManagerScript.ReturnSoundDuration("Rewind", flightControllerScript.audioManagerScript.localSFX);
    }
    private void CalculateNewPlanePosition()
    {
        int listIndex = (int)Mathf.Lerp(playerPositions.Count - 1, 0, positionRevertLerpValue);
        flightControllerScript.RevertPlanePosition(flightControllerScript.gameModeScript.playerOnePlane, playerPositions[listIndex], playerState[listIndex], playerTilt[listIndex]);
    }
    private void CalculateNewPlayerScore()
    {
        float newScore = Mathf.Lerp(scoreAtTheBeginningOfTheCheckpoint, scoreJustBeforeTheRewind, 1 - positionRevertLerpValue);
        flightControllerScript.gameModeScript.playerOnePlane.gameScore = newScore;
        flightControllerScript.uiManagerScript.UpdateScoreCounter(flightControllerScript.gameModeScript.playerOnePlane);
    }
    private void ClearRewindData()
    {
        playerPositions.Clear();
        playerState.Clear();
        playerTilt.Clear();
        currentState = TutorialPlayerState.Frozen;
        elapsedTime = 0;
        positonRevertTime = 0;
        positionRevertLerpValue = 0;
        checkpointFinished = false;
        scoreJustBeforeTheRewind = scoreAtTheBeginningOfTheCheckpoint;
        flightControllerScript.audioManagerScript.ResumeAllPausedSounds();
        flightControllerScript.uiManagerScript.EnableTutorialScreen();
    }
    internal void SpawnTutorialInfo(int arrayIndex)
    {
        if (tutorialScreens.Length >= arrayIndex)
        {
            GameObject newTutorialScreen = Instantiate(tutorialScreens[arrayIndex], flightControllerScript.uiManagerScript.tutorialMainGameObject.transform);
            newTutorialScreen.transform.name = "TutorialScreen";
        }
    }
}
