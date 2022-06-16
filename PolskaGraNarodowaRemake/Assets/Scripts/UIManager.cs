using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    internal int numberOfBottlesDrunk;
    internal int currentYear;
    internal PlaneBase baseScript;
    public GameObject planeGameObject;
    void Start()
    {
        baseScript = planeGameObject.GetComponent<PlaneBase>();
        numberOfBottlesDrunk = 0;
        currentYear = 2009 + baseScript.flightControllScript.levelCounter;
        UpdateUI();
    }
    internal void UpdateUI()
    {
        //TODO
    }
}
