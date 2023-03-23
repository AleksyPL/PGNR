using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject cameraGameObject;
    public GameObject objectToFollow;
    public GameplaySettings gameplaySettings;
    public GameObject levelManagerGameObject;
    private void Update()
    {
        cameraGameObject.transform.position = new Vector3(objectToFollow.transform.position.x + gameplaySettings.cameraPositionXOffset, cameraGameObject.transform.position.y, cameraGameObject.transform.position.z);
        CheckObjectsToDestroy();
    }
    private void CheckObjectsToDestroy()
    {
        foreach (Transform child in levelManagerGameObject.transform)
        {
            if(child.transform.name != "airport")
                if(child.transform.position.x + gameplaySettings.cameraDespawnDisatance < (cameraGameObject.transform.position.x - (cameraGameObject.GetComponent<Camera>().aspect * cameraGameObject.GetComponent<Camera>().orthographicSize) / 2))
                    Destroy(child.gameObject);
        }
    }
}