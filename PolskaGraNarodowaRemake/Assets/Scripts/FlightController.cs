using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlightController : MonoBehaviour
{
    public GameObject bottleSpawner;
    public GameObject smokeSpawner;
    public GameObject bottlePrefab;
    public GameObject smokePrefab;
    public GameObject explosionPrefab;
    
    internal PlaneBase baseScript;
    [SerializeField] internal float defaultPlaneSpeed;
    [SerializeField] internal float altitudeChangeForce;
    [SerializeField] internal float fallingForce;
    [SerializeField] internal float airportSlowingForce;
    [SerializeField] internal float bottleThrowForceMin;
    [SerializeField] internal float bottleThrowForceMax;
    [SerializeField] internal float bottleThrowForceIncreasmentPerFrame;
    internal float currentPlaneSpeed;
    internal bool isTouchingAirport;
    internal bool isTouchingGround;
    internal bool toNewLevel;
    private float bottleThrowForceCurrent;
    
    void Start()
    {
        baseScript = GetComponent<PlaneBase>();
        isTouchingAirport = false;
        isTouchingGround = false;
        currentPlaneSpeed = defaultPlaneSpeed;
        bottleThrowForceCurrent = bottleThrowForceMin;
    }
    void Update()
    {
        if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard || baseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
        {
            transform.position += new Vector3(currentPlaneSpeed, baseScript.inputScript.position.y * altitudeChangeForce, 0);
            if (isTouchingAirport)
            {
                baseScript.inputScript.position.y = 0;
                currentPlaneSpeed -= airportSlowingForce;
                baseScript.difficultyScript.enableDifficultyImpulses = false;
                if (currentPlaneSpeed <= 0)
                {
                    currentPlaneSpeed = 0;
                    if(!toNewLevel)
                    {
                        toNewLevel = true;
                        baseScript.levelManagerScript.LoadLevel();
                    }
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
            transform.position += new Vector3(currentPlaneSpeed, -fallingForce, 0);
        }
        else if (baseScript.currentPlaneState == PlaneBase.StateMachine.crashed)
        {
            //TODO SOUNDS
            SceneManager.LoadScene("MainMenu");
        }
    }
    internal void ThrowBottleOfVodka()
    {
        if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
        {
            GameObject bottle = Instantiate(bottlePrefab, bottleSpawner.transform.position, Quaternion.identity, transform);
            bottle.GetComponent<Rigidbody2D>().AddForce(new Vector2(1,-1) * bottleThrowForceCurrent);
            baseScript.difficultyScript.difficultyMultiplier++;
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
            Instantiate(smokePrefab, smokeSpawner.transform.position, Quaternion.Euler(270,0,0), smokeSpawner.transform);
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
    }
    internal void DestroyThePlane()
    {
        baseScript.currentPlaneState = PlaneBase.StateMachine.crashed;
        baseScript.planeRendererScript.ChangePlaneSprite();
        baseScript.planeRendererScript.ChangeTilt();
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
    }
}
