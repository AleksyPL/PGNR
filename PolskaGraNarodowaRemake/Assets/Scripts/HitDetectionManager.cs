using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetectionManager : MonoBehaviour
{
    private GameModeManager gameModeManagerScript;
    void Start()
    {
        gameModeManagerScript = GameObject.Find("MasterController").GetComponent<GameModeManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Plane"))
        {
            if(transform.tag == "Airport")
            {
                if(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState == PlaneState.wheelsOn)
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).isTouchingAirport = true;
            }
            else if(transform.tag == "Obstacle")
            {
                if(transform.name == "birchTree" || transform.name == "trotylLauncher")
                {
                    if(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).godMode == false && !gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled)
                    {
                        if (transform.GetComponent<FadeOutTool>())
                            transform.gameObject.GetComponent<FadeOutTool>().enabled = true;
                        if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState != PlaneState.damaged)
                            gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DamageThePlane();
                    }
                    else if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).godMode == false && gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled)
                    {
                        if (transform.GetComponent<FadeOutTool>())
                            transform.gameObject.GetComponent<FadeOutTool>().enabled = true;
                        else
                            Destroy(transform.gameObject);
                        gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled = false;
                        gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).planeRendererScript.HideShield();
                    }
                    else
                    {
                        if (transform.GetComponent<FadeOutTool>())
                            transform.gameObject.GetComponent<FadeOutTool>().enabled = true;
                        else
                            Destroy(transform.gameObject);
                    }
                }
            }
            else if(transform.tag == "Ground")
            {
                if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).godMode == false || gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState == PlaneState.damaged)
                {
                    if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState!= PlaneState.crashed)
                        gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DestroyThePlane();
                }
            }
            else if(transform.tag == "KillPlane")
            {
                if (transform.GetComponent<FadeOutTool>())
                    transform.gameObject.GetComponent<FadeOutTool>().enabled = true;
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DamageThePlane();
            }
            else if(transform.name == "SpawnNewObjects" && (gameModeManagerScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || gameModeManagerScript.currentGameMode == GameModeManager.GameMode.versusEndless))
            {
                //ENDLESS MODE IMPORTANT
                gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.levelSafeSpace = 0;
                foreach (Transform child in gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject.transform)
                {
                    if (child != transform && child.transform.name == "SpawnNewObjects")
                        Destroy(child.gameObject);
                }
                gameModeManagerScript.flightControllerScript.levelManagerScript.obstacleSectorWidth = gameModeManagerScript.flightControllerScript.levelManagerScript.numberOfObstacles / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance;
                gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnObstacles(gameModeManagerScript.playerOnePlane, ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject,(float)(gameModeManagerScript.flightControllerScript.levelManagerScript.obstacleSectorWidth / 2 + gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance));
                gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnPowerUps(gameModeManagerScript.playerOnePlane, ref gameModeManagerScript.flightControllerScript.levelManagerScript.powerUpsParentGameObject, gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter + (2 * Camera.main.orthographicSize * Camera.main.aspect), gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerOneCounter + (2 * Camera.main.orthographicSize * Camera.main.aspect) + gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                if (gameModeManagerScript.currentGameMode == GameModeManager.GameMode.versusEndless)
                {
                    gameModeManagerScript.flightControllerScript.levelManagerScript.CopyObstaclesFromOnePlayerToAnother(ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject, ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject);
                    gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnPowerUps(gameModeManagerScript.playerTwoPlane, ref gameModeManagerScript.flightControllerScript.levelManagerScript.powerUpsParentGameObject, gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter + (2 * Camera.main.orthographicSize * Camera.main.aspect), gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.levelProgressPlayerTwoCounter + (2 * Camera.main.orthographicSize * Camera.main.aspect) + gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                }
                else if (gameModeManagerScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless)
                    gameModeManagerScript.flightControllerScript.levelManagerScript.MoveObstaclesFromOneObjectToAnother(ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject, ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject);
                Destroy(transform.gameObject);
            }
        }
        else if((collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("PowerUp")) && transform.tag == "KillPlane")
        {
            Destroy(collision.transform.gameObject);
        }
    }
}
