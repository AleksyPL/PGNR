using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneRenderer : MonoBehaviour
{
    internal PlaneSkin planeSkin;
    private bool wheelsSlideOut;
    public float timeToFullySlideOutWheels;
    private float wheelsSlidingOutCounter;
    private float wheelsInitialPositionY;
    private void OnEnable()
    {
        wheelsInitialPositionY = transform.Find("WheelsRenderer").gameObject.transform.localPosition.y;
        timeToFullySlideOutWheels = (float)System.Math.Round(timeToFullySlideOutWheels, 2);
    }
    private void Update()
    {
        if(wheelsSlideOut)
        {
            wheelsSlidingOutCounter += Time.deltaTime;
            SlideOutWheels();
            if (wheelsSlidingOutCounter >= timeToFullySlideOutWheels)
            {
                wheelsSlidingOutCounter = timeToFullySlideOutWheels;
                wheelsSlideOut = false;
            }
        }
    }
    internal void ResetPlaneRenderer(PlaneState currentPlaneState)
    {
        ChangePlaneSprite(currentPlaneState);
        ChangeTilt(currentPlaneState, 0);
        transform.parent.Find("HolesRenderer").GetComponent<SpriteRenderer>().sprite = null;
        transform.parent.Find("HolesRenderer").gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.Find("WheelsRenderer").GetComponent<SpriteRenderer>().sprite = null;
        transform.Find("WheelsRenderer").gameObject.transform.localPosition = new Vector3(0, wheelsInitialPositionY, 0);
        while (transform.Find("SmokeSpawnerInAir").transform.childCount > 0)
            DestroyImmediate(transform.Find("SmokeSpawnerInAir").transform.GetChild(0).gameObject);
        while (transform.Find("SmokeSpawnerOnTheGround").transform.childCount > 0)
            DestroyImmediate(transform.Find("SmokeSpawnerOnTheGround").transform.GetChild(0).gameObject);
        wheelsSlidingOutCounter = 0;
        wheelsSlideOut = false;
    }
    internal void ChangePlaneSprite(PlaneState currentPlaneState)
    {
        if (GetComponent<SpriteRenderer>())
        {
            if (currentPlaneState == PlaneState.standard)
                GetComponent<SpriteRenderer>().sprite = planeSkin.planeStandard;
            else if (currentPlaneState == PlaneState.crashed)
            {
                GetComponent<SpriteRenderer>().sprite = planeSkin.planeCrashed;
                transform.parent.Find("HolesRenderer").GetComponent<SpriteRenderer>().sprite = null;
            }
            else if (currentPlaneState == PlaneState.wheelsOn)
            {
                transform.Find("WheelsRenderer").GetComponent<SpriteRenderer>().sprite = planeSkin.planeWheels;
                wheelsSlideOut = true;
            }
            else if (currentPlaneState == PlaneState.damaged)
            {
                transform.parent.Find("HolesRenderer").GetComponent<SpriteRenderer>().sprite = planeSkin.planeHoles;
                transform.parent.Find("HolesRenderer").gameObject.transform.localRotation = Quaternion.Euler(0, 0, -15f);
            }
        }
    }
    internal void ShowShield()
    {
        transform.Find("ShieldRenderer").GetComponent<SpriteRenderer>().enabled = true;
    }
    internal void HideShield()
    {
        transform.Find("ShieldRenderer").GetComponent<SpriteRenderer>().enabled = false;
    }
    internal void ChangeTilt(PlaneState currentPlaneState, float direction)
    {
        if (currentPlaneState == PlaneState.standard)
        {
            if (direction > 0)
                transform.rotation = Quaternion.Euler(0, 0, 15f);
            else if (direction == 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            if (direction < 0)
                transform.rotation = Quaternion.Euler(0, 0, -15f);
        }
        else if (currentPlaneState == PlaneState.wheelsOn || currentPlaneState == PlaneState.crashed)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (currentPlaneState == PlaneState.damaged)
            transform.rotation = Quaternion.Euler(0, 0, -15f);
    }
    private void SlideOutWheels()
    {
        float lerpValue = wheelsSlidingOutCounter / timeToFullySlideOutWheels;
        if (transform.Find("WheelsRenderer").gameObject.transform.localPosition.y > 0)
            transform.Find("WheelsRenderer").gameObject.transform.localPosition = new Vector3(0, Mathf.Lerp(wheelsInitialPositionY, 0, lerpValue), 0);
    }
}
