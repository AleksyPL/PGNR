using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifeTime;
    private float lifeTimeCounter;
    public bool enableFading;
    private float colorFadeTick;
    private float colorFadeCounter;
    private SpriteRenderer[] fadingElements;
    private Collider2D[] colliderElements;
    public GameObject cameraGameObject;
    void Start()
    {
        lifeTimeCounter = 0;
        cameraGameObject = FindObjectOfType<Camera>().gameObject;
        if (enableFading)
        {
            colorFadeCounter = 0;
            colorFadeTick = lifeTime / 255;
            fadingElements = GetComponentsInChildren<SpriteRenderer>();
            colliderElements = GetComponentsInChildren<Collider2D>();
            for (int i = 0; i < colliderElements.Length; i++)
            {
                colliderElements[i].GetComponent<Collider2D>().enabled = false;
            }
        }

    }
    void Update()
    {
        lifeTimeCounter += Time.deltaTime;
        if(enableFading)
        {
            colorFadeCounter += Time.deltaTime;
            if (fadingElements.Length > 0 && colorFadeCounter >= colorFadeTick)
            {
                FadeOut();
            }
        }
        if (lifeTimeCounter >= lifeTime || (transform.position.x < cameraGameObject.transform.position.x && cameraGameObject.transform.position.x - transform.position.x > 20))
            Destroy(gameObject);
    }
    private void FadeOut()
    {
        for (int i = 0; i < fadingElements.Length; i++)
        {
            if (fadingElements[i].GetComponent<SpriteRenderer>().color.a > 0)
            {
                fadingElements[i].GetComponent<SpriteRenderer>().color = new Color(fadingElements[i].GetComponent<SpriteRenderer>().color.r, fadingElements[i].GetComponent<SpriteRenderer>().color.g, fadingElements[i].GetComponent<SpriteRenderer>().color.b, fadingElements[i].GetComponent<SpriteRenderer>().color.a - 1f / 255f);
            }
        }
        colorFadeCounter = 0;
    }
    public void DestroyInstant()
    {
        Destroy(gameObject);
    }
}
