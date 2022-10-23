using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetectionManager : MonoBehaviour
{
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
            if (collision.gameObject.GetComponent<FadeOutTool>())
                collision.gameObject.GetComponent<FadeOutTool>().enabled = true;
            if (planeBaseScript.currentPlaneState != PlaneBase.StateMachine.damaged)
                planeBaseScript.flightControllScript.DamageThePlane();
        }
    }
}
