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
        if(collision.gameObject.CompareTag("Plane") && gameModeManagerScript.ReturnPlayerStateObject(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject)) == GameModeManager.PlayerState.flying)
        {
            if(currentPowerUp == PowerUp1.Shield)
            {
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).cameraGameObject.GetComponent<CameraManager>().PlayCameraFocusAnimation();
                if (!gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled)
                {
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled = true;
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).planeRendererScript.ShowShield();
                    gameModeManagerScript.flightControllerScript.uiManagerScript.SpawnPowerUpUIClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject);
                    if(gameplaySettings.safeMode)
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpSafeDescription[gameplaySettings.langauageIndex]);
                    else
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpDescription[gameplaySettings.langauageIndex]);
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
                    if (gameplaySettings.safeMode)
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpSafeDescription[gameplaySettings.langauageIndex]);
                    else
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpDescription[gameplaySettings.langauageIndex]);
                }
                else
                {
                    gameModeManagerScript.flightControllerScript.powerUpManagerScript.ResetDurationForThePowerUp(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpName);
                    gameModeManagerScript.flightControllerScript.uiManagerScript.ResetDurationForTheUIPowerUpClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpName);
                }
            }
            else if (currentPowerUp == PowerUp1.SoberUp)
            {
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).cameraGameObject.GetComponent<CameraManager>().PlayCameraFocusAnimation();
                if(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).bottlesDrunk != 0)
                {
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).SoberUp();
                    if (gameplaySettings.safeMode)
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpSafeDescription[gameplaySettings.langauageIndex]);
                    else
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpDescription[gameplaySettings.langauageIndex]);
                }
                else
                    if (gameplaySettings.safeMode)
                    gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpAlternativeSafeDescription[gameplaySettings.langauageIndex]);
                else
                    gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpAlternativeDescription[gameplaySettings.langauageIndex]);
            }
            else if (currentPowerUp == PowerUp1.MultiShot)
            {
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).cameraGameObject.GetComponent<CameraManager>().PlayCameraFocusAnimation();
                if (!gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).multiShotEnabled)
                {
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).multiShotEnabled = true;
                    gameModeManagerScript.flightControllerScript.uiManagerScript.SpawnPowerUpUIClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject);
                    if (gameplaySettings.safeMode)
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpSafeDescription[gameplaySettings.langauageIndex]);
                    else
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpDescription[gameplaySettings.langauageIndex]);
                }
                else
                {
                    gameModeManagerScript.flightControllerScript.powerUpManagerScript.ResetDurationForThePowerUp(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpName);
                    gameModeManagerScript.flightControllerScript.uiManagerScript.ResetDurationForTheUIPowerUpClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpName);
                }
            }
            else if (currentPowerUp == PowerUp1.InvertedSteering)
            {
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).cameraGameObject.GetComponent<CameraManager>().PlayCameraShakeAnimation();
                if (!gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).invertedSteeringEnabled)
                {
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).invertedSteeringEnabled = true;
                    gameModeManagerScript.flightControllerScript.uiManagerScript.SpawnPowerUpUIClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject);
                    if (gameplaySettings.safeMode)
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpSafeDescription[gameplaySettings.langauageIndex]);
                    else
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpDescription[gameplaySettings.langauageIndex]);
                }
                else if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).invertedSteeringEnabled)
                {
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).invertedSteeringEnabled = false;
                    gameModeManagerScript.flightControllerScript.uiManagerScript.DeletePowerUpUIClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpName);
                    if (gameplaySettings.safeMode)
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpAlternativeSafeDescription[gameplaySettings.langauageIndex]);
                    else
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpAlternativeDescription[gameplaySettings.langauageIndex]);

                }
            }
            else if (currentPowerUp == PowerUp1.GettingWastedXTimesQuicker)
            {
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).cameraGameObject.GetComponent<CameraManager>().PlayCameraShakeAnimation();
                if(!gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).gettingWastedXTimesMoreEnabled)
                {
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).gettingWastedXTimesMoreEnabled = true;
                    gameModeManagerScript.flightControllerScript.uiManagerScript.SpawnPowerUpUIClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject);
                    if (gameplaySettings.safeMode)
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpSafeDescription[gameplaySettings.langauageIndex]);
                    else
                        gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpDescription[gameplaySettings.langauageIndex]);
                }
                else
                {
                    gameModeManagerScript.flightControllerScript.powerUpManagerScript.ResetDurationForThePowerUp(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpName);
                    gameModeManagerScript.flightControllerScript.uiManagerScript.ResetDurationForTheUIPowerUpClock(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject), currentPowerUpScriptableObject.powerUpName);
                }
            }
            Destroy(transform.gameObject);
        }
        else if (collision.gameObject.CompareTag("Plane") && gameModeManagerScript.ReturnPlayerStateObject(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject)) != GameModeManager.PlayerState.flying)
        {
            Destroy(transform.gameObject);
        }
    }
}
