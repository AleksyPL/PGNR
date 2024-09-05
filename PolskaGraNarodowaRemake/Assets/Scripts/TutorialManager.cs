using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Serializable]
    public struct CheckpointScreens
    {
        internal int screenToShowIndex;
        public GameObject[] screensToShow;
    }
    internal FlightController flightControllerScript;
    internal enum TutorialPlayerState
    {
        Flying,
        Frozen,
        Reverting
    }
    internal TutorialPlayerState currentTutorialState;
    //Tutorial info screens
    public GameObject tutorialScreenTryAgain;
    public CheckpointScreens[] tutorialScreens;
    //Revert Effect
    private List<Vector3> playerPositions;
    private List<PlaneState> playerState;
    private List<float> playerTilt;
    internal float elapsedTime;
    internal float scoreAtTheBeginningOfTheCheckpoint;
    internal float scoreJustBeforeTheRewind;
    internal float bottlesAtTheBeginningOfTheCheckpoint;
    internal float bottlesJustBeforeTheRewind;
    internal PlaneState planeStateAtTheBeginningOfTheCheckpoint;
    internal PlaneState planeStateJustBeforeTheRewind;
    private float positonRevertTime;
    private float positionRevertLerpValue;
    //Checkpoint facts
    internal int checkpointNumber;
    internal bool checkpointFailedTryAgain;
    //internal bool checkpointFinished;
    internal bool checkpointGoalAchieved;
    [Header("Checkpoint 1")]
    public float checkpoint1LauncherPositionX;
    internal GameObject checkpoint1Launcher;
    [Header("Checkpoint 2")]
    public float checkpoint2Tree1PositionX;
    internal GameObject checkpoint2Tree1;
    public float checkpoint2Tree2PositionX;
    internal GameObject checkpoint2Tree2;
    [Header("Checkpoint 3")]
    public float checkpoint3LauncherPositionX;
    internal GameObject checkpoint3Launcher;
    public float checkpoint3TreePositionX;
    internal GameObject checkpoint3Tree;
    public float checkpoint3FogPositionX;
    internal GameObject checkpoint3Fog;
    [Header("Checkpoint 4")]
    public Vector2 checkpoint4PowerUpPosition;
    public int checkpoint4PowerUpNumber;
    internal GameObject checkpoint4PowerUp;
    [Header("Checkpoint 5")]
    public float checkpoint5AirportPositionX;
    internal GameObject checkpoint5Airport;

    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        playerPositions = new List<Vector3>();
        playerState = new List<PlaneState>();
        playerTilt = new List<float>();
        currentTutorialState = TutorialPlayerState.Frozen;
        flightControllerScript.uiManagerScript.EnableTutorialScreen();
        checkpointNumber = 0;
        positionRevertLerpValue = 0;
        checkpointFailedTryAgain = false;
        checkpointGoalAchieved = false;
        flightControllerScript.gameModeScript.playerOnePlane.canThrowBottles = false;
        scoreAtTheBeginningOfTheCheckpoint = flightControllerScript.gameModeScript.playerOnePlane.gameScore;
        scoreJustBeforeTheRewind = scoreAtTheBeginningOfTheCheckpoint;
        bottlesAtTheBeginningOfTheCheckpoint = flightControllerScript.gameModeScript.playerOnePlane.bottlesDrunk;
        bottlesJustBeforeTheRewind = bottlesAtTheBeginningOfTheCheckpoint;
        planeStateAtTheBeginningOfTheCheckpoint = flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState;
        planeStateJustBeforeTheRewind = planeStateAtTheBeginningOfTheCheckpoint;
        for (int i = 0; i < tutorialScreens.Length; i++)
            tutorialScreens[i].screenToShowIndex = 0;
    }
    void Update()
    {
        if (currentTutorialState == TutorialPlayerState.Flying)
            SavePlayerRewindData();
        else if (currentTutorialState == TutorialPlayerState.Reverting)
        {
            positionRevertLerpValue += Time.deltaTime / positonRevertTime;
            if (positionRevertLerpValue < 1)
            {
                CalculateNewPlanePosition();
                CalculateNewPlayerScore();
                CalculateNewPlayerBottlesDrunk();
            }
            else
                ClearRewindData(false);
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
        if (elapsedTime < 1)
            positonRevertTime = elapsedTime;
        else if (elapsedTime > 1 && elapsedTime < 2)
            positonRevertTime = 1;
        else if (elapsedTime >= 2)
            positonRevertTime = flightControllerScript.audioManagerScript.ReturnSoundDuration("Rewind", flightControllerScript.audioManagerScript.localSFX);
    }
    internal void PlayRewindSFX()
    {
        if (elapsedTime > 1 && elapsedTime < 2)
            flightControllerScript.audioManagerScript.PlaySound("Rewind_faster", flightControllerScript.audioManagerScript.localSFX);
        else if (elapsedTime >= 2)
            flightControllerScript.audioManagerScript.PlaySound("Rewind", flightControllerScript.audioManagerScript.localSFX);
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
    private void CalculateNewPlayerBottlesDrunk()
    {
        float newBottles = Mathf.Lerp(bottlesAtTheBeginningOfTheCheckpoint, bottlesJustBeforeTheRewind, 1 - positionRevertLerpValue);
        flightControllerScript.gameModeScript.playerOnePlane.bottlesDrunk = newBottles;
        flightControllerScript.uiManagerScript.UpdateBottlesCounter(flightControllerScript.gameModeScript.playerOnePlane);
    }
    internal void ClearRewindData(bool nextCheckpoint)
    {
        playerPositions.Clear();
        playerState.Clear();
        playerTilt.Clear();
        elapsedTime = 0;
        positonRevertTime = 0;
        positionRevertLerpValue = 0;
        scoreJustBeforeTheRewind = scoreAtTheBeginningOfTheCheckpoint;
        if (bottlesAtTheBeginningOfTheCheckpoint != 0)
            flightControllerScript.gameModeScript.playerOnePlane.difficultyImpulseEnabled = true;
        else
            flightControllerScript.gameModeScript.playerOnePlane.difficultyImpulseEnabled = false;
        flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState = planeStateAtTheBeginningOfTheCheckpoint;
        flightControllerScript.gameModeScript.playerOnePlane.planeRendererScript.ResetPlaneRenderer(flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState);
        flightControllerScript.gameModeScript.playerOnePlane.planeRendererScript.ChangePlaneSprite(flightControllerScript.gameModeScript.playerOnePlane.currentPlaneState);
        if (!nextCheckpoint)
        {
            checkpointFailedTryAgain = false;
            currentTutorialState = TutorialPlayerState.Frozen;
            flightControllerScript.audioManagerScript.ResumeAllPausedSounds();
            flightControllerScript.uiManagerScript.EnableTutorialScreen();
        }
    }
    internal void SpawnTutorialInfoScreen()
    {
        if (checkpointFailedTryAgain)
        {
            if (tutorialScreenTryAgain != null)
            {
                GameObject newTutorialScreen = Instantiate(tutorialScreenTryAgain, flightControllerScript.uiManagerScript.tutorialPlaceToSpawnScreens.transform);
                newTutorialScreen.transform.name = "TutorialScreen";
            }
        }
        else
        {
            if (tutorialScreens.Length >= checkpointNumber)
            {
                GameObject newTutorialScreen = Instantiate(tutorialScreens[checkpointNumber].screensToShow[tutorialScreens[checkpointNumber].screenToShowIndex], flightControllerScript.uiManagerScript.tutorialPlaceToSpawnScreens.transform);
                newTutorialScreen.transform.name = "TutorialScreen";
            }
        }
    }
    internal void SpawnTutorialObstacles()
    {
        if (checkpointNumber == 1)
        {
            if (checkpoint1Launcher == null)
                checkpoint1Launcher = flightControllerScript.levelManagerScript.SpawnSingleObstacle(flightControllerScript.gameModeScript.playerOnePlane, ref flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, checkpoint1LauncherPositionX, flightControllerScript.levelManagerScript.trotylLauncherPrefab, "trotylLauncher");
            else
                checkpoint1Launcher.GetComponent<TrotylLauncher>().canShoot = true;
            checkpoint1Launcher.GetComponent<TrotylLauncher>().rateOfFireCounter = 2;
        }
        else if (checkpointNumber == 2)
        {
            if (checkpoint2Tree1 == null)
                checkpoint2Tree1 = flightControllerScript.levelManagerScript.SpawnSingleObstacle(flightControllerScript.gameModeScript.playerOnePlane, ref flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, checkpoint2Tree1PositionX, flightControllerScript.environmentManagerScript.environmentsScenarios[flightControllerScript.environmentManagerScript.scenarioIndex].verticalObstaclesPrefabs[1]);
            if (checkpoint2Tree2 == null)
                checkpoint2Tree2 = flightControllerScript.levelManagerScript.SpawnSingleObstacle(flightControllerScript.gameModeScript.playerOnePlane, ref flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, checkpoint2Tree2PositionX, flightControllerScript.environmentManagerScript.environmentsScenarios[flightControllerScript.environmentManagerScript.scenarioIndex].verticalObstaclesPrefabs[2]);
        }
        else if (checkpointNumber == 3)
        {
            if (checkpoint3Fog == null)
                checkpoint3Fog = flightControllerScript.levelManagerScript.SpawnSingleObstacle(flightControllerScript.gameModeScript.playerOnePlane, ref flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, checkpoint3FogPositionX, flightControllerScript.environmentManagerScript.environmentsScenarios[flightControllerScript.environmentManagerScript.scenarioIndex].fogPrefab);
            if (checkpoint3Tree == null)
                checkpoint3Tree = flightControllerScript.levelManagerScript.SpawnSingleObstacle(flightControllerScript.gameModeScript.playerOnePlane, ref flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, checkpoint3TreePositionX, flightControllerScript.environmentManagerScript.environmentsScenarios[flightControllerScript.environmentManagerScript.scenarioIndex].verticalObstaclesPrefabs[0]);
            if (checkpoint3Launcher == null)
                checkpoint3Launcher = flightControllerScript.levelManagerScript.SpawnSingleObstacle(flightControllerScript.gameModeScript.playerOnePlane, ref flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, checkpoint3LauncherPositionX, flightControllerScript.levelManagerScript.trotylLauncherPrefab, "trotylLauncher");
            else
                checkpoint3Launcher.GetComponent<TrotylLauncher>().canShoot = true;
            checkpoint3Launcher.GetComponent<TrotylLauncher>().rateOfFireCounter = 2;
        }
        else if (checkpointNumber == 4)
        {
            if (checkpoint4PowerUp == null)
                checkpoint4PowerUp = flightControllerScript.levelManagerScript.SpawnSinglePowerUp(flightControllerScript.levelManagerScript.powerUpsPrefabs[checkpoint4PowerUpNumber], ref flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, checkpoint4PowerUpPosition.x, checkpoint4PowerUpPosition.y);
        }
        else if (checkpointNumber == 5)
        {
            if (checkpoint5Airport == null)
                checkpoint5Airport = flightControllerScript.levelManagerScript.SpawnAirport(flightControllerScript.gameModeScript.playerOnePlane, checkpoint5AirportPositionX);
        }
    }
    internal void RemoveOldLevelObstacles()
    {
        if (checkpointNumber == 2)
        {
            if (checkpoint1Launcher != null)
                Destroy(checkpoint1Launcher.gameObject);
        }
        else if (checkpointNumber == 4)
        {
            if (checkpoint3Fog != null)
                Destroy(checkpoint3Fog.gameObject);
            if (checkpoint3Tree != null)
                Destroy(checkpoint3Tree.gameObject);
            if (checkpoint3Launcher != null)
                Destroy(checkpoint3Launcher.gameObject);
        }
    }
}
