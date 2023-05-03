using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneRenderer : MonoBehaviour
{
    internal SkinManager planeSkin;
    internal void LoadPlaneSkin()
    {
        this.GetComponent<SpriteRenderer>().sprite = planeSkin.planeWithoutWheels;
    }
    internal void ChangePlaneSprite(PlaneState currentPlaneState)
    {
        if (this.GetComponent<SpriteRenderer>())
        {
            if (currentPlaneState == PlaneState.standard)
                this.GetComponent<SpriteRenderer>().sprite = planeSkin.planeWithoutWheels;
            else if (currentPlaneState == PlaneState.crashed)
                this.GetComponent<SpriteRenderer>().sprite = planeSkin.planeCrashed;
            else if (currentPlaneState == PlaneState.wheelsOn)
                this.GetComponent<SpriteRenderer>().sprite = planeSkin.planeWithWheels;
            else if (currentPlaneState == PlaneState.damaged)
                this.GetComponent<SpriteRenderer>().sprite = planeSkin.planeWithHoles;
        }
    }
    internal void ChangeTilt(PlaneState currentPlaneState, float direction)
    {
        if (currentPlaneState == PlaneState.standard)
        {
            if (direction > 0)
                this.transform.rotation = Quaternion.Euler(0, 0, 15f);
            else if (direction == 0)
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (direction < 0)
                this.transform.rotation = Quaternion.Euler(0, 0, -15f);
        }
        else if (currentPlaneState == PlaneState.wheelsOn || currentPlaneState == PlaneState.crashed)
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (currentPlaneState == PlaneState.damaged)
            this.transform.rotation = Quaternion.Euler(0, 0, -15f);
    }
}
