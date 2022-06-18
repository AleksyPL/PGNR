using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trotyl : MonoBehaviour
{
    public GameObject explosionPrefab;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (explosionPrefab != null)
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Plane"))
        {
            collision.gameObject.GetComponent<PlaneBase>().flightControllScript.DamageThePlane();
            Destroy(gameObject);
        }
    }
}
