using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trotyl : MonoBehaviour
{
    public GameObject explosionPrefab;
    private GameObject audioManagerGameObject;
    private AudioManager audioScript;
    void Start()
    {
        audioManagerGameObject = GameObject.Find("AudioManager");
        audioScript = audioManagerGameObject.GetComponent<AudioManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                audioScript.PlaySound("Explosion", audioScript.SFX);
            }
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Plane"))
        {
            collision.gameObject.GetComponentInChildren<HitDetectionManager>().planeBaseScript.flightControllScript.DamageThePlane();
            Destroy(gameObject);
        }
    }
}
