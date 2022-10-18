using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetectionManager : MonoBehaviour
{
    //public GameObject planeGameObject;
    public GameObject planeControlPanelGameObject;
    internal PlaneBase planeBaseScript;
    void Start()
    {
        planeBaseScript = planeControlPanelGameObject.GetComponent<PlaneBase>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            planeBaseScript.flightControllScript.isTouchingGround = true;
            planeBaseScript.flightControllScript.DestroyThePlane();
        }
        else if (collision.gameObject.CompareTag("Airport"))
        {
            if (planeBaseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
                planeBaseScript.flightControllScript.isTouchingAirport = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (planeBaseScript.currentPlaneState != PlaneBase.StateMachine.damaged)
                planeBaseScript.flightControllScript.DamageThePlane();
            if (collision.gameObject.GetComponent<DestroyAfterTime>())
                collision.gameObject.GetComponent<DestroyAfterTime>().enabled = true;
        }
    }
}
