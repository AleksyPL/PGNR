using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutTool : MonoBehaviour
{
    private float lifeTime;
    private float colorFadeTick;
    private float colorFadeCounter;
    private SpriteRenderer[] fadingElements;
    private Collider2D[] colliderElements;
    public GameplaySettings gameplaySettings;
    private void Start()
    {
        lifeTime = gameplaySettings.fadeOutLifeTime;
        colorFadeCounter = 0;
        colorFadeTick = lifeTime / 255;
        fadingElements = GetComponentsInChildren<SpriteRenderer>();
        colliderElements = GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliderElements.Length; i++)
            colliderElements[i].GetComponent<Collider2D>().enabled = false;
    }
    private void Update()
    {
        colorFadeCounter += Time.deltaTime;
        if (fadingElements.Length > 0 && colorFadeCounter >= colorFadeTick)
            FadeOut();
    }
    private void FadeOut()
    {
        for (int i = 0; i < fadingElements.Length; i++)
            if (fadingElements[i].GetComponent<SpriteRenderer>().color.a > 1)
                fadingElements[i].GetComponent<SpriteRenderer>().color = new Color(fadingElements[i].GetComponent<SpriteRenderer>().color.r, fadingElements[i].GetComponent<SpriteRenderer>().color.g, fadingElements[i].GetComponent<SpriteRenderer>().color.b, fadingElements[i].GetComponent<SpriteRenderer>().color.a - 1f / 255f);
            else
            {
                fadingElements[i].GetComponent<SpriteRenderer>().color = new Color(fadingElements[i].GetComponent<SpriteRenderer>().color.r, fadingElements[i].GetComponent<SpriteRenderer>().color.g, fadingElements[i].GetComponent<SpriteRenderer>().color.b, 0);
                Destroy(transform.gameObject);
            }
        colorFadeCounter = 0;
    }
}
