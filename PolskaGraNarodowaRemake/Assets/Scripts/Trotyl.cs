using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trotyl : MonoBehaviour
{
    private GameModeManager gameModeManagerScript;
    private AudioManager audioScript;
    void Start()
    {
        gameModeManagerScript = GameObject.Find("MasterController").GetComponent<GameModeManager>();
        audioScript = GameObject.Find("MasterController").GetComponent<AudioManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (gameModeManagerScript.flightControllerScript.gameplaySettings.explosionPrefab != null)
            {
                Instantiate(gameModeManagerScript.flightControllerScript.gameplaySettings.explosionPrefab, transform.position, Quaternion.identity, gameModeManagerScript.transform.Find("ObstaclesAndProjectiles").transform);
                audioScript.PlaySound("Explosion", audioScript.localSFX);
            }
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Plane"))
        {
            if(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).godMode == false)
            {
                if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled)
                {
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).TurnOffTheShield();
                }
                else
                {
                    if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState != PlaneState.damaged)
                        gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DamageThePlane();
                }
                
            }
            Destroy(gameObject);
        }
    }
}
