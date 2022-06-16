using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleOfVodka : MonoBehaviour
{
    [SerializeField] private GameObject crackedVersionPrefab;
    void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
        {
            //Destroy Objects TODO
            Instantiate(crackedVersionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
