using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneRenderer : MonoBehaviour
{
    public Sprite planeWithoutWheels;
    public Sprite planeWithHoles;
    public Sprite planeWithWheels;
    public Sprite planeCrashed;
    public GameObject rendererEntity;
    //public GameObject planeControlCenterGameObject;
    //internal PlaneScript baseScript;
    void OnEnable()
    {
        //baseScript = planeControlCenterGameObject.GetComponent<PlaneScript>();
        //ChangePlaneSprite();
    }
    private void Update()
    {
        //ChangeTilt();
    }
    internal void ChangePlaneSprite(PlaneState currentPlaneState)
    {
        if (rendererEntity.GetComponent<SpriteRenderer>() && rendererEntity != null)
        {
            if (currentPlaneState == PlaneState.standard)
                rendererEntity.GetComponent<SpriteRenderer>().sprite = planeWithoutWheels;
            else if (currentPlaneState == PlaneState.crashed)
                rendererEntity.GetComponent<SpriteRenderer>().sprite = planeCrashed;
            else if (currentPlaneState == PlaneState.wheelsOn)
                rendererEntity.GetComponent<SpriteRenderer>().sprite = planeWithWheels;
            else if (currentPlaneState == PlaneState.damaged)
                rendererEntity.GetComponent<SpriteRenderer>().sprite = planeWithHoles;
        }
    }
    internal void ChangeTilt(PlaneState currentPlaneState, float direction)
    {
        if (currentPlaneState == PlaneState.standard)
        {
            if (direction > 0)
                rendererEntity.transform.rotation = Quaternion.Euler(0, 0, 15f);
            else if (direction == 0)
                rendererEntity.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (direction < 0)
                rendererEntity.transform.rotation = Quaternion.Euler(0, 0, -15f);
        }
        else if (currentPlaneState == PlaneState.wheelsOn || currentPlaneState == PlaneState.crashed)
            rendererEntity.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (currentPlaneState == PlaneState.damaged)
            rendererEntity.transform.rotation = Quaternion.Euler(0, 0, -15f);
    }
}
