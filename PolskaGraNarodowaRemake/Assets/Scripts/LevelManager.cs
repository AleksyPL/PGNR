using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject airportPrefab;
    public GameObject treePrefab;
    public GameObject trotylLauncherPrefab;
    public GameObject fogPrefab;
    public GameObject planeGameObject;
    public LayerMask planeLayer;
    public float groundLevelHeight;
    public float topScreenHeight;
    internal PlaneBase planeBaseScript;
    internal int levelCounter;
    internal float currentlevelDistance;
    internal float levelProgress;
    internal int gameScore;
    private GameObject afterAirportDestroyPointGameObject;
    private float scorePointsCounter;
    private int numberOfObstacles;

    void Start()
    {
        levelCounter = 1;
        planeBaseScript = planeGameObject.GetComponent<PlaneBase>();
        gameScore = 0;
        LoadLevel();
    }
    void Update()
    {
        if (planeBaseScript.flightControllScript.currentPlaneSpeed > 0 && (planeBaseScript.currentPlaneState == PlaneBase.StateMachine.standard || planeBaseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn))
        {
            scorePointsCounter += Time.deltaTime;
            if (scorePointsCounter > 1)
            {
                scorePointsCounter = 0;
                gameScore++;
            }
            if (planeBaseScript.currentPlaneState == PlaneBase.StateMachine.standard)
            {
                if (levelProgress < currentlevelDistance)
                    levelProgress += planeBaseScript.flightControllScript.currentPlaneSpeed * Time.deltaTime;
                if (levelProgress >= currentlevelDistance)
                {
                    levelProgress = currentlevelDistance;
                    planeBaseScript.currentPlaneState = PlaneBase.StateMachine.wheelsOn;
                    planeBaseScript.planeRendererScript.ChangePlaneSprite();
                }
            }
            else if(planeBaseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
            {
                if (levelProgress >= currentlevelDistance)
                    CheckIfThePlayerIsBehindTheAirport();
            }
        }
    }
    internal void LoadLevel()
    {
        if(planeBaseScript.flightControllScript.toNewLevel)
        {
            planeBaseScript.flightControllScript.toNewLevel = false;
            levelCounter++;
            planeBaseScript.flightControllScript.currentPlaneSpeed = planeBaseScript.flightControllScript.defaultPlaneSpeed;
            planeBaseScript.flightControllScript.isTouchingAirport = false;
            planeBaseScript.flightControllScript.isTouchingGround = false;
            planeBaseScript.flightControllScript.waitingTimeAfterLandingCombinedWithSoundLength = 3f;
            planeBaseScript.flightControllScript.rewardForLandingAdded = false;
            planeBaseScript.difficultyScript.difficultyMultiplier = 0;
            planeBaseScript.audioScript.tiresSFXPlayed = false;
            planeBaseScript.audioScript.landingSpeechPlayed = false;
            planeBaseScript.flightControllScript.altitudeChangeForceCurrent = planeBaseScript.flightControllScript.altitudeChangeForce;
            foreach (Transform child in transform)
                GameObject.Destroy(child.gameObject);
        }
        planeBaseScript.audioScript.PlaySound("EngineSound", planeBaseScript.audioScript.SFX);
        levelProgress = 0;
        scorePointsCounter = 0;
        currentlevelDistance = 100 + levelCounter * 10;
        numberOfObstacles = 1 + levelCounter * 2;
        planeGameObject.transform.position = new Vector3(0, (topScreenHeight - groundLevelHeight) / 2, 0);
        planeBaseScript.currentPlaneState = PlaneBase.StateMachine.standard;
        planeBaseScript.planeRendererScript.ChangePlaneSprite();
        SpawnObstacles();
        SpawnAirpot();
    }
    private void SpawnObstacles()
    {
        double sectorWidth = 0.8 * currentlevelDistance / numberOfObstacles;
        for (int i = 1; i < numberOfObstacles + 1; i++)
        {
            int obstacle = Random.Range(0, 2);
            if (obstacle == 0) //TREE
            {
                float offsetY = treePrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                GameObject tree = Instantiate(treePrefab, new Vector3((float)((0.1 * currentlevelDistance) + (i * sectorWidth)), groundLevelHeight, 0), Quaternion.identity, transform);
                tree.gameObject.name = "birchTree";
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
                GameObject trotylLauncher = Instantiate(trotylLauncherPrefab, new Vector3((float)((0.1 * currentlevelDistance) + (i * sectorWidth)), groundLevelHeight, 0), Quaternion.identity, transform);
                trotylLauncher.gameObject.name = "trotylLauncher";
            }
        }
        int numberOfFogInstances = numberOfObstacles / 4;
        for(int i= 0; i < numberOfFogInstances; i++)
        {
            float fogPlacementX = Random.Range((float)0.1 * currentlevelDistance, (float)0.9 * currentlevelDistance);
            GameObject fog = Instantiate(fogPrefab, new Vector3(fogPlacementX, (topScreenHeight - groundLevelHeight) / 2, 0), Quaternion.identity, transform);
            fog.gameObject.name = "fog";
        }
    }
    private void SpawnAirpot()
    {
        GameObject airport = Instantiate(airportPrefab, new Vector3((float)1.25 * currentlevelDistance, groundLevelHeight, 0), Quaternion.identity, transform);
        afterAirportDestroyPointGameObject = airport.transform.Find("DestroyPlanePoint").gameObject;
    }
    private void CheckIfThePlayerIsBehindTheAirport()
    {
        if (afterAirportDestroyPointGameObject != null && Physics2D.Raycast(afterAirportDestroyPointGameObject.transform.position, Vector2.up, Mathf.Infinity, planeLayer))
        {
            if (planeBaseScript.currentPlaneState == PlaneBase.StateMachine.standard || planeBaseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
            {
                planeBaseScript.flightControllScript.DamageThePlane();
            }
        }
    }
}
