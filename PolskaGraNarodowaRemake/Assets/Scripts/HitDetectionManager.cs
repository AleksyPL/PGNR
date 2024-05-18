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
                if (gameModeManagerScript.currentGameMode == GameModeManager.GameMode.tutorial)
                {
                    gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointFailedTryAgain = true;
                    OperateTutorialCheckpoints();
                }
                else
                {
                    if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).godMode == false || gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState == PlaneState.damaged)
                    {
                        if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState != PlaneState.crashed)
                            gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DestroyThePlane();
                        if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled)
                            gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled = false;
                        gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).TurnOffTheShield();
                    }
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
                gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnPowerUps(gameModeManagerScript.playerOnePlane, ref gameModeManagerScript.flightControllerScript.levelManagerScript.powerUpsParentGameObject, (((int)transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 1) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance, ((int)(transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 2) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                if (gameModeManagerScript.currentGameMode == GameModeManager.GameMode.versusEndless)
                {
                    if(!gameModeManagerScript.flightControllerScript.levelManagerScript.sameObstaclesForBothPlayes)
                        gameModeManagerScript.flightControllerScript.levelManagerScript.FillTheLevelWithObstacles(gameModeManagerScript.playerOnePlane, ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject, ((int)(transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 1) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance, ((int)(transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 2) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
                    else
                        gameModeManagerScript.flightControllerScript.levelManagerScript.CopyObstaclesFromOnePlayerToAnother(ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesBufferGameObject, ref gameModeManagerScript.flightControllerScript.levelManagerScript.obstaclesAndProjectilesParentGameObject);
                    gameModeManagerScript.flightControllerScript.levelManagerScript.SpawnPowerUps(gameModeManagerScript.playerTwoPlane, ref gameModeManagerScript.flightControllerScript.levelManagerScript.powerUpsParentGameObject, (((int)transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 1) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance, ((int)(transform.position.x / gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance) + 2) * gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.currentLevelDistance);
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
            else if (transform.name.Contains("TutorialCheckpoint") && gameModeManagerScript.currentGameMode == GameModeManager.GameMode.tutorial)
            {
                OperateTutorialCheckpoints();   
            }
        }
        else if((collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("PowerUp")) && transform.gameObject.CompareTag("KillPlane"))
            Destroy(collision.transform.gameObject);
    }
    private void OperateTutorialCheckpoints()
    {
        gameModeManagerScript.flightControllerScript.tutorialManagerScript.scoreJustBeforeTheRewind = gameModeManagerScript.playerOnePlane.gameScore;
        gameModeManagerScript.flightControllerScript.tutorialManagerScript.bottlesJustBeforeTheRewind = gameModeManagerScript.playerOnePlane.bottlesDrunk;
        gameModeManagerScript.flightControllerScript.rewardAndProgressionManagerScript.playerOneProgress.scorePointsCounter = 0;
        //setting checkpoints facts
        if (!gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointFailedTryAgain)
        {
            if (gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointNumber == 0)
                gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointGoalAchieved = true;
            else if (gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointNumber == 1)
            {
                gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointGoalAchieved = true;
                gameModeManagerScript.playerOnePlane.canThrowBottles = true;
            }
            else if (gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointNumber == 2)
            {
                if (gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpoint2Tree1 == null && gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpoint2Tree2 == null)
                    gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointGoalAchieved = true;
            }
            else if (gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointNumber == 3)
            {
                gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointGoalAchieved = true;
                gameModeManagerScript.playerOnePlane.canThrowBottles = false;
                gameModeManagerScript.playerOnePlane.SoberUp();
                gameModeManagerScript.flightControllerScript.uiManagerScript.UpdateBottlesCounter(gameModeManagerScript.playerOnePlane);
            }
            else if (gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointNumber == 4)
            {
                if (gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpoint4PowerUp == null)
                {
                    gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointGoalAchieved = true;
                    gameModeManagerScript.playerOnePlane.currentPlaneState = PlaneState.wheelsOn;
                    gameModeManagerScript.playerOnePlane.planeRendererScript.ChangePlaneSprite(PlaneState.wheelsOn);
                }
            }
            if (gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointGoalAchieved)
            {
                Destroy(this.transform.gameObject);
                gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointNumber++;
                gameModeManagerScript.flightControllerScript.tutorialManagerScript.scoreAtTheBeginningOfTheCheckpoint = gameModeManagerScript.playerOnePlane.gameScore;
                gameModeManagerScript.flightControllerScript.tutorialManagerScript.bottlesAtTheBeginningOfTheCheckpoint = gameModeManagerScript.playerOnePlane.bottlesDrunk;
                gameModeManagerScript.flightControllerScript.tutorialManagerScript.ClearRewindData(true);
                gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointGoalAchieved = false;
            }
            else
                gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointFailedTryAgain = true;
        }
        if(gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointFailedTryAgain)
        {
            if(gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointNumber == 1)
            {
                if (gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpoint1Launcher != null)
                    gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpoint1Launcher.GetComponent<TrotylLauncher>().canShoot = false;
            }
            else if (gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpointNumber == 3)
            {
                if (gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpoint3Launcher != null)
                    gameModeManagerScript.flightControllerScript.tutorialManagerScript.checkpoint3Launcher.GetComponent<TrotylLauncher>().canShoot = false;
            }
        }
        gameModeManagerScript.flightControllerScript.uiManagerScript.EnableTutorialScreen();
    }
}
