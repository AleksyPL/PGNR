using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trotyl : MonoBehaviour
{
    public GameObject explosionPrefab;
    private GameModeManager gameModeManagerScript;
    private GameObject audioManagerGameObject;
    private AudioManager audioScript;
    void Start()
    {
        gameModeManagerScript = GameObject.Find("MasterController").GetComponent<GameModeManager>();
        //audioManagerGameObject = GameObject.Find("AudioManager");
        //audioScript = audioManagerGameObject.GetComponent<AudioManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                //audioScript.PlaySound("Explosion", audioScript.SFX);
            }
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Plane"))
        {
            if (gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).currentPlaneState != PlaneState.damaged)
                gameModeManagerScript.ReturnAPlaneObject(collision.gameObject).DamageThePlane();
            Destroy(gameObject);
        }
    }
}
