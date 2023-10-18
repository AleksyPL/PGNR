using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    private GameModeManager gameModeManagerScript;
    public GameplaySettings gameplaySettings;
    public PowerUp currentPowerUpScriptableObject;
    public enum PowerUp1
    {
        Shield,
        GettingWastedXTimesQuicker,
        MultiShot,
        SoberUp,
        FastPlane,
        InvertedSteering
    }
    public PowerUp1 currentPowerUp;
    void Start()
    {
        gameModeManagerScript = GameObject.Find("MasterController").GetComponent<GameModeManager>();
        GetComponent<SpriteRenderer>().sprite = currentPowerUpScriptableObject.currentPowerUpImageBox;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Plane"))
        {
            if(currentPowerUp == PowerUp1.Shield)
            {
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).cameraGameObject.GetComponent<CameraManager>().PlayCameraFocusAnimation();
                if (!gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled)
                {
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled = true;
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).planeRendererScript.ShowShield();
                    gameModeManagerScript.flightControllerScript.uiManagerScript.SpawnPowerUpUIClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject);
                    gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(collision.gameObject, currentPowerUpScriptableObject.powerUpDescription);
                }
                else
                {
                    gameModeManagerScript.flightControllerScript.powerUpManagerScript.ResetDurationForThePowerUp(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpName);
                    gameModeManagerScript.flightControllerScript.uiManagerScript.ResetDurationForTheUIPowerUpClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpName);
                }
            }
            else if (currentPowerUp == PowerUp1.FastPlane)
            {
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).cameraGameObject.GetComponent<CameraManager>().PlayCameraFocusAnimation();
                if (!gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled)
                {
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled = true;
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneSpeed += (float)(1.0* gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneSpeed);
                    gameModeManagerScript.flightControllerScript.uiManagerScript.SpawnPowerUpUIClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject);
                    gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(collision.gameObject, currentPowerUpScriptableObject.powerUpDescription);
                }
                else
                {
                    gameModeManagerScript.flightControllerScript.powerUpManagerScript.ResetDurationForThePowerUp(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpName);
                    gameModeManagerScript.flightControllerScript.uiManagerScript.ResetDurationForTheUIPowerUpClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpName);
                }
            }
            Destroy(transform.gameObject);
        }    
    }
}
