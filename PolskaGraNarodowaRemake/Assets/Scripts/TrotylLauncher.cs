using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrotylLauncher : MonoBehaviour
{
    public GameObject lauchingPoint;
    [SerializeField] private GameObject trotylPrefab;
    [SerializeField] private float rateOfFire;
    [SerializeField] private float launchForce;
    private GameObject audioManagerGameObject;
    private AudioManager audioScript;
    private float rateOfFireCounter;
    void Start()
    {
        rateOfFireCounter = 0;
        audioManagerGameObject = GameObject.Find("AudioManager");
        audioScript = audioManagerGameObject.GetComponent<AudioManager>();
    }
    void Update()
    {
        rateOfFireCounter += Time.deltaTime;
        if (rateOfFireCounter >= rateOfFire)
        {
            LaunchTheProjectile();
            rateOfFireCounter = 0;
        }
    }
    private void LaunchTheProjectile()
    {
        if (trotylPrefab != null && lauchingPoint != null)
        {
            GameObject trotyl = Instantiate(trotylPrefab, lauchingPoint.transform.position, Quaternion.identity);
            trotyl.gameObject.name = "trotyl";
            trotyl.GetComponent<Rigidbody2D>().AddForce(Vector2.up * launchForce);
            audioScript.PlaySound("Cannon", audioScript.SFX);
        }
    }
}
