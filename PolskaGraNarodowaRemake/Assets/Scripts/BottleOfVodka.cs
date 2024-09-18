using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleOfVodka : MonoBehaviour
{
    public GameObject crackedVersionPrefab;
    public GameObject explosionPrefab;
    public GameplaySettings gameplaySettings;
    private GameObject projectileParentGameObject;
    private FlightController flightControllerScript;
    internal Plane parentObject;

    void Start()
    {
        flightControllerScript = GameObject.Find("MasterController").GetComponent<FlightController>();
        projectileParentGameObject = GameObject.Find("ObstaclesAndProjectiles");
    }
    internal void SetParentObject(Plane plane)
    {
        parentObject = plane;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
        {
            flightControllerScript.audioManagerScript.PlaySound("BreakingGlass", flightControllerScript.audioManagerScript.localSFX);
            if (collision.gameObject.transform.name == "verticalObstacle")
            {
                parentObject.gameScore += gameplaySettings.rewardForHittingATarget;
                collision.gameObject.GetComponent<FadeOutTool>().enabled = true;
            }
            else if (collision.gameObject.transform.name == "trotyl" || collision.gameObject.transform.name == "trotylLauncher")
            {
                parentObject.gameScore += gameplaySettings.rewardForHittingATarget;
                GameObject.Destroy(collision.gameObject);
                if (explosionPrefab != null)
                {
                    GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity, projectileParentGameObject.transform);
                    explosion.name = "explosion";
                }
            }
            if (crackedVersionPrefab != null)
            {
                GameObject bottle = Instantiate(crackedVersionPrefab, transform.position, Quaternion.identity, projectileParentGameObject.transform);
                bottle.name = "vodkaBottleCracked";
                foreach (Transform child in bottle.transform)
                    child.gameObject.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * gameplaySettings.debrisSplashForce);
            }
            GameObject.Destroy(gameObject);
        }
    }
}
