using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject cameraGameObject;
    public GameObject objectToFollow;
    public float cameraPositionXOffset;
    private void Update()
    {
        cameraGameObject.transform.position = new Vector3(objectToFollow.transform.position.x + cameraPositionXOffset, cameraGameObject.transform.position.y, cameraGameObject.transform.position.z);
    }
}
