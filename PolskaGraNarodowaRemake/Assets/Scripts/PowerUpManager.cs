using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    internal FlightController flightControllerScript;
    //shield
    private float shieldPlayerOneCounter;
    private float shieldPlayerTwoCounter;


    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
    }

    void Update()
    {
        CheckPlayersShield(ref flightControllerScript.gameModeScript.playerOnePlane, ref shieldPlayerOneCounter);
        CheckPlayersShield(ref flightControllerScript.gameModeScript.playerTwoPlane, ref shieldPlayerTwoCounter);
    }
    private void SetAllValuesForPowerUps()
    {

    }
    private void CheckPlayersShield(ref Plane plane, ref float shieldCounter)
    {
        if (plane.shieldEnabled)
        {
            shieldCounter -= Time.deltaTime;
            if (shieldPlayerOneCounter <= 0)
            {
                shieldCounter = 0;
                plane.shieldEnabled = false;
            }
        }
    }
}
