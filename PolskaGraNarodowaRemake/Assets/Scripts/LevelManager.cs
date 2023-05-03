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
    public GameObject ObstaclesAndProjectilesGameObject;
    internal FlightController flightControllerScript;
    public bool sameObstaclesForBothPlayes;
    private float distanceBetweenPlayers;
    private int numberOfObstacles;

    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        CalculatePlayerBoundries(flightControllerScript.gameModeScript.playerOnePlane);
        CalculatePlayerBoundries(flightControllerScript.gameModeScript.playerTwoPlane);
        distanceBetweenPlayers = Vector2.Distance(new Vector2(flightControllerScript.gameModeScript.playerOnePlane.groundLevelHeight, 0), new Vector2(flightControllerScript.gameModeScript.playerTwoPlane.groundLevelHeight, 0));
    }
    internal void LoadLevel()
    {
        while (ObstaclesAndProjectilesGameObject.transform.childCount > 0)
            DestroyImmediate(ObstaclesAndProjectilesGameObject.transform.GetChild(0).gameObject);
        if (flightControllerScript.rewardAndProgressionManagerScript.toNewLevel)
        {
            flightControllerScript.rewardAndProgressionManagerScript.toNewLevel = false;
            flightControllerScript.rewardAndProgressionManagerScript.levelCounter++;
            flightControllerScript.gameModeScript.playerOnePlane.ResetPlaneData();
            flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter = 0;
            flightControllerScript.rewardAndProgressionManagerScript.totalBottlesDrunkPlayerOne = flightControllerScript.gameModeScript.playerOnePlane.bottleDrunkCounter;
            if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayer)
            {
                flightControllerScript.gameModeScript.playerTwoPlane.ResetPlaneData();
                flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter = 0;
                flightControllerScript.rewardAndProgressionManagerScript.totalBottlesDrunkPlayerTwo = flightControllerScript.gameModeScript.playerTwoPlane.bottleDrunkCounter;
            }
        }
        if(!flightControllerScript.audioManagerScript.IsTheSoundCurrentlyPlaying("EngineSound", flightControllerScript.audioManagerScript.SFX))
            flightControllerScript.audioManagerScript.PlaySound("EngineSound", flightControllerScript.audioManagerScript.SFX);
        numberOfObstacles = 1 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter * 2;
        flightControllerScript.gameModeScript.playerOnePlane.planeGameObject.transform.position = new Vector3(0, (flightControllerScript.gameModeScript.playerOnePlane.topScreenHeight + flightControllerScript.gameModeScript.playerOnePlane.groundLevelHeight) / 2, 0);
        if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayer)
            flightControllerScript.gameModeScript.playerTwoPlane.planeGameObject.transform.position = new Vector3(0, (flightControllerScript.gameModeScript.playerTwoPlane.topScreenHeight + flightControllerScript.gameModeScript.playerTwoPlane.groundLevelHeight) / 2, 0);
        SpawnObstacles(flightControllerScript.gameModeScript.playerOnePlane);
        if (flightControllerScript.gameModeScript.currentGameMode != GameModeManager.GameMode.singleplayer)
        {
            if (sameObstaclesForBothPlayes)
                CopyObstaclesFromPlayerOne();
            else
                SpawnObstacles(flightControllerScript.gameModeScript.playerTwoPlane);
            SpawnAirpot(flightControllerScript.gameModeScript.playerTwoPlane);
        } 
        SpawnAirpot(flightControllerScript.gameModeScript.playerOnePlane);
    }
    internal void CalculatePlayerBoundries(Plane plane)
    {
        float cameraH = plane.cameraGameObject.GetComponent<Camera>().orthographicSize;
        plane.topScreenHeight = plane.cameraGameObject.transform.position.y + cameraH - 1;
        //plane.groundLevelHeight = plane.cameraGameObject.transform.position.y - cameraH + 1;
        plane.groundLevelHeight = plane.cameraGameObject.transform.Find("Ground").position.y + plane.cameraGameObject.transform.Find("Ground").GetComponent<BoxCollider2D>().size.y / 2;
    }
    private void CopyObstaclesFromPlayerOne()
    {
        List<GameObject> clonedObjects = new List<GameObject>();
        foreach (Transform child in ObstaclesAndProjectilesGameObject.transform)
        {
            clonedObjects.Add(Instantiate(child.gameObject, new Vector3(child.position.x, child.position.y - distanceBetweenPlayers, 0), Quaternion.identity));
            clonedObjects[clonedObjects.Count - 1].transform.name = child.name;
            if (child.GetComponent<TrotylLauncher>())
                clonedObjects[clonedObjects.Count - 1].GetComponent<TrotylLauncher>().rateOfFireCounter = child.GetComponent<TrotylLauncher>().rateOfFireCounter;
        }
        foreach (GameObject child in clonedObjects)
            child.transform.parent = ObstaclesAndProjectilesGameObject.transform;
    }
    private void SpawnObstacles(Plane plane)
    {
        double sectorWidth = 0.8 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance / numberOfObstacles;
        for (int i = 1; i < numberOfObstacles + 1; i++)
        {
            int obstacle = Random.Range(0, 2);
            if (obstacle == 0) //TREE
            {
                int treeHeight = Random.Range(0, treePrefab.Length);
                GameObject tree = Instantiate(treePrefab[treeHeight], new Vector3((float)((0.1 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance) + (i * sectorWidth)), plane.groundLevelHeight + treePrefab[treeHeight].GetComponent<BoxCollider2D>().size.y / 2, 0), Quaternion.identity, ObstaclesAndProjectilesGameObject.transform);
                tree.name = "birchTree";
            }
            else if (obstacle == 1) //TROTYLLAUNCHER
            {
                GameObject trotylLauncher = Instantiate(trotylLauncherPrefab, new Vector3((float)((0.1 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance) + (i * sectorWidth)), plane.groundLevelHeight + trotylLauncherPrefab.GetComponent<BoxCollider2D>().size.y / 2, 0), Quaternion.identity, ObstaclesAndProjectilesGameObject.transform);
                trotylLauncher.name = "trotylLauncher";
            }
        }
        int numberOfFogInstances = numberOfObstacles / 4;
        for (int i = 0; i < numberOfFogInstances; i++)
        {
            float fogPlacementX = Random.Range((float)0.1 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance, (float)0.9 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance);
            GameObject fog = Instantiate(fogPrefab, new Vector3(fogPlacementX, (plane.topScreenHeight + plane.groundLevelHeight) / 2, 0), Quaternion.identity, ObstaclesAndProjectilesGameObject.transform);
            fog.name = "fog";
        }
    }
    private void SpawnAirpot(Plane plane)
    {
        GameObject airport = Instantiate(airportPrefab, new Vector3((float)1.25 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance, plane.groundLevelHeight + airportPrefab.GetComponent<BoxCollider2D>().size.y / 2, 0), Quaternion.identity, ObstaclesAndProjectilesGameObject.transform);
        airport.name = "airport";
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
