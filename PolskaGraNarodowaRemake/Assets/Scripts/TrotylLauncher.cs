using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrotylLauncher : MonoBehaviour
{
    public GameObject lauchingPoint;
    public GameObject trotylPrefab;
    public GameplaySettings gameplaySettings;
    private AudioManager audioScript;
    internal float rateOfFireCounter;
    void OnEnable()
    {
        if(gameplaySettings.maxLaunchDelay > gameplaySettings.rateOfFire)
            gameplaySettings.rateOfFire = gameplaySettings.maxLaunchDelay;
        rateOfFireCounter = Random.Range(0, gameplaySettings.maxLaunchDelay);
        audioScript = GameObject.Find("MasterController").GetComponent<AudioManager>();
    }
    void Update()
    {
        rateOfFireCounter += Time.deltaTime;
        if (rateOfFireCounter >= gameplaySettings.rateOfFire)
        {
            LaunchTheProjectile();
            rateOfFireCounter = 0;
        }
    }
    private void LaunchTheProjectile()
    {
        if (trotylPrefab != null && lauchingPoint != null)
        {
            GameObject trotyl = Instantiate(trotylPrefab, lauchingPoint.transform.position, Quaternion.identity, transform.parent);
            trotyl.name = "trotyl";
            trotyl.GetComponent<Rigidbody2D>().AddForce(Vector2.up * gameplaySettings.launchForce);
            audioScript.PlaySound("Cannon", audioScript.localSFX);
        }
    }
}
