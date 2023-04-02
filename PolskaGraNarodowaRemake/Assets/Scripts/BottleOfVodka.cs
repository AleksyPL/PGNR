using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleOfVodka : MonoBehaviour
{
    public GameObject crackedVersionPrefab;
    public GameObject explosionPrefab;
    public GameplaySettings gameplaySettings;
    private GameObject projectileParentGameObject;
    //private GameObject audioManagerGameObject;
    //private AudioManager audioScript;
    //private GameObject levelManagerGameObject;
    //private LevelManager levelManagerScript;

    void Start()
    {
        //audioManagerGameObject = GameObject.Find("AudioManager");
        //audioScript = audioManagerGameObject.GetComponent<AudioManager>();
        projectileParentGameObject = GameObject.Find("ObstaclesAndProjectiles");
        //levelManagerScript = levelManagerGameObject.GetComponent<LevelManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
        {
            //audioScript.PlaySound("BreakingGlass", audioScript.SFX);
            if (collision.gameObject.transform.name == "birchTree")
            {
                //levelManagerScript.gameScore += gameplaySettings.rewardForHittingATarget;
                collision.gameObject.GetComponent<FadeOutTool>().enabled = true;
            }
            else if (collision.gameObject.transform.name == "trotyl" || collision.gameObject.transform.name == "trotylLauncher")
            {
                //levelManagerScript.gameScore += gameplaySettings.rewardForHittingATarget;
                Destroy(collision.gameObject);
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
                {
                    child.gameObject.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * gameplaySettings.debrisSplashForce);
                }
            }
            Destroy(gameObject);
        }
    }
}
