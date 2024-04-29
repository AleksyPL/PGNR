using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject airportPrefab;
    public GameObject trotylLauncherPrefab;
    public GameObject endlessModeSpawnerPrefab;
    public GameObject[] powerUpsPrefabs;
    public GameObject obstaclesAndProjectilesParentGameObject;
    public GameObject powerUpsParentGameObject;
    public bool enableObstacles;
    public bool sameObstaclesForBothPlayes;
    public bool enablePowerUps;
    public int numberOfPowerUpsPerSector;
    internal GameObject obstaclesBufferGameObject;
    internal FlightController flightControllerScript;
    private float distanceBetweenPlayers;
    internal int numberOfObstacles;
    internal int numberOfFogInstances;
    internal double obstacleSectorWidth;
    [SerializeField] internal float endlessModeSpawnerPlayerTwoOffset;

    void OnEnable()
    {
        flightControllerScript = GetComponent<FlightController>();
        CalculatePlayerBoundries(flightControllerScript.GetComponent<GameModeManager>().playerOnePlane);
        if (flightControllerScript.GetComponent<GameModeManager>().currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.GetComponent<GameModeManager>().currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            CalculatePlayerBoundries(flightControllerScript.GetComponent<GameModeManager>().playerTwoPlane);
            distanceBetweenPlayers = Vector2.Distance(new Vector2(flightControllerScript.GetComponent<GameModeManager>().playerOnePlane.groundLevelHeight, 0), new Vector2(flightControllerScript.GetComponent<GameModeManager>().playerTwoPlane.groundLevelHeight, 0));
        }
    }
    private void RemoveAllChildrenOfTheGameObject(GameObject sourceGameObject)
    {
        while (sourceGameObject.transform.childCount > 0)
            DestroyImmediate(sourceGameObject.transform.GetChild(0).gameObject);
    }
    internal void LoadLevel()
    {
        flightControllerScript = GetComponent<FlightController>();
        flightControllerScript.environmentManagerScript.SpawnBackgroundImage();
        RemoveAllChildrenOfTheGameObject(obstaclesAndProjectilesParentGameObject);
        if (obstaclesBufferGameObject == null)
        {
            obstaclesBufferGameObject = new GameObject("Obstacles Buffer");
            obstaclesBufferGameObject.transform.position = new Vector3(0, 0, 0);
        }
        if (flightControllerScript.rewardAndProgressionManagerScript.toNewLevel)
        {
            numberOfObstacles += 2;
            numberOfFogInstances = flightControllerScript.rewardAndProgressionManagerScript.levelCounter / 3 + 1;
            flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance += 10;
            flightControllerScript.rewardAndProgressionManagerScript.toNewLevel = false;
            flightControllerScript.rewardAndProgressionManagerScript.levelCounter++;
            flightControllerScript.gameModeScript.playerOnePlane.ResetPlaneData();
            flightControllerScript.rewardAndProgressionManagerScript.playerOneProgress.levelProgressCounter = 0;
            flightControllerScript.gameModeScript.playerOnePlane.SoberUp();
            flightControllerScript.uiManagerScript.UpdateBottlesCounter(flightControllerScript.gameModeScript.playerOnePlane);
            if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
            {
                flightControllerScript.gameModeScript.playerTwoPlane.ResetPlaneData();
                flightControllerScript.rewardAndProgressionManagerScript.playerTwoProgress.levelProgressCounter = 0;
                flightControllerScript.gameModeScript.playerTwoPlane.SoberUp();
                flightControllerScript.uiManagerScript.UpdateBottlesCounter(flightControllerScript.gameModeScript.playerTwoPlane);
            }
        }
        if (!flightControllerScript.audioManagerScript.IsTheSoundCurrentlyPlaying("EngineSound", flightControllerScript.audioManagerScript.localSFX))
            flightControllerScript.audioManagerScript.PlaySound("EngineSound", flightControllerScript.audioManagerScript.localSFX);
        //singleplayer
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic)
        {
            flightControllerScript.gameModeScript.playerOnePlane.planeGameObject.transform.position = new Vector3(0 - flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace, (flightControllerScript.gameModeScript.playerOnePlane.topScreenHeight + flightControllerScript.gameModeScript.playerOnePlane.groundLevelHeight) / 2, 0);
            FillTheLevelWithObstacles(flightControllerScript.gameModeScript.playerOnePlane, ref obstaclesBufferGameObject, 0, flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
            if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic)
            {
                flightControllerScript.gameModeScript.playerTwoPlane.planeGameObject.transform.position = new Vector3(0 - flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace, (flightControllerScript.gameModeScript.playerTwoPlane.topScreenHeight + flightControllerScript.gameModeScript.playerTwoPlane.groundLevelHeight) / 2, 0);
                if (sameObstaclesForBothPlayes)
                    CopyObstaclesFromOnePlayerToAnother(ref obstaclesBufferGameObject, ref obstaclesAndProjectilesParentGameObject);
                else
                    FillTheLevelWithObstacles(flightControllerScript.gameModeScript.playerTwoPlane, ref obstaclesAndProjectilesParentGameObject, 0, flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                SpawnAirpot(flightControllerScript.gameModeScript.playerTwoPlane);
            }
            else
                MoveObstaclesFromOneObjectToAnother(ref obstaclesBufferGameObject, ref obstaclesAndProjectilesParentGameObject);
            SpawnAirpot(flightControllerScript.gameModeScript.playerOnePlane);
        }
        //multiplayer
        else if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            numberOfObstacles = 5;
            numberOfFogInstances = 1;
            flightControllerScript.gameModeScript.playerOnePlane.planeGameObject.transform.position = new Vector3(0 - flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace, (flightControllerScript.gameModeScript.playerOnePlane.topScreenHeight + flightControllerScript.gameModeScript.playerOnePlane.groundLevelHeight) / 2, 0);
            FillTheLevelWithObstacles(flightControllerScript.gameModeScript.playerOnePlane, ref obstaclesBufferGameObject, 0, flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
            SpawnPowerUps(flightControllerScript.gameModeScript.playerOnePlane, powerUpsParentGameObject, flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace + 10, flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
            if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
            {
                flightControllerScript.gameModeScript.playerTwoPlane.planeGameObject.transform.position = new Vector3(0 - flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace, (flightControllerScript.gameModeScript.playerTwoPlane.topScreenHeight + flightControllerScript.gameModeScript.playerTwoPlane.groundLevelHeight) / 2, 0);
                if (sameObstaclesForBothPlayes)
                    CopyObstaclesFromOnePlayerToAnother(ref obstaclesBufferGameObject, ref obstaclesAndProjectilesParentGameObject);
                else
                    FillTheLevelWithObstacles(flightControllerScript.gameModeScript.playerTwoPlane, ref obstaclesAndProjectilesParentGameObject, 0, flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                SpawnPowerUps(flightControllerScript.gameModeScript.playerTwoPlane, powerUpsParentGameObject, flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace + 10, flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                SpawnEndlessModeSpawner(flightControllerScript.gameModeScript.playerOnePlane, obstaclesAndProjectilesParentGameObject, (int)(flightControllerScript.rewardAndProgressionManagerScript.playerOneProgress.levelProgressCounter / flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + (float)(0.25 * flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance));
                SpawnEndlessModeSpawner(flightControllerScript.gameModeScript.playerTwoPlane, obstaclesAndProjectilesParentGameObject, (int)(flightControllerScript.rewardAndProgressionManagerScript.playerTwoProgress.levelProgressCounter / flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + (float)(0.25 * flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + endlessModeSpawnerPlayerTwoOffset);
            }
            else
            {
                MoveObstaclesFromOneObjectToAnother(ref obstaclesBufferGameObject, ref obstaclesAndProjectilesParentGameObject);
                SpawnEndlessModeSpawner(flightControllerScript.gameModeScript.playerOnePlane, obstaclesAndProjectilesParentGameObject, (int)(flightControllerScript.rewardAndProgressionManagerScript.playerOneProgress.levelProgressCounter / flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + (float)(0.25 * flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance));
            }
        }
        //tutorial
        else
        {

        }
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic)
            if(flightControllerScript.gameplaySettings.safeMode)
                flightControllerScript.uiManagerScript.EnableBeforeTheFlightProcedure(flightControllerScript.gameplaySettings.localizationsStrings[flightControllerScript.gameplaySettings.langauageIndex].regularHudLevel + flightControllerScript.rewardAndProgressionManagerScript.levelCounter.ToString() + flightControllerScript.gameplaySettings.localizationsStrings[flightControllerScript.gameplaySettings.langauageIndex].classicModeMessage, 3f);
            else
                flightControllerScript.uiManagerScript.EnableBeforeTheFlightProcedure(flightControllerScript.gameplaySettings.localizationsStrings[flightControllerScript.gameplaySettings.langauageIndex].regularHudLevel + (2009 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter).ToString() + flightControllerScript.gameplaySettings.localizationsStrings[flightControllerScript.gameplaySettings.langauageIndex].classicModeMessage, 3f);
        else if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
            flightControllerScript.uiManagerScript.EnableBeforeTheFlightProcedure(flightControllerScript.gameplaySettings.localizationsStrings[flightControllerScript.gameplaySettings.langauageIndex].endlessSingleMessage, 3f);
    }
    internal void CalculatePlayerBoundries(Plane plane)
    {
        float cameraH = plane.cameraGameObject.GetComponent<Camera>().orthographicSize;
        plane.topScreenHeight = plane.cameraGameObject.transform.position.y + cameraH - 1;
        plane.groundLevelHeight = plane.cameraGameObject.transform.Find("Ground").position.y + plane.cameraGameObject.transform.Find("Ground").GetComponent<BoxCollider2D>().size.y / 2;
    }
    internal void CopyObstaclesFromOnePlayerToAnother(ref GameObject sourceGameObject, ref GameObject finalGameObject)
    {
        if(enableObstacles)
        {
            List<GameObject> clonedObjects = new();
            foreach (Transform child in sourceGameObject.transform)
            {
                clonedObjects.Add(Instantiate(child.gameObject, new Vector3(child.position.x, child.position.y - distanceBetweenPlayers, 0), Quaternion.identity));
                clonedObjects[clonedObjects.Count - 1].transform.name = child.name;
                if (child.GetComponent<TrotylLauncher>())
                    clonedObjects[clonedObjects.Count - 1].GetComponent<TrotylLauncher>().rateOfFireCounter = child.GetComponent<TrotylLauncher>().rateOfFireCounter;
                //Spawning the same object twice hack
                if (child.transform.name == "SpawnNewObjects")
                    clonedObjects[clonedObjects.Count - 1].transform.position = new Vector3(clonedObjects[clonedObjects.Count - 1].transform.position.x + 0.2f, clonedObjects[clonedObjects.Count - 1].transform.position.y, 0);
            }
            foreach (GameObject child in clonedObjects)
                child.transform.parent = finalGameObject.transform;
            MoveObstaclesFromOneObjectToAnother(ref sourceGameObject, ref finalGameObject);
            Destroy(sourceGameObject);
        }
    }
    internal void MoveObstaclesFromOneObjectToAnother(ref GameObject sourceGameObject, ref GameObject finalGameObject)
    {
        while (sourceGameObject.transform.childCount > 0)
            sourceGameObject.transform.GetChild(0).gameObject.transform.parent = finalGameObject.transform;
        Destroy(sourceGameObject);
    }
    private void SpawnSingleObstacle(Plane plane, ref GameObject obstaclesParent, float placementPointX, GameObject prefabToSpawn, string gameObjectName="")
    {
        GameObject newObstacle = Instantiate(prefabToSpawn, new Vector3(placementPointX, plane.groundLevelHeight,0), Quaternion.identity, obstaclesParent.transform);
        if(newObstacle.GetComponent<BoxCollider2D>())
        {
            float distanceBetweenPoints = newObstacle.transform.position.y - newObstacle.GetComponent<BoxCollider2D>().bounds.center.y;
            newObstacle.transform.position = new Vector3(newObstacle.transform.position.x, newObstacle.transform.position.y + distanceBetweenPoints, 0);
        }
        if (gameObjectName == "")
            newObstacle.name = "verticalObstacle";
        else
            newObstacle.name = gameObjectName;
    }
    internal void FillTheLevelWithObstacles(Plane plane, ref GameObject obstaclesParent, float placementPointXstart, float placementPointXFinish, float placementPointXOffset = 0)
    {
        if(enableObstacles)
        {
            obstacleSectorWidth = (placementPointXFinish - placementPointXstart) / numberOfObstacles;
            for (int i = 0; i < numberOfObstacles; i++)
            {
                int obstacle = Random.Range(0, 2);
                if (obstacle == 0) //VERTICAL OBSTACLE
                {
                    int objectIndex = Random.Range(0, flightControllerScript.environmentManagerScript.environmentsScenarios[flightControllerScript.environmentManagerScript.scenarioIndex].verticalObstaclesPrefabs.Length);
                    SpawnSingleObstacle(plane, ref obstaclesParent, (float)(placementPointXstart + placementPointXOffset + (i * obstacleSectorWidth) + (obstacleSectorWidth / 2)), flightControllerScript.environmentManagerScript.environmentsScenarios[flightControllerScript.environmentManagerScript.scenarioIndex].verticalObstaclesPrefabs[objectIndex], "verticalObstacle");
                }
                else if (obstacle == 1) //TROTYLLAUNCHER
                    SpawnSingleObstacle(plane, ref obstaclesParent, (float)(placementPointXstart + placementPointXOffset + (i * obstacleSectorWidth) + (obstacleSectorWidth / 2)), trotylLauncherPrefab, "trotylLauncher");
            }
            for (int i = 0; i < numberOfFogInstances; i++)
            {
                //FOG
                GameObject fog = Instantiate(flightControllerScript.environmentManagerScript.environmentsScenarios[flightControllerScript.environmentManagerScript.scenarioIndex].fogPrefab, new Vector3(Random.Range(placementPointXstart, placementPointXFinish) + placementPointXOffset, (plane.topScreenHeight + plane.groundLevelHeight) / 2, 0), Quaternion.identity, obstaclesParent.transform);
                fog.name = "fog";
            }
        }
    }
    internal void SpawnPowerUps(Plane plane, GameObject powerUpParent, float placementPointXstart, float placementPointXFinish)
    {
        if(powerUpsPrefabs.Length !=0 && enablePowerUps && numberOfPowerUpsPerSector >= 1 && (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless))
        {
            for (int i = 0; i < numberOfPowerUpsPerSector; i++)
            {
                int powerUpNumber = Random.Range(0, powerUpsPrefabs.Length);
                GameObject powerUpObject = Instantiate(powerUpsPrefabs[powerUpNumber], new Vector3(Random.Range(placementPointXstart, placementPointXFinish), Random.Range(plane.topScreenHeight, plane.groundLevelHeight + 2), 0), Quaternion.identity, powerUpParent.transform);
                powerUpObject.name = powerUpObject.GetComponent<PowerUpObject>().currentPowerUpScriptableObject.powerUpName;
            }
        }
    }
    internal void SpawnEndlessModeSpawner(Plane plane, GameObject parentGameObject, float placementPointX)
    {
        if (endlessModeSpawnerPrefab != null && (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless))
        {
            GameObject endlessModeSpawner = Instantiate(endlessModeSpawnerPrefab, new Vector3(placementPointX, (plane.topScreenHeight + plane.groundLevelHeight) / 2, 0), Quaternion.identity, parentGameObject.transform);
            endlessModeSpawner.name = "SpawnNewObjects";
        }
    }
    private void SpawnAirpot(Plane plane)
    {
        GameObject airport = Instantiate(airportPrefab, new Vector3(flightControllerScript.rewardAndProgressionManagerScript.levelLandingSpace + flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance, plane.groundLevelHeight + airportPrefab.GetComponent<BoxCollider2D>().size.y / 2, 0), Quaternion.identity, obstaclesAndProjectilesParentGameObject.transform);
        airport.name = "airport";
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        flightControllerScript.audioManagerScript.StopPlayingAllPausedSounds();
        SceneManager.LoadScene("MainMenu");
    }
}
