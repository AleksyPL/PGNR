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
        audioScript = GetComponent<AudioManager>();
        UIScript = UIGameObject.GetComponent<UIManager>();
        inputScript = GetComponent<InputManager>();
        flightControllScript = GetComponent<FlightController>();
        planeRendererScript = GetComponent<PlaneRenderer>();
        difficultyScript = GetComponent<DifficultyManager>();
        levelManagerScript = levelManagerGameObject.GetComponent<LevelManager>();
    }
    void Update()
    {
        cameraGameObject.transform.position = new Vector3(transform.position.x + cameraPositionXOffset, cameraGameObject.transform.position.y, cameraGameObject.transform.position.z);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            flightControllScript.isTouchingGround = true;
            flightControllScript.DestroyThePlane();
        }
        else if(collision.gameObject.CompareTag("Airport"))
        {
            if(currentPlaneState == StateMachine.wheelsOn)
                flightControllScript.isTouchingAirport = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            if(currentPlaneState != StateMachine.damaged)
                flightControllScript.DamageThePlane();
            if(collision.gameObject.GetComponent<DestroyAfterTime>())
                collision.gameObject.GetComponent<DestroyAfterTime>().enabled = true;
        }
    }
}
