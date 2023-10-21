using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerUp : MonoBehaviour
{
    private UIManager uiManagerScript;
    internal float powerUpDurationCounter;
    internal float powerUpDuration;
    internal string powerUpName;
    private Image clockImageGameObject;
    private bool powerUpEnabled;
    void Start()
    {
        clockImageGameObject = transform.Find("Clock").GetComponent<Image>();
        uiManagerScript = GameObject.Find("MasterController").GetComponent<UIManager>();
    }
    void Update()
    {
        if(powerUpEnabled)
        {
            powerUpDurationCounter -= Time.deltaTime;
            clockImageGameObject.fillAmount = Mathf.InverseLerp(0, 1, powerUpDurationCounter / powerUpDuration);
            if (powerUpDurationCounter <= 0)
            {
                uiManagerScript.ChangeTheOrderOnThePowerUpsBar(uiManagerScript.powerUpBarPlayerOneParentGameObject);
                if(uiManagerScript.flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
                    uiManagerScript.ChangeTheOrderOnThePowerUpsBar(uiManagerScript.powerUpBarPlayerTwoParentGameObject);
            }
        }
    }
    internal void EnableUIPowerUp(float thisPowerUpDuration, string thisPowerUpName)
    {
        powerUpDuration = thisPowerUpDuration;
        powerUpDurationCounter = powerUpDuration;
        powerUpName = thisPowerUpName;
        powerUpEnabled = true;
    }
}
