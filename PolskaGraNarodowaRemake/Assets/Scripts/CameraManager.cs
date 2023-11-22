using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject objectToFollow;
    [SerializeField] private GameplaySettings gameplaySettings;
    private void Update()
    {
        transform.position = new Vector3(objectToFollow.transform.position.x + gameplaySettings.cameraPositionXOffset, transform.position.y, transform.position.z);
    }
    internal void PlayCameraFocusAnimation()
    {
        GetComponent<Animator>().SetBool("ActivateFocus", true);
    }
    public void StopCameraFocusAnimation()
    {
        GetComponent<Animator>().SetBool("ActivateFocus", false);
    }
    internal void PlayCameraShakeAnimation()
    {
        GetComponent<Animator>().SetBool("ActivateShake", true);
    }
    public void StopCameraStopAnimation()
    {
        GetComponent<Animator>().SetBool("ActivateShake", false);
    }
}
