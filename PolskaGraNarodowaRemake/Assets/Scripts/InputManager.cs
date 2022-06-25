using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    internal Vector3 position;
    internal bool spaceHold;
    internal bool spaceReleased;
    internal PlaneBase baseScript;
    internal bool ESCpressed;
    void Start()
    {
        baseScript = GetComponent<PlaneBase>();
        position = Vector3.zero;
        spaceHold = false;
        ESCpressed = false;
    }
    void Update()
    {
        if(!baseScript.flightControllScript.isTouchingAirport && baseScript.currentPlaneState != PlaneBase.StateMachine.damaged && baseScript.currentPlaneState != PlaneBase.StateMachine.crashed)
        {
            position.y = Input.GetAxisRaw("Vertical");
            position.Normalize();
            spaceHold = Input.GetButton("Jump");
            spaceReleased = Input.GetButtonUp("Jump");
        }
        if(baseScript.currentPlaneState != PlaneBase.StateMachine.crashed)
            ESCpressed = Input.GetButtonDown("Cancel");
    }
}
