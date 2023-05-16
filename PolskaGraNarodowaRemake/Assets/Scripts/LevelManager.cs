using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject airportPrefab;
    public GameObject[] treePrefab;
    public GameObject trotylLauncherPrefab;
    public GameObject fogPrefab;
    public GameObject endlessModeSpawnerPrefab;
    public GameObject obstaclesAndProjectilesGameObject;
    internal GameObject obstaclesBufferGameObject;
    internal FlightController flightControllerScript;
    public bool sameObstaclesForBothPlayes;
    private float distanceBetweenPlayers;
    internal int numberOfObstacles;
    internal int numberOfFogInstances;
    internal double obstacleSectorWidth;

    void OnEnable()
    {
        flightControllerScript = GetComponent<FlightController>();
        CalculatePlayerBoundries(flightControllerScript.GetComponent<GameModeManager>().playerOnePlane);
        if (flightControllerScript.GetComponent<GameModeManager>().currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.GetComponent<GameModeManager>().currentGameMode != GameModeManager.GameMode.singleplayerEndless)
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
        RemoveAllChildrenOfTheGameObject(obstaclesAndProjectilesGameObject);
        if (flightControllerScript.rewardAndProgressionManagerScript.toNewLevel)
        {
            numberOfObstacles += 2;
            numberOfFogInstances = flightControllerScript.rewardAndProgressionManagerScript.levelCounter / 3 + 1;
            flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance += 10;
            flightControllerScript.rewardAndProgressionManagerScript.toNewLevel = false;
            flightControllerScript.rewardAndProgressionManagerScript.levelCounter++;
            flightControllerScript.gameModeScript.playerOnePlane.ResetPlaneData();
            flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter = 0;
            flightControllerScript.rewardAndProgressionManagerScript.totalBottlesDrunkPlayerOne = flightControllerScript.gameModeScript.playerOnePlane.bottleDrunkCounter;
            if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
            {
                flightControllerScript.gameModeScript.playerTwoPlane.ResetPlaneData();
                flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter = 0;
                flightControllerScript.rewardAndProgressionManagerScript.totalBottlesDrunkPlayerTwo = flightControllerScript.gameModeScript.playerTwoPlane.bottleDrunkCounter;
            }
        }
        if (!flightControllerScript.audioManagerScript.IsTheSoundCurrentlyPlaying("EngineSound", flightControllerScript.audioManagerScript.SFX))
            flightControllerScript.audioManagerScript.PlaySound("EngineSound", flightControllerScript.audioManagerScript.SFX);
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic)
        {
            flightControllerScript.gameModeScript.playerOnePlane.planeGameObject.transform.position = new Vector3(0, (flightControllerScript.gameModeScript.playerOnePlane.topScreenHeight + flightControllerScript.gameModeScript.playerOnePlane.groundLevelHeight) / 2, 0);
            SpawnObstacles(flightControllerScript.gameModeScript.playerOnePlane, ref obstaclesBufferGameObject);
            if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerClassic && flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayerEndless)
            {
                flightControllerScript.gameModeScript.playerTwoPlane.planeGameObject.transform.position = new Vector3(0, (flightControllerScript.gameModeScript.playerTwoPlane.topScreenHeight + flightControllerScript.gameModeScript.playerTwoPlane.groundLevelHeight) / 2, 0);
                if (sameObstaclesForBothPlayes)
                    CopyObstaclesFromOnePlayerToAnother(ref obstaclesBufferGameObject, ref obstaclesAndProjectilesGameObject);
                else
                    SpawnObstacles(flightControllerScript.gameModeScript.playerTwoPlane, ref obstaclesAndProjectilesGameObject);
                SpawnAirpot(flightControllerScript.gameModeScript.playerTwoPlane);
            }
            else
                MoveObstaclesFromOneObjectToAnother(ref obstaclesBufferGameObject, ref obstaclesAndProjectilesGameObject);
            SpawnAirpot(flightControllerScript.gameModeScript.playerOnePlane);
        }
        else
        {
            numberOfObstacles = 5;
            numberOfFogInstances = 1;
            flightControllerScript.gameModeScript.playerOnePlane.planeGameObject.transform.position = new Vector3(0, (flightControllerScript.gameModeScript.playerOnePlane.topScreenHeight + flightControllerScript.gameModeScript.playerOnePlane.groundLevelHeight) / 2, 0);
            SpawnObstacles(flightControllerScript.gameModeScript.playerOnePlane, ref obstaclesBufferGameObject);
            if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
            {
                flightControllerScript.gameModeScript.playerTwoPlane.planeGameObject.transform.position = new Vector3(0, (flightControllerScript.gameModeScript.playerTwoPlane.topScreenHeight + flightControllerScript.gameModeScript.playerTwoPlane.groundLevelHeight) / 2, 0);
                if (sameObstaclesForBothPlayes)
                    CopyObstaclesFromOnePlayerToAnother(ref obstaclesBufferGameObject, ref obstaclesAndProjectilesGameObject);
                else
                    SpawnObstacles(flightControllerScript.gameModeScript.playerTwoPlane, ref obstaclesAndProjectilesGameObject);
            }
            else
                MoveObstaclesFromOneObjectToAnother(ref obstaclesBufferGameObject, ref obstaclesAndProjectilesGameObject);
        }
    }
    internal void CalculatePlayerBoundries(Plane plane)
    {
        float cameraH = plane.cameraGameObject.GetComponent<Camera>().orthographicSize;
        plane.topScreenHeight = plane.cameraGameObject.transform.position.y + cameraH - 1;
        plane.groundLevelHeight = plane.cameraGameObject.transform.Find("Ground").position.y + plane.cameraGameObject.transform.Find("Ground").GetComponent<BoxCollider2D>().size.y / 2;
    }
    internal void CopyObstaclesFromOnePlayerToAnother(ref GameObject sourceGameObject, ref GameObject finalGameObject)
    {
        List<GameObject> clonedObjects = new List<GameObject>();
        foreach (Transform child in sourceGameObject.transform)
        {
            clonedObjects.Add(Instantiate(child.gameObject, new Vector3(child.position.x, child.position.y - distanceBetweenPlayers, 0), Quaternion.identity));
            clonedObjects[clonedObjects.Count - 1].transform.name = child.name;
            if (child.GetComponent<TrotylLauncher>())
                clonedObjects[clonedObjects.Count - 1].GetComponent<TrotylLauncher>().rateOfFireCounter = child.GetComponent<TrotylLauncher>().rateOfFireCounter;
            //Spawning the same object twice hack
            if(child.transform.name == "SpawnNewObjects")
                clonedObjects[clonedObjects.Count - 1].transform.position = new Vector3(clonedObjects[clonedObjects.Count - 1].transform.position.x + 0.2f, clonedObjects[clonedObjects.Count - 1].transform.position.y, 0);
        }
        foreach (GameObject child in clonedObjects)
        {
            child.transform.parent = finalGameObject.transform;
        }
        MoveObstaclesFromOneObjectToAnother(ref sourceGameObject, ref finalGameObject);
        Destroy(sourceGameObject);
    }
    internal void MoveObstaclesFromOneObjectToAnother(ref GameObject sourceGameObject, ref GameObject finalGameObject)
    {
        while (sourceGameObject.transform.childCount > 0)
            sourceGameObject.transform.GetChild(0).gameObject.transform.parent = finalGameObject.transform;
        Destroy(sourceGameObject);
    }
    internal void SpawnObstacles(Plane plane, ref GameObject obstaclesParent, float obstaclesXOffset = 0)
    {
        obstacleSectorWidth = flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance / numberOfObstacles;
        if (obstaclesBufferGameObject == null)
        {
            obstaclesBufferGameObject = new GameObject("Obstacles Buffer");
            obstaclesBufferGameObject.transform.position = new Vector3(0, 0, 0);
        }
        for (int i = 0; i < numberOfObstacles; i++)
        {
            int obstacle = Random.Range(0, 2);
            if (obstacle == 0) //TREE
            {
                int treeHeight = Random.Range(0, treePrefab.Length);
                GameObject tree = Instantiate(treePrefab[treeHeight], new Vector3((float)((i * obstacleSectorWidth) + (obstacleSectorWidth / 2) + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) + obstaclesXOffset, plane.groundLevelHeight + treePrefab[treeHeight].GetComponent<BoxCollider2D>().size.y / 2, 0), Quaternion.identity, obstaclesParent.transform);
                tree.name = "birchTree";
            }
            else if (obstacle == 1) //TROTYLLAUNCHER
            {
                GameObject trotylLauncher = Instantiate(trotylLauncherPrefab, new Vector3((float)((i * obstacleSectorWidth) + (obstacleSectorWidth / 2) + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) + obstaclesXOffset, plane.groundLevelHeight + trotylLauncherPrefab.GetComponent<BoxCollider2D>().size.y / 2 + 0.2f, 0), Quaternion.identity, obstaclesParent.transform);
                trotylLauncher.name = "trotylLauncher";
            }
            if (i == 1 && endlessModeSpawnerPrefab != null && (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless))
            {
                GameObject endlessModeSpawner = Instantiate(endlessModeSpawnerPrefab, new Vector3((float)((i * obstacleSectorWidth) + (obstacleSectorWidth / 2) + flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace) + obstaclesXOffset, (plane.topScreenHeight + plane.groundLevelHeight) / 2, 0), Quaternion.identity, obstaclesParent.transform);
                endlessModeSpawner.name = "SpawnNewObjects";
            }
        }
        for (int i = 0; i < numberOfFogInstances; i++)
        {
            float fogPlacementX = Random.Range(0 , flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance);
            GameObject fog = Instantiate(fogPrefab, new Vector3(fogPlacementX + obstaclesXOffset, (plane.topScreenHeight + plane.groundLevelHeight) / 2, 0), Quaternion.identity, obstaclesParent.transform);
            fog.name = "fog";
        }
    }
    private void SpawnAirpot(Plane plane)
    {
        GameObject airport = Instantiate(airportPrefab, new Vector3(flightControllerScript.rewardAndProgressionManagerScript.levelLandingSpace + flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance, plane.groundLevelHeight + airportPrefab.GetComponent<BoxCollider2D>().size.y / 2, 0), Quaternion.identity, obstaclesAndProjectilesGameObject.transform);
        airport.name = "airport";
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
