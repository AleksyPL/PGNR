using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInTool : MonoBehaviour
{
    private float lifeTime;
    private float colorFadeTick;
    private float colorFadeCounter;
    private SpriteRenderer fadingImage;
    public GameplaySettings gameplaySettings;
    private void Start()
    {
        lifeTime = gameplaySettings.fadeInLifeTime;
        colorFadeCounter = 0;
        colorFadeTick = lifeTime / 255;
        fadingImage = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        colorFadeCounter += Time.deltaTime;
        if (fadingImage != null && colorFadeCounter >= colorFadeTick && fadingImage.color.a < 255)
            FadeIn();
    }
    private void FadeIn()
    {
        if (fadingImage.color.a < 255)
            fadingImage.color = new Color(fadingImage.color.r, fadingImage.color.g, fadingImage.color.b, fadingImage.color.a + 1f / 255f);
        else
            fadingImage.color = new Color(fadingImage.color.r, fadingImage.color.g, fadingImage.color.b, 255);
        colorFadeCounter = 0;
    }
}
