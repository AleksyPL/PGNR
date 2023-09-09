using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    private GameModeManager gameModeManagerScript;
    public GameplaySettings gameplaySettings;
    public PowerUp currentPowerUpScriptableObject;
    public GameObject UiPowerUpPrefab;
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
                if (!gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled)
                {
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).shieldEnabled = true;
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).planeRendererScript.ShowShield();
                    GameObject powerUpUIGameObject = Instantiate(UiPowerUpPrefab);
                    if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject) == gameModeManagerScript.playerOnePlane)
                        powerUpUIGameObject.gameObject.transform.SetParent(gameModeManagerScript.flightControllerScript.uiManagerScript.powerUpBarPlayerOneParentGameObject.transform);
                    else
                        powerUpUIGameObject.gameObject.transform.SetParent(gameModeManagerScript.flightControllerScript.uiManagerScript.powerUpBarPlayerTwoParentGameObject.transform);
                    powerUpUIGameObject.GetComponent<UIPowerUp>().EnableUIPowerUp(currentPowerUpScriptableObject.powerUpDuration, currentPowerUpScriptableObject.powerUpName);
                    gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(collision.gameObject, currentPowerUpScriptableObject.powerUpDescription);

                }
                else
                {
                    if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject) == gameModeManagerScript.playerOnePlane)
                    {
                        gameModeManagerScript.flightControllerScript.powerUpManagerScript.ResetDurationForThePowerUp(currentPowerUpScriptableObject.powerUpName, ref gameModeManagerScript.flightControllerScript.powerUpManagerScript.shieldPlayerOneCounter);
                        for(int i=0;i< gameModeManagerScript.flightControllerScript.uiManagerScript.powerUpBarPlayerOneParentGameObject.gameObject.transform.childCount;i++)
                        {
                            if(gameModeManagerScript.flightControllerScript.uiManagerScript.powerUpBarPlayerOneParentGameObject.gameObject.transform.GetChild(i).name == currentPowerUpScriptableObject.powerUpName)
                                gameModeManagerScript.flightControllerScript.uiManagerScript.powerUpBarPlayerOneParentGameObject.gameObject.transform.GetChild(i).GetComponent<UIPowerUp>().EnableUIPowerUp(currentPowerUpScriptableObject.powerUpDuration, currentPowerUpScriptableObject.powerUpName);
                        }
                    }
                    else
                    {
                        gameModeManagerScript.flightControllerScript.powerUpManagerScript.ResetDurationForThePowerUp(currentPowerUpScriptableObject.powerUpName, ref gameModeManagerScript.flightControllerScript.powerUpManagerScript.shieldPlayerTwoCounter);
                        for (int i = 0; i < gameModeManagerScript.flightControllerScript.uiManagerScript.powerUpBarPlayerTwoParentGameObject.gameObject.transform.childCount; i++)
                        {
                            if (gameModeManagerScript.flightControllerScript.uiManagerScript.powerUpBarPlayerTwoParentGameObject.gameObject.transform.GetChild(i).name == currentPowerUpScriptableObject.powerUpName)
                                gameModeManagerScript.flightControllerScript.uiManagerScript.powerUpBarPlayerTwoParentGameObject.gameObject.transform.GetChild(i).GetComponent<UIPowerUp>().EnableUIPowerUp(currentPowerUpScriptableObject.powerUpDuration, currentPowerUpScriptableObject.powerUpName);
                        }
                    }
                }
            }
            else if (currentPowerUp == PowerUp1.FastPlane)
            {
                if (!gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled)
                {
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).speedEnabled = true;
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneSpeed += (float)(1.0* gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneSpeed);
                    GameObject powerUpUIGameObject = Instantiate(UiPowerUpPrefab);
                    if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject) == gameModeManagerScript.playerOnePlane)
                        powerUpUIGameObject.gameObject.transform.SetParent(gameModeManagerScript.flightControllerScript.uiManagerScript.powerUpBarPlayerOneParentGameObject.transform);
                    else
                        powerUpUIGameObject.gameObject.transform.SetParent(gameModeManagerScript.flightControllerScript.uiManagerScript.powerUpBarPlayerTwoParentGameObject.transform);
                    powerUpUIGameObject.GetComponent<UIPowerUp>().EnableUIPowerUp(currentPowerUpScriptableObject.powerUpDuration, currentPowerUpScriptableObject.powerUpName);
                    gameModeManagerScript.flightControllerScript.uiManagerScript.DisplayPowerUpDescriptionOnHUD(collision.gameObject, currentPowerUpScriptableObject.powerUpDescription);
                }
            }
            Destroy(transform.gameObject);
        }    
    }
}
