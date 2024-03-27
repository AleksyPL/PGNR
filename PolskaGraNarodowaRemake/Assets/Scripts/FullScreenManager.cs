using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FullScreenManager : MonoBehaviour
{
    private bool landscapeModeEnabled;
    private GameObject fullScreenButton;
    private GameObject canvasScalerGameObject;
    public GameplaySettings gameplaySettings;
    void Start()
    {
        landscapeModeEnabled = false;
        fullScreenButton = transform.gameObject;
        canvasScalerGameObject = transform.parent.gameObject;
        if (UnityEngine.Device.Application.isMobilePlatform)
        {
            DetectDeviceResolution();
            if (SceneManager.GetActiveScene().name == "MainMenu")
                TurnOnFullScreenButton();
            //else if (SceneManager.GetActiveScene().name != "MainMenu" && Screen.fullScreen)
            //    SetScreenScalerMatchValue();
        }  
    }
    private void DetectDeviceResolution()
    {
        if(!gameplaySettings.mobileDataTaken)
        {
            gameplaySettings.deviceResolution = Screen.currentResolution;
            gameplaySettings.deviceScreenOrientation = Screen.orientation;
            gameplaySettings.mobileDataTaken = true;
        }
    }
    internal void TurnOffFullScreenButton()
    {
        if (fullScreenButton != null)
        {
            fullScreenButton.GetComponent<Image>().enabled = false;
            fullScreenButton.GetComponent<Button>().enabled = false;
        }
    }
    internal void TurnOnFullScreenButton()
    {
        if (fullScreenButton != null)
        {
            fullScreenButton.GetComponent<Image>().enabled = true;
            fullScreenButton.GetComponent<Button>().enabled = true;
        }
    }
    public void DoThingsWithFullScreen()
    {
        if (!landscapeModeEnabled)
        {
            EnableFullScreen();
        }
        else if (landscapeModeEnabled)
        {
            DisableFullScreen();
        }
    }
    //private void SetScreenScalerMatchValue()
    //{
    //    if (gameplaySettings.deviceResolution.width > gameplaySettings.deviceResolution.height)
    //        canvasScalerGameObject.GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
    //    else
    //        canvasScalerGameObject.GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
    //}
    private void EnableFullScreen()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        canvasScalerGameObject.GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        landscapeModeEnabled = true;
        if (Screen.fullScreen)
            Screen.fullScreen = false;
        else
            Screen.fullScreen = true;

    }
    private void DisableFullScreen()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        landscapeModeEnabled = false;
        if (Screen.fullScreen)
            Screen.fullScreen = false;
        else
            Screen.fullScreen = true;
    }
}
