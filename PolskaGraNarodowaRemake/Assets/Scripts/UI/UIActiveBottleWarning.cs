using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIActiveBottleWarning : MonoBehaviour
{
    internal FlightController flightControllerScript;
    private TMP_Text activeBottleWarningPlayerOneGameObject;
    private TMP_Text activeBottleWarningPlayerTwoGameObject;
    private float bottleWarningTimeCounter;
    void Start()
    {
        flightControllerScript = GetComponentInParent<FlightController>();
        activeBottleWarningPlayerOneGameObject = transform.Find("WaringPlayerUp").GetComponent<TMP_Text>();
        activeBottleWarningPlayerTwoGameObject = transform.Find("WaringPlayerDown").GetComponent<TMP_Text>();
        TurnOnBottleWarning();
    }
    void Update()
    {
        UpdateBottleWarning();
    }
    internal void TurnOnBottleWarning()
    {
        if (flightControllerScript.gameModeScript.playerOnePlane.activeBottleWarning)
        {
            activeBottleWarningPlayerOneGameObject.gameObject.SetActive(true);
            activeBottleWarningPlayerOneGameObject.text = flightControllerScript.gameplaySettings.localizationsStrings[flightControllerScript.gameplaySettings.langauageIndex].activeBottleWarning;
        }
        if (flightControllerScript.gameModeScript.playerTwoPlane.activeBottleWarning)
        {
            activeBottleWarningPlayerTwoGameObject.gameObject.SetActive(true);
            activeBottleWarningPlayerTwoGameObject.text = flightControllerScript.gameplaySettings.localizationsStrings[flightControllerScript.gameplaySettings.langauageIndex].activeBottleWarning;
        }
    }
    internal void UpdateBottleWarning()
    {
        bottleWarningTimeCounter += Time.unscaledDeltaTime;
        if (bottleWarningTimeCounter > 0.5f)
        {
            bottleWarningTimeCounter = 0;
            if (flightControllerScript.gameModeScript.playerOnePlane.activeBottleWarning)
                activeBottleWarningPlayerOneGameObject.gameObject.SetActive(!activeBottleWarningPlayerOneGameObject.gameObject.activeSelf);
            if (flightControllerScript.gameModeScript.playerTwoPlane.activeBottleWarning)
                activeBottleWarningPlayerTwoGameObject.gameObject.SetActive(!activeBottleWarningPlayerTwoGameObject.gameObject.activeSelf);
        }
    }
    internal void DestroyBottleWarning()
    {
        Destroy(transform.gameObject);
    }
}
