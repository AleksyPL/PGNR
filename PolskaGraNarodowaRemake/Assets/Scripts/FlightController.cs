using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    public GameObject bottleSpawner;
    public GameObject smokeSpawner;
    public GameObject bottlePrefab;
    public GameObject smokePrefab;
    public GameObject explosionPrefab;
    internal int levelCounter;
    internal PlaneBase baseScript;
    [SerializeField] internal float planeSpeed;
    [SerializeField] internal float altitudeChangeForce;
    [SerializeField] internal float fallingForce;
    [SerializeField] internal float airportSlowingForce;
    [SerializeField] internal float bottleThrowForceMin;
    [SerializeField] internal float bottleThrowForceMax;
    [SerializeField] internal float bottleThrowForceIncreasmentPerFrame;
    private float bottleThrowForceCurrent;
    internal bool isTouchingAirport;
    internal bool isTouchingGround;
    
    void Start()
    {
        baseScript = GetComponent<PlaneBase>();
        levelCounter = 1;
        isTouchingAirport = false;
        isTouchingGround = false;
        bottleThrowForceCurrent = bottleThrowForceMin;
    }
    void Update()
    {
        if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard || baseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
        {
            transform.position += new Vector3(planeSpeed, baseScript.inputScript.position.y * altitudeChangeForce, 0);
            if (isTouchingAirport)
            {
                baseScript.inputScript.position.y = 0;
                planeSpeed -= airportSlowingForce;
                if (planeSpeed <= 0)
                {
                    planeSpeed = 0;
                    //NEW LEVEL TODO
                }
            }  
            if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
            {
                if (baseScript.inputScript.spaceHold)
                {
                    if(bottleThrowForceCurrent < bottleThrowForceMax)
                    {
                        bottleThrowForceCurrent += bottleThrowForceIncreasmentPerFrame;
                        if (bottleThrowForceCurrent > bottleThrowForceMax)
                            bottleThrowForceCurrent = bottleThrowForceMax;
                    }
                }
                if (baseScript.inputScript.spaceReleased)
                {
                    ThrowBottleOfVodka();
                    bottleThrowForceCurrent = bottleThrowForceMin;
                }
            }
        }
        else if (baseScript.currentPlaneState == PlaneBase.StateMachine.damaged)
        {
            baseScript.difficultyScript.enableDifficultyImpulses = false;
            transform.position += new Vector3(0, -fallingForce, 0);
        }
        else if (baseScript.currentPlaneState == PlaneBase.StateMachine.crashed)
        {
            //RESTART LEVEL TODO
        }
    }
    internal void ThrowBottleOfVodka()
    {
        if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
        {
            GameObject bottle = Instantiate(bottlePrefab, bottleSpawner.transform.position, Quaternion.identity);
            bottle.GetComponent<Rigidbody2D>().AddForce(new Vector2(1,-1) * bottleThrowForceCurrent);
            baseScript.difficultyScript.difficultyMultiplier++;
            baseScript.UIScript.numberOfBottlesDrunk++;
            if (!baseScript.difficultyScript.enableDifficultyImpulses)
                baseScript.difficultyScript.enableDifficultyImpulses = true;
        }
    }
    internal void DamageThePlane()
    {
        baseScript.currentPlaneState = PlaneBase.StateMachine.damaged;
        baseScript.planeRendererScript.ChangePlaneSprite();
        baseScript.planeRendererScript.ChangeTilt();
        if (smokePrefab != null)
            Instantiate(smokePrefab, smokeSpawner.transform.position, Quaternion.identity);
    }
}
