using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetectionManager : MonoBehaviour
{
    //public GameObject planeControlPanelGameObject;
    //internal PlaneScript PlaneScriptScript;
    void Start()
    {
        //PlaneScriptScript = GetComponent<PlaneScript>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    PlaneScriptScript.flightControllScript.isTouchingGround = true;
        //    PlaneScriptScript.DestroyThePlane();
        //}
        //else if (collision.gameObject.CompareTag("Airport"))
        //{
        //    if (PlaneScriptScript.currentPlaneState == PlaneScript.PlaneState.wheelsOn)
        //        PlaneScriptScript.flightControllScript.isTouchingAirport = true;
        //}
        //else if (collision.gameObject.CompareTag("Obstacle"))
        //{
        //    if (collision.gameObject.GetComponent<FadeOutTool>())
        //        collision.gameObject.GetComponent<FadeOutTool>().enabled = true;
        //    if (PlaneScriptScript.currentPlaneState != PlaneScript.PlaneState.damaged)
        //        PlaneScriptScript.DamageThePlane();
        //}
    }
}
