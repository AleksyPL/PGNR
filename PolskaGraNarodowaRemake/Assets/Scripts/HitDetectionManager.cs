using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetectionManager : MonoBehaviour
{
    private GameModeManager gameModeManagerScript;
    void Start()
    {
        gameModeManagerScript = GameObject.Find("MasterController").GetComponent<GameModeManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Plane"))
        {
            if(transform.tag == "Airport")
            {
                if(gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState == PlaneState.wheelsOn)
                    gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).isTouchingAirport = true;
            }
            else if(transform.tag == "Obstacle")
            {
                if(transform.name == "birchTree")
                {
                    if(transform.GetComponent<FadeOutTool>())
                        transform.gameObject.GetComponent<FadeOutTool>().enabled = true;
                    if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState != PlaneState.damaged)
                        gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DamageThePlane();
                }
            }
            else if(transform.tag == "Ground")
            {
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DestroyThePlane();
            }
            else if(transform.tag == "KillPlane")
            {
                if (transform.GetComponent<FadeOutTool>())
                    transform.gameObject.GetComponent<FadeOutTool>().enabled = true;
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DamageThePlane();
            }
        }
        else if(collision.gameObject.CompareTag("Obstacle") && transform.tag == "KillPlane")
        {
            Destroy(collision.transform.gameObject);
        }
    }
}
