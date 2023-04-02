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
        //PlaneScriptScript = planeControlCenterGameObject.GetComponent<PlaneScript>();
        //PlaneScriptScript.audioScript.PlaySound("TopGunTheme", PlaneScriptScript.audioScript.otherSounds);
        RestartGame();
    }
    void Update()
    {
        
    }
    public void RestartGame()
    {
        //levelCounter = 1;
        //gameScore = 0;
        //PlaneScriptScript.flightControllScript.drunkBottlesInTotal = 0;
        //// PlaneScriptScript.difficultyScript.difficultyMultiplier = 0;
        //PlaneScriptScript.flightControllScript.waitingTimeAfterLandingCurrent = 0;
        //if (PlaneScriptScript.smokeSpawner.transform.childCount != 0)
        //{
        //    foreach (Transform child in PlaneScriptScript.smokeSpawner.transform)
        //        GameObject.Destroy(child.gameObject);
        //}
        LoadLevel();
    }
    internal void LoadLevel()
    {
        //PlaneScriptScript.currentPlaneState = PlaneScript.PlaneState.standard;
        //PlaneScriptScript.planeRendererScript.ChangePlaneSprite();
        //PlaneScriptScript.UIScript.DisableOptionsMenu();
        //PlaneScriptScript.UIScript.DisableGameOverScreen();
        //if (PlaneScriptScript.flightControllScript.toNewLevel)
        //{
        //    PlaneScriptScript.flightControllScript.toNewLevel = false;
        //    levelCounter++;
        //    PlaneScriptScript.flightControllScript.currentPlaneSpeed = gameplaySettings.defaultPlaneSpeed;
        //    PlaneScriptScript.flightControllScript.isTouchingAirport = false;
        //    PlaneScriptScript.flightControllScript.isTouchingGround = false;
        //    PlaneScriptScript.flightControllScript.waitingTimeAfterLandingCombinedWithSoundLength = 3f;
        //    PlaneScriptScript.flightControllScript.rewardForLandingAdded = false;
        //    //PlaneScriptScript.difficultyScript.difficultyMultiplier = 0;
        //    //PlaneScriptScript.audioScript.tiresSFXPlayed = false;
        //    //PlaneScriptScript.audioScript.landingSpeechPlayed = false;
        //    PlaneScriptScript.flightControllScript.altitudeChangeForceCurrent = gameplaySettings.altitudeChangeForce;
        //}
        //foreach (Transform child in transform)
        //    GameObject.Destroy(child.gameObject);
        ////if(!PlaneScriptScript.audioScript.IsTheSoundCurrentlyPlaying("EngineSound", PlaneScriptScript.audioScript.SFX))
        ////    PlaneScriptScript.audioScript.PlaySound("EngineSound", PlaneScriptScript.audioScript.SFX);
        ////PlaneScriptScript.UIScript.DisablePauseScreen();
        //levelProgress = 0;
        //scorePointsCounter = 0;
        //currentlevelDistance = 100 + levelCounter * 10;
        numberOfObstacles = 1 + flightControllerScript.rewardAndProgressionManagerScript.levelCounter * 2;
        //planeGameObject.transform.position = new Vector3(0, (gameplaySettings.topScreenHeight - gameplaySettings.groundLevelHeight) / 2, 0);
        SpawnObstacles(flightControllerScript.gameModeScript.playerOnePlane);
        SpawnObstacles(flightControllerScript.gameModeScript.playerTwoPlane);
        SpawnAirpot(flightControllerScript.gameModeScript.playerOnePlane);
        SpawnAirpot(flightControllerScript.gameModeScript.playerTwoPlane);
    }
    internal void CalculatePlayerBoundries(Plane plane)
    {
        float cameraH = plane.cameraGameObject.GetComponent<Camera>().orthographicSize;
        plane.topScreenHeight = plane.cameraGameObject.transform.position.y + cameraH;
        plane.groundLevelHeight = plane.cameraGameObject.transform.position.y - cameraH;
    }
    private void SpawnObstacles(Plane plane)
    {
        double sectorWidth = 0.8 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance / numberOfObstacles;
        for (int i = 1; i < numberOfObstacles + 1; i++)
        {
            int obstacle = Random.Range(0, 2);
            if (obstacle == 0) //TREE
            {
                float offsetY = treePrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                GameObject tree = Instantiate(treePrefab, new Vector3((float)((0.1 * flightControllerScript.rewardAndProgressionManagerScript.currentlevelDistance) + (i * sectorWidth)), plane.groundLevelHeight, 0), Quaternion.identity, ObstaclesAndProjectilesGameObject.transform);
                tree.name = "birchTree";
                int treeHeight = Random.Range(0, 3);
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
    private void CheckIfThePlayerIsBehindTheAirport()
    {
        //if (afterAirportDestroyPointGameObject != null && Physics2D.Raycast(afterAirportDestroyPointGameObject.transform.position, Vector2.up, Mathf.Infinity, planeLayer))
            //if (PlaneScriptScript.currentPlaneState == PlaneScript.PlaneState.standard || PlaneScriptScript.currentPlaneState == PlaneScript.PlaneState.wheelsOn)
                //PlaneScriptScript.flightControllScript.DamageThePlane();
    }
}
