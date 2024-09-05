using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonTransform
{
    internal Vector2 anchorMin;
    internal Vector2 anchorMax;
    internal Vector3 position;
    internal Vector3 scale;
}
public class FullScreenManager : MonoBehaviour
{
    internal bool landscapeModeEnabled;
    private GameObject fullScreenButton;
    private GameObject canvasScalerGameObject;
    public GameplaySettings gameplaySettings;
    internal ButtonTransform rectTransformHorizontal;
    void Start()
    {
        landscapeModeEnabled = false;
        fullScreenButton = transform.gameObject;
        canvasScalerGameObject = transform.parent.gameObject;
        rectTransformHorizontal = new ButtonTransform();
        if (UnityEngine.Device.Application.isMobilePlatform)
        {
            DetectDeviceResolution();
            if (SceneManager.GetActiveScene().name == "MainMenu")
                TurnOnFullScreenButton();
            //if (Screen.orientation != ScreenOrientation.LandscapeLeft && Screen.orientation != ScreenOrientation.LandscapeRight)
            //    rectTransformHorizontal = transform.GetComponent<RectTransform>();
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
        if (SceneManager.GetActiveScene().name == "MainMenu" && transform.parent.GetComponent<MainMenuManager>().gameIsNotFullScreenedAndVertical)
        {
            TurnOffFullScreenButton();
            transform.parent.GetComponent<MainMenuManager>().ScreenOrientationChangedToHorizontal();
        }
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
    internal void ModifyRectTransformOfTheGameObject()
    {
        rectTransformHorizontal.anchorMin = transform.GetComponent<RectTransform>().anchorMin;
        rectTransformHorizontal.anchorMax = transform.GetComponent<RectTransform>().anchorMax;
        rectTransformHorizontal.position = transform.GetComponent<RectTransform>().localPosition;
        rectTransformHorizontal.scale = transform.GetComponent<RectTransform>().localScale;
        transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        transform.GetComponent<RectTransform>().localScale = new Vector3(3, 3, 3);
    }
    internal void ResetRectTransformOfTheGameObject()
    {
        transform.GetComponent<RectTransform>().anchorMin = rectTransformHorizontal.anchorMin;
        transform.GetComponent<RectTransform>().anchorMax = rectTransformHorizontal.anchorMax;
        transform.GetComponent<RectTransform>().localPosition = rectTransformHorizontal.position;
        transform.GetComponent<RectTransform>().localScale = rectTransformHorizontal.scale;
    }
}
