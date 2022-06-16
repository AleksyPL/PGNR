using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneRenderer : MonoBehaviour
{
    public Sprite planeWithoutWheels;
    public Sprite planeWithHoles;
    public Sprite planeWithWheels;
    public Sprite planeCrashed;
    public GameObject RendererEntity;
    internal PlaneBase baseScript;
    void Start()
    {
        baseScript = GetComponent<PlaneBase>();
        ChangePlaneSprite();
    }
    private void Update()
    {
        ChangeTilt();
    }
    internal void ChangePlaneSprite()
    {
        if(RendererEntity.GetComponent<SpriteRenderer>() && RendererEntity != null)
        {
            if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
                RendererEntity.GetComponent<SpriteRenderer>().sprite = planeWithoutWheels;
            else if (baseScript.currentPlaneState == PlaneBase.StateMachine.crashed)
                RendererEntity.GetComponent<SpriteRenderer>().sprite = planeCrashed;
            else if (baseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
                RendererEntity.GetComponent<SpriteRenderer>().sprite = planeWithWheels;
            else if (baseScript.currentPlaneState == PlaneBase.StateMachine.damaged)
                RendererEntity.GetComponent<SpriteRenderer>().sprite = planeWithHoles;
        }
    }
    internal void ChangeTilt()
    {
        if(baseScript.currentPlaneState == PlaneBase.StateMachine.standard)
        {
            if (baseScript.inputScript.position.y > 0)
                RendererEntity.transform.rotation = Quaternion.Euler(0, 0, 15f);
            else if (baseScript.inputScript.position.y == 0)
                RendererEntity.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (baseScript.inputScript.position.y < 0)
                RendererEntity.transform.rotation = Quaternion.Euler(0, 0, -15f);
        }
        else if (baseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn || baseScript.currentPlaneState == PlaneBase.StateMachine.crashed)
            RendererEntity.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (baseScript.currentPlaneState == PlaneBase.StateMachine.damaged)
            RendererEntity.transform.rotation = Quaternion.Euler(0, 0, -15f);
    }
}
