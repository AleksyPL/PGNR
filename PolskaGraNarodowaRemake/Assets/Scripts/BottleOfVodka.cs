using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleOfVodka : MonoBehaviour
{
    public GameObject crackedVersionPrefab;
    public GameObject explosionPrefab;
    public float debrisSplashForce;
    private GameObject audioManagerGameObject;
    private AudioManager audioScript;
    void Start()
    {
        audioManagerGameObject = GameObject.Find("AudioManager");
        audioScript = audioManagerGameObject.GetComponent<AudioManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
        {
            audioScript.PlaySound("BreakingGlass", audioScript.SFX);
            if (collision.gameObject.transform.name == "birchTree")
            {
                collision.gameObject.GetComponent<DestroyAfterTime>().enabled = true;
            }
            else if (collision.gameObject.transform.name == "trotyl" || collision.gameObject.transform.name == "trotylLauncher")
            {
                Destroy(collision.gameObject);
                if(explosionPrefab != null)
                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            if(crackedVersionPrefab !=null)
            {
                GameObject bottle = Instantiate(crackedVersionPrefab, transform.position, Quaternion.identity);
                foreach (Transform child in bottle.transform)
                {
                    child.gameObject.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * debrisSplashForce);
                }
            }
            Destroy(gameObject);
        }
    }
}
