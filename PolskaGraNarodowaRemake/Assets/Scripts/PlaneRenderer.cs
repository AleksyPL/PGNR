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
    public GameObject planeControlCenterGameObject;
    internal PlaneBase baseScript;
    void OnEnable()
    {
        baseScript = planeControlCenterGameObject.GetComponent<PlaneBase>();
        ChangePlaneSprite();
    }
    private void Update()
    {
        ChangeTilt();
    }
    internal void ChangePlaneSprite()
    {
        if(rendererEntity.GetComponent<SpriteRenderer>() && rendererEntity != null)
        {
            if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
                rendererEntity.GetComponent<SpriteRenderer>().sprite = planeWithoutWheels;
            else if (baseScript.currentPlaneState == PlaneBase.StateMachine.crashed)
                rendererEntity.GetComponent<SpriteRenderer>().sprite = planeCrashed;
            else if (baseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
                rendererEntity.GetComponent<SpriteRenderer>().sprite = planeWithWheels;
            else if (baseScript.currentPlaneState == PlaneBase.StateMachine.damaged)
                rendererEntity.GetComponent<SpriteRenderer>().sprite = planeWithHoles;
        }
    }
    internal void ChangeTilt()
    {
        if(baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
        {
            if (baseScript.inputScript.position.y > 0)
                rendererEntity.transform.rotation = Quaternion.Euler(0, 0, 15f);
            else if (baseScript.inputScript.position.y == 0)
                rendererEntity.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (baseScript.inputScript.position.y < 0)
                rendererEntity.transform.rotation = Quaternion.Euler(0, 0, -15f);
        }
        else if (baseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn || baseScript.currentPlaneState == PlaneBase.StateMachine.crashed)
            rendererEntity.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (baseScript.currentPlaneState == PlaneBase.StateMachine.damaged)
            rendererEntity.transform.rotation = Quaternion.Euler(0, 0, -15f);
    }
}
