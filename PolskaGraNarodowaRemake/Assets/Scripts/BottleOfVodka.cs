using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleOfVodka : MonoBehaviour
{
    public GameObject crackedVersionPrefab;
    public GameObject explosionPrefab;
    void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
        {
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
                Instantiate(crackedVersionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
