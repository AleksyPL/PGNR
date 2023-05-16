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
                    if(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).godMode == false)
                    {
                        if (transform.GetComponent<FadeOutTool>())
                            transform.gameObject.GetComponent<FadeOutTool>().enabled = true;
                        if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState != PlaneState.damaged)
                            gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DamageThePlane();
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
                gameModeManagerScript.flightController.rewardAndProgressionManagerScript.levelSafeSpace = 0;
                foreach (Transform child in gameModeManagerScript.flightController.levelManagerScript.obstaclesAndProjectilesGameObject.transform)
                {
                    if (child != transform && child.transform.name == "SpawnNewObjects")
                        Destroy(child.gameObject);
                }
                gameModeManagerScript.flightController.levelManagerScript.obstacleSectorWidth = gameModeManagerScript.flightController.levelManagerScript.numberOfObstacles / gameModeManagerScript.flightController.rewardAndProgressionManagerScript.currentlevelDistance;
                gameModeManagerScript.flightController.levelManagerScript.SpawnObstacles(gameModeManagerScript.playerOnePlane, ref gameModeManagerScript.flightController.levelManagerScript.obstaclesBufferGameObject,(float)(gameModeManagerScript.flightController.levelManagerScript.obstacleSectorWidth / 2 + gameModeManagerScript.flightController.rewardAndProgressionManagerScript.currentlevelDistance));
                if (gameModeManagerScript.currentGameMode == GameModeManager.GameMode.versusEndless)
                    gameModeManagerScript.flightController.levelManagerScript.CopyObstaclesFromOnePlayerToAnother(ref gameModeManagerScript.flightController.levelManagerScript.obstaclesBufferGameObject, ref gameModeManagerScript.flightController.levelManagerScript.obstaclesAndProjectilesGameObject);
                else if (gameModeManagerScript.currentGameMode == GameModeManager.GameMode.singleplayerEndless)
                    gameModeManagerScript.flightController.levelManagerScript.MoveObstaclesFromOneObjectToAnother(ref gameModeManagerScript.flightController.levelManagerScript.obstaclesBufferGameObject, ref gameModeManagerScript.flightController.levelManagerScript.obstaclesAndProjectilesGameObject);
                Destroy(transform.gameObject);
            }
        }
        else if(collision.gameObject.CompareTag("Obstacle") && transform.tag == "KillPlane")
        {
            Destroy(collision.transform.gameObject);
        }
    }
}
