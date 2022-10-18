using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBase : MonoBehaviour
{
    internal enum StateMachine
    {
        standard,
        wheelsOn,
        damaged,
        crashed
    }
    
    public GameObject inputManagerGameObject;
    public GameObject UIGameObject;
    public GameObject levelManagerGameObject;
    public GameObject audioManagerGameObject;
    public GameObject flighControllerGameObject;
    
    [SerializeField] internal StateMachine currentPlaneState;
    internal InputManager inputScript;
    internal AudioManager audioScript;
    internal DifficultyManager difficultyScript;
    internal LevelManager levelManagerScript;
    internal FlightController flightControllScript;
    internal UIManager UIScript;
    internal PlaneRenderer planeRendererScript;
    void OnEnable()
    {
        Application.targetFrameRate = 144;
        flightControllScript = flighControllerGameObject.GetComponent<FlightController>();
        planeRendererScript = GetComponent<PlaneRenderer>();
        difficultyScript = GetComponent<DifficultyManager>();
        levelManagerScript = levelManagerGameObject.GetComponent<LevelManager>();
        audioScript = audioManagerGameObject.GetComponent<AudioManager>();
        UIScript = UIGameObject.GetComponent<UIManager>();
        inputScript = inputManagerGameObject.GetComponent<InputManager>();
    }
}
