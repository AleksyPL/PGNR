using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITutorialScreen : MonoBehaviour
{
    public GameplaySettings gameplaySettings;
    private UIControlsMenu controlsMenuScript;
    public int tutorialScreenNumber;
    private Image imageGameObject;
    private TMP_Text textGameObject;
    void Start()
    {
        imageGameObject = transform.Find("Image").gameObject.GetComponent<Image>();
        textGameObject = transform.Find("MainText").gameObject.GetComponent<TMP_Text>();
        controlsMenuScript = transform.parent.parent.parent.transform.Find("ControlsPanel").GetComponent<UIControlsMenu>();
        SetupTutorialScreen(tutorialScreenNumber);
    }
    void SetupTutorialScreen(int stringNumber)
    {
        if (imageGameObject.IsActive() && imageGameObject.sprite != null)
            imageGameObject.color = new Color(imageGameObject.color.r, imageGameObject.color.g, imageGameObject.color.b, 255);
        string textToDisplay = "";
        if (stringNumber == 0)
        {
            textToDisplay = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen0;
            transform.Find("PresentationObject").Find("Up").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuUp;
            transform.Find("PresentationObject").Find("Down").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuDown;
            if (UnityEngine.Device.Application.isMobilePlatform)
            {
                GameObject button = Instantiate(controlsMenuScript.playerOneMobilePrefabs.upMovementPrefab, transform.Find("PresentationObject").Find("Up").transform);
                button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("Up").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("Up").transform.position.y);
                button = Instantiate(controlsMenuScript.playerOneMobilePrefabs.downMovementPrefab, transform.Find("PresentationObject").Find("Down").transform);
                button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("Down").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("Down").transform.position.y);
            }
            else
            {
                GameObject button = Instantiate(controlsMenuScript.playerOnePrefabs.upMovementPrefab, transform.Find("PresentationObject").Find("Up").transform);
                button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("Up").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("Up").transform.position.y);
                button = Instantiate(controlsMenuScript.playerOnePrefabs.downMovementPrefab, transform.Find("PresentationObject").Find("Down").transform);
                button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("Down").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("Down").transform.position.y);
            }
        }
        else if (stringNumber == 1)
        {
            textToDisplay = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen1;
        }
        textGameObject.text = textToDisplay;
    }
}
