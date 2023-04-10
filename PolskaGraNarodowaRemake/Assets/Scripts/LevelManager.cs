using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject airportPrefab;
    public GameObject treePrefab;
    public GameObject trotylLauncherPrefab;
    public GameObject fogPrefab;
    public GameObject ObstaclesAndProjectilesGameObject;
    internal FlightController flightControllerScript;
    internal bool toNewLevel;
    private bool sameObstaclesForBothPlayes;
    private float distanceBetweenPlayers;
    //public GameplaySettings gameplaySettings;
    //public GameObject planeControlCenterGameObject;
    //public GameObject planeGameObject;
    //public LayerMask planeLayer;
    //internal PlaneScript PlaneScriptScript;


    //private GameObject afterAirportDestroyPointGameObject;

    private int numberOfObstacles;

    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        CalculatePlayerBoundries(flightControllerScript.gameModeScript.playerOnePlane);
        CalculatePlayerBoundries(flightControllerScript.gameModeScript.playerTwoPlane);
        distanceBetweenPlayers = Vector2.Distance(new Vector2(flightControllerScript.gameModeScript.playerOnePlane.groundLevelHeight, 0), new Vector2(flightControllerScript.gameModeScript.playerTwoPlane.groundLevelHeight, 0)); 
        sameObstaclesForBothPlayes = true;
        //PlaneScriptScript = planeControlCenterGameObject.GetComponent<PlaneScript>();
        //PlaneScriptScript.audioScript.PlaySound("TopGunTheme", PlaneScriptScript.audioScript.otherSounds);
    }
    void Update()
    {
        
    }
    
    internal void LoadLevel()
    {
        foreach (Transform child in ObstaclesAndProjectilesGameObject.transform)
            GameObject.Destroy(child.gameObject);
        if (toNewLevel)
        {
            toNewLevel = false;
            flightControllerScript.rewardAndProgressionManagerScript.levelCounter++;
            flightControllerScript.rewardAndProgressionManagerScript.totalBottlesDrunkPlayerOne = flightControllerScript.gameModeScript.playerOnePlane.bottleDrunkCounter;
            flightControllerScript.rewardAndProgressionManagerScript.totalBottlesDrunkPlayerTwo = flightControllerScript.gameModeScript.playerTwoPlane.bottleDrunkCounter;
            flightControllerScript.gameModeScript.playerOnePlane.ResetPlaneData();
            flightControllerScript.gameModeScript.playerTwoPlane.ResetPlaneData();
            //PlaneScriptScript.flightControllScript.waitingTimeAfterLandingCombinedWithSoundLength = 3f;
            //PlaneScriptScript.flightControllScript.rewardForLandingAdded = false;
            //PlaneScriptScript.audioScript.tiresSFXPlayed = false;
            //PlaneScriptScript.audioScript.landingSpeechPlayed = false;
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
        plane.groundLevelHeight = plane.cameraGameObject.transform.position.y - cameraH + 1;
    }
    private void CopyObstaclesFromPlayerOne()
    {
        GameObject[] clonedObjects = new GameObject[ObstaclesAndProjectilesGameObject.transform.childCount];
        int i = 0;
        foreach (Transform child in ObstaclesAndProjectilesGameObject.transform)
        {
            clonedObjects[i] = Instantiate(child.gameObject, new Vector3(child.position.x, child.position.y - distanceBetweenPlayers, 0), Quaternion.identity);
            clonedObjects[i].transform.name = child.name;
            if (child.GetComponent<TrotylLauncher>())
                clonedObjects[i].GetComponent<TrotylLauncher>().rateOfFireCounter = child.GetComponent<TrotylLauncher>().rateOfFireCounter;
            i++;
        }
        for (i = 0; i < clonedObjects.Length; i++)
            clonedObjects[i].transform.parent = ObstaclesAndProjectilesGameObject.transform;

        //var aaa = new List<GameObject>();
        //foreach (Transform child in ObstaclesAndProjectilesGameObject.transform)
        //{

        //    var aa = Instantiate(child.gameObject, new Vector3(child.position.x, child.position.y - distanceBetweenPlayers, 0), Quaternion.identity);
        //    aaa.Add(aa);
        //}

        //foreach (var a in aaa)
        //{
        //    a.transform.parent = ObstaclesAndProjectilesGameObject.transform;
        //}
    }
    private void SpawnObstacles(Plane plane)
    {
        double sectorWidth = 0.8 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance / numberOfObstacles;
        for (int i = 1; i < numberOfObstacles + 1; i++)
        {
            int obstacle = Random.Range(0, 2);
            if (obstacle == 0) //TREE
            {
                int treeHeight = Random.Range(0, 3);
                float offsetY = treePrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                GameObject tree = Instantiate(treePrefab, new Vector3((float)((0.1 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance) + (i * sectorWidth)), flightControllerScript.gameModeScript.playerOnePlane.groundLevelHeight, 0), Quaternion.identity, ObstaclesAndProjectilesGameObject.transform);
                tree.name = "birchTree";
                if (treeHeight == 0)
                {
                    tree.transform.position += new Vector3(0, offsetY - 1f, 0);
                }
                if (treeHeight == 1)
                {
                    tree.transform.localScale = new Vector3(2, 2, 1);
                    tree.transform.position += new Vector3(0, 2 * offsetY - 1f, 0);
                }
                else if (treeHeight == 2)
                {
                    tree.transform.localScale = new Vector3(3, 3, 1);
                    tree.transform.position += new Vector3(0, 3 * offsetY - 1f, 0);
                }

            }
            else if (obstacle == 1) //TROTYLLAUNCHER
            {
                GameObject trotylLauncher = Instantiate(trotylLauncherPrefab, new Vector3((float)((0.1 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance) + (i * sectorWidth)), plane.groundLevelHeight, 0), Quaternion.identity, ObstaclesAndProjectilesGameObject.transform);
                trotylLauncher.name = "trotylLauncher";
            }
        }
        int numberOfFogInstances = numberOfObstacles / 4;
        for (int i = 0; i < numberOfFogInstances; i++)
        {
            float fogPlacementX = Random.Range((float)0.1 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance, (float)0.9 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance);
            GameObject fog = Instantiate(fogPrefab, new Vector3(fogPlacementX, (plane.topScreenHeight - plane.groundLevelHeight) / 2, 0), Quaternion.identity, ObstaclesAndProjectilesGameObject.transform);
            fog.name = "fog";
        }
    }
    private void SpawnAirpot(Plane plane)
    {
        GameObject airport = Instantiate(airportPrefab, new Vector3((float)1.25 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance, plane.groundLevelHeight, 0), Quaternion.identity, ObstaclesAndProjectilesGameObject.transform);
        airport.name = "airport";
    }
}
