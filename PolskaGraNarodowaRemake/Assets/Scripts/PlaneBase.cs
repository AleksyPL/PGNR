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
    public GameObject cameraGameObject;
    public GameObject UIGameObject;
    public GameObject levelManagerGameObject;
    public GameObject audioManagerGameObject;
    public float cameraPositionXOffset;

    [SerializeField] internal StateMachine currentPlaneState;
    internal InputManager inputScript;
    internal AudioManager audioScript;
    internal FlightController flightControllScript;
    internal UIManager UIScript;
    internal PlaneRenderer planeRendererScript;
    internal DifficultyManager difficultyScript;
    internal LevelManager levelManagerScript;
    void OnEnable()
    {
        Application.targetFrameRate = 144;
        audioScript = audioManagerGameObject.GetComponent<AudioManager>();
        UIScript = UIGameObject.GetComponent<UIManager>();
        levelManagerScript = levelManagerGameObject.GetComponent<LevelManager>();
        inputScript = GetComponent<InputManager>();
        flightControllScript = GetComponent<FlightController>();
        planeRendererScript = GetComponent<PlaneRenderer>();
        difficultyScript = GetComponent<DifficultyManager>();
        audioScript.PlaySound("TopGunTheme", audioScript.otherSounds);
    }
    void Update()
    {
        cameraGameObject.transform.position = new Vector3(transform.position.x + cameraPositionXOffset, cameraGameObject.transform.position.y, cameraGameObject.transform.position.z);
    }
}
