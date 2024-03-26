using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject objectToFollow;
    [SerializeField] private GameplaySettings gameplaySettings;
    private void Start()
    {
        if (UnityEngine.Device.Application.isMobilePlatform)
        {
            //float targetaspect = 16.0f / 9.0f;
            //float windowaspect = (float)Screen.width / (float)Screen.height;
            //float scaleheight = windowaspect / targetaspect;
            //Camera camera = GetComponent<Camera>();

            //if (scaleheight < 1.0f)
            //{
            //    Rect rect = camera.rect;

            //    rect.width = 1.0f;
            //    rect.height = scaleheight;
            //    rect.x = 0;
            //    rect.y = (1.0f - scaleheight) / 2.0f;

            //    camera.rect = rect;
            //}
            //else // add pillarbox
            //{
            //    float scalewidth = 1.0f / scaleheight;

            //    Rect rect = camera.rect;

            //    rect.width = scalewidth;
            //    rect.height = 1.0f;
            //    rect.x = (1.0f - scalewidth) / 2.0f;
            //    rect.y = 0;

            //    camera.rect = rect;
            //}

            //ScreenSizeX = Screen.width;
            //ScreenSizeY = Screen.height;
            //transform.GetComponent<Camera>().aspect = (float)(1920 / 1080);
        }
    }
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
