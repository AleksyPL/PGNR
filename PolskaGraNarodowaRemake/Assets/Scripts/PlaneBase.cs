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
    public float cameraPositionXOffset;

    internal StateMachine currentPlaneState;
    internal InputManager inputScript;
    internal AudioManager audioScript;
    internal FlightController flightControllScript;
    internal UIManager UIScript;
    internal PlaneRenderer planeRendererScript;
    internal DifficultyManager difficultyScript;
    void OnEnable()
    {

        //audioScript = GetComponent<AudioManager>();
        UIScript = UIGameObject.GetComponent<UIManager>();
        inputScript = GetComponent<InputManager>();
        flightControllScript = GetComponent<FlightController>();
        planeRendererScript = GetComponent<PlaneRenderer>();
        difficultyScript = GetComponent<DifficultyManager>();
        currentPlaneState = StateMachine.wheelsOn;
    }
    void Update()
    {
        cameraGameObject.transform.position = new Vector3(transform.position.x + cameraPositionXOffset, cameraGameObject.transform.position.y, cameraGameObject.transform.position.z);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            currentPlaneState = StateMachine.crashed;
            flightControllScript.isTouchingGround = true;
            planeRendererScript.ChangePlaneSprite();
        }
        else if(collision.gameObject.CompareTag("Airport"))
        {
            if(currentPlaneState == StateMachine.wheelsOn)
                flightControllScript.isTouchingAirport = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Projectile"))
        {
            flightControllScript.DamageThePlane();
        }
    }
}
