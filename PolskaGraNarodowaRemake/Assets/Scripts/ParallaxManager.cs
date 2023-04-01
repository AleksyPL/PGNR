using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    public GameObject cameraObject;
    public float parallaxEffectMultipler;
    void Start()
    {
        cameraTransform = cameraObject.transform;
        lastCameraPosition = cameraTransform.position;
        textureUnitSizeX = GetComponent<SpriteRenderer>().sprite.texture.width / GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
    }
    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += deltaMovement * parallaxEffectMultipler;
        lastCameraPosition = cameraTransform.position;
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }
}
