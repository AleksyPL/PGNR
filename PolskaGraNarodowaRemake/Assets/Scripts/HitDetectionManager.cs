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
            if(transform.CompareTag("Airport"))
            {
                if(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState == PlaneState.wheelsOn)
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).isTouchingAirport = true;
            }
            else if(transform.CompareTag("Obstacle"))
            {
                if(transform.name == "verticalObstacle" || transform.name == "trotylLauncher")
                {
                    if(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).godMode == false && !gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled)
                    {
                        if (transform.GetComponent<FadeOutTool>())
                            transform.gameObject.GetComponent<FadeOutTool>().enabled = true;
                        if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState != PlaneState.damaged)
                            gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DamageThePlane();
                        if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled)
                            gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled = false;
                    }
                    else if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).godMode == false && gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled)
                    {
                        if (transform.GetComponent<FadeOutTool>())
                            transform.gameObject.GetComponent<FadeOutTool>().enabled = true;
                        else
                            Destroy(transform.gameObject);
                        gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).TurnOffTheShield();
                        if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled)
                            gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled = false;
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
            else if(transform.CompareTag("Ground"))
            {
                if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).godMode == false || gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState == PlaneState.damaged)
                {
                    if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState!= PlaneState.crashed)
                        gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DestroyThePlane();
                    if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled)
                        gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled = false;
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).TurnOffTheShield();
                }
            }
            else if(transform.CompareTag("KillPlane"))
            {
                if (transform.GetComponent<FadeOutTool>())
                    transform.gameObject.GetComponent<FadeOutTool>().enabled = true;
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DamageThePlane();
                if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled)
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled = false;
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).TurnOffTheShield();
            }
            else if(transform.name == "SpawnNewObjects" && (gameModeManagerScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless || gameModeManagerScript.currentGameMode == GameModeManager.GameMode.versusEndless))
            {
                if (gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject == null)
                {
                    gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject = new GameObject("Obstacles Buffer");
                    gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject.transform.position = new Vector3(0, 0, 0);
                }
                //ENDLESS MODE IMPORTANT
                foreach (Transform child in gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject.transform)
                {
                    if (child != transform && child.transform.name == "SpawnNewObjects")
                        Destroy(child.gameObject);
                }
                gameModeManagerScript.flightControllerScript.levelManagerScript.FillTheLevelWithObstacles(gameModeManagerScript.playerOnePlane, ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject, ((int)(transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 1) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance, ((int)(transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 2) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnPowerUps(gameModeManagerScript.playerOnePlane, gameModeManagerScript.flightControllerScript.levelManagerScript.powerUpsParentGameObject, (((int)transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 1) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance, ((int)(transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 2) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                if (gameModeManagerScript.currentGameMode == GameModeManager.GameMode.versusEndless)
                {
                    if(!gameModeManagerScript.flightControllerScript.levelManagerScript.sameObstaclesForBothPlayes)
                        gameModeManagerScript.flightControllerScript.levelManagerScript.FillTheLevelWithObstacles(gameModeManagerScript.playerOnePlane, ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject, ((int)(transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 1) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance, ((int)(transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 2) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                    else
                        gameModeManagerScript.flightControllerScript.levelManagerScript.CopyObstaclesFromOnePlayerToAnother(ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject, ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject);
                    gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnPowerUps(gameModeManagerScript.playerTwoPlane, gameModeManagerScript.flightControllerScript.levelManagerScript.powerUpsParentGameObject, (((int)transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 1) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance, ((int)(transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 2) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                    if(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject) == gameModeManagerScript.playerOnePlane)
                    {
                        gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnEndlessModeSpawner(gameModeManagerScript.playerOnePlane, gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, transform.position.x + gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                        gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnEndlessModeSpawner(gameModeManagerScript.playerTwoPlane, gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, transform.position.x + gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance + gameModeManagerScript.flightControllerScript.levelManagerScript.endlessModeSpawnerPlayerTwoOffset);
                    }
                    else if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject) == gameModeManagerScript.playerTwoPlane)
                    {
                        gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnEndlessModeSpawner(gameModeManagerScript.playerOnePlane, gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, transform.position.x + gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance - gameModeManagerScript.flightControllerScript.levelManagerScript.endlessModeSpawnerPlayerTwoOffset);
                        gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnEndlessModeSpawner(gameModeManagerScript.playerTwoPlane, gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, transform.position.x + gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                    }
                }
                else if (gameModeManagerScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless)
                {
                    gameModeManagerScript.flightControllerScript.levelManagerScript.MoveObstaclesFromOneObjectToAnother(ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject, ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject);
                    gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnEndlessModeSpawner(gameModeManagerScript.playerOnePlane, gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject, transform.position.x + gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                }
                Destroy(transform.gameObject);
            }
        }
        else if((collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("PowerUp")) && transform.gameObject.CompareTag("KillPlane"))
            Destroy(collision.transform.gameObject);
    }
}
