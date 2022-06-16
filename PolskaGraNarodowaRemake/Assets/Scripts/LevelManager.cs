using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject airportGameObject;
    [SerializeField] private GameObject afterAirportDestroyPointGameObject;
    public GameObject planeGameObject;
    public LayerMask planeLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(afterAirportDestroyPointGameObject != null && Physics2D.Raycast(afterAirportDestroyPointGameObject.transform.position, Vector2.up, Mathf.Infinity, planeLayer))
        {
            planeGameObject.GetComponent<PlaneBase>().flightControllScript.DamageThePlane();
        }
    }
}
