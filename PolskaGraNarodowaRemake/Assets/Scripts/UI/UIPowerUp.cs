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
    //internal bool fakeClock;
    void Start()
    {
        
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
                uiManagerScript.ChangeTheOrderOnThePowerUpsBar(uiManagerScript.playerOneUI.powerUpBarParentGameObject);
                if(uiManagerScript.flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
                    uiManagerScript.ChangeTheOrderOnThePowerUpsBar(uiManagerScript.playerTwoUI.powerUpBarParentGameObject);
            }
        }
    }
    internal void EnableUIPowerUp(float thisPowerUpDuration, string thisPowerUpName, bool isFake = false, float fakeClockFillAmount = 0.75f)
    {
        clockImageGameObject = transform.Find("Clock").GetComponent<Image>();
        powerUpDuration = thisPowerUpDuration;
        powerUpDurationCounter = powerUpDuration;
        powerUpName = thisPowerUpName;
        if (!isFake)
            powerUpEnabled = true;
        else
        {
            fakeClockFillAmount = Mathf.Clamp(fakeClockFillAmount, 0, 1);
            clockImageGameObject.fillAmount = fakeClockFillAmount;
        }
    }
}
