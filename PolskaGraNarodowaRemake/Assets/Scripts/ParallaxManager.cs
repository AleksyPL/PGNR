using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public float upperScreenY;
    public float lowerScreenY;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private GameObject cameraObject;
    public List<OneLayer> parallaxLayers;
    [System.Serializable]
    public class OneLayer
    {
        public GameObject parallaxLayerImage;
        public float parallaxEffectMultipler;
        internal float textureUnitSizeX;
        internal void LoadTextureUnitSizeX()
        {
            textureUnitSizeX = parallaxLayerImage.GetComponent<SpriteRenderer>().sprite.texture.width / parallaxLayerImage.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        }
    }
    internal void BackgroundSetup(float backgroundY, GameObject cameraObjectToSet)
    {
        transform.position = new Vector3(transform.position.x, backgroundY, transform.position.z);
        cameraObject = cameraObjectToSet;
        cameraTransform = cameraObject.transform;
        lastCameraPosition = cameraTransform.position;
        for (int i = 0; i < parallaxLayers.Count; i++)
            parallaxLayers[i].LoadTextureUnitSizeX();
    }
    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        lastCameraPosition = cameraTransform.position;
        for (int i = 0; i < parallaxLayers.Count; i++)
        {
            parallaxLayers[i].parallaxLayerImage.transform.position += deltaMovement * parallaxLayers[i].parallaxEffectMultipler;
            if (Mathf.Abs(cameraTransform.position.x - parallaxLayers[i].parallaxLayerImage.transform.position.x) >= parallaxLayers[i].textureUnitSizeX)
            {
                float offsetPositionX = (cameraTransform.position.x - parallaxLayers[i].parallaxLayerImage.transform.position.x) % parallaxLayers[i].textureUnitSizeX;
                parallaxLayers[i].parallaxLayerImage.transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, parallaxLayers[i].parallaxLayerImage.transform.position.y, 0);
            }
        }
    }
}
