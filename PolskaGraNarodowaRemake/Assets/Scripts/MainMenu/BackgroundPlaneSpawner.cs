using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPlaneSpawner : MonoBehaviour
{
    public GameObject backgroundPlanePrefab;
    public List<GameObject> spawners;
    private float delayBetweenSpawns;
    private float delayBetweenSpawnsCounter;
    public float flightAngleVariationMax;
    // Start is called before the first frame update
    void Start()
    {
        delayBetweenSpawns = Random.Range(0.1f, 5);
        if (flightAngleVariationMax == 0)
            flightAngleVariationMax = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(delayBetweenSpawns > 0)
        {
            delayBetweenSpawnsCounter += Time.deltaTime;
            if(delayBetweenSpawnsCounter > delayBetweenSpawns)
            {
                SpawnBackgroundPlane();
                delayBetweenSpawnsCounter = 0;
                delayBetweenSpawns = Random.Range(0.1f, 5);
            }
        }
    }
    private void SpawnBackgroundPlane()
    {
        int spawnerNumber = Random.Range(0, spawners.Count);
        GameObject backgroundPlane = Instantiate(backgroundPlanePrefab, spawners[spawnerNumber].transform.position, Quaternion.identity, spawners[spawnerNumber].transform);
        backgroundPlane.layer = LayerMask.NameToLayer("UI");
        //scale
        float planeScale = Random.Range(0.01f, 1);
        backgroundPlane.transform.localScale = new Vector3(planeScale, planeScale, 1);
        //rotation
        float angleVariation = Random.Range(0, flightAngleVariationMax);
        int angleVariationAddOrSub = Random.Range(0, 2);
        if(angleVariationAddOrSub == 0)
            backgroundPlane.transform.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -backgroundPlane.transform.GetComponent<RectTransform>().localRotation.z + angleVariation);
        else
            backgroundPlane.transform.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -backgroundPlane.transform.GetComponent<RectTransform>().localRotation.z - angleVariation);
        LaunchSpawnedPlane(ref backgroundPlane);
    }
    private void LaunchSpawnedPlane(ref GameObject backgroundPlane)
    {
        backgroundPlane.GetComponent<Rigidbody2D>().AddForce(backgroundPlane.GetComponent<RectTransform>().up * 2500);
    }
}
