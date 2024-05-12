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
        if(transform.Find("Image") != null && transform.Find("Image").gameObject.activeSelf)
            imageGameObject = transform.Find("Image").gameObject.GetComponent<Image>();
        textGameObject = transform.Find("MainText").gameObject.GetComponent<TMP_Text>();
        controlsMenuScript = transform.parent.parent.parent.transform.Find("ControlsPanel").GetComponent<UIControlsMenu>();
        SetupTutorialScreen(tutorialScreenNumber);
    }
    void SetupTutorialScreen(int stringNumber)
    {
        if (imageGameObject != null && imageGameObject.IsActive() && imageGameObject.sprite != null)
            imageGameObject.color = new Color(imageGameObject.color.r, imageGameObject.color.g, imageGameObject.color.b, 255);
        string textToDisplay = "";
        if(stringNumber == -1)
            textToDisplay = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialTryAgain;
        else if (stringNumber == 0)
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
                GameObject button = Instantiate(controlsMenuScript.playerOneNonMobilePrefabs.upMovementPrefab, transform.Find("PresentationObject").Find("Up").transform);
                button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("Up").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("Up").transform.position.y);
                button = Instantiate(controlsMenuScript.playerOneNonMobilePrefabs.downMovementPrefab, transform.Find("PresentationObject").Find("Down").transform);
                button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("Down").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("Down").transform.position.y);
            }
        }
        else if (stringNumber == 1)
            textToDisplay = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen1;
        else if (stringNumber == 2)
        {
            textToDisplay = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen2;
            transform.Find("PresentationObject").Find("Shoot").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuShoot;
            transform.Find("PresentationObject").Find("ShootAlt").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuShootAlt;
            if (UnityEngine.Device.Application.isMobilePlatform)
            {
                GameObject button = Instantiate(controlsMenuScript.playerOneMobilePrefabs.shootPrefab, transform.Find("PresentationObject").Find("Shoot").transform);
                button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("Shoot").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("Shoot").transform.position.y);
                button = Instantiate(controlsMenuScript.playerOneMobilePrefabs.shootAltPrefab, transform.Find("PresentationObject").Find("ShootAlt").transform);
                button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("ShootAlt").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("ShootAlt").transform.position.y);
            }
            else
            {
                if (gameplaySettings.langauageIndex == 0)
                {
                    GameObject button = Instantiate(controlsMenuScript.playerOneNonMobilePrefabs.shootPrefabPL, transform.Find("PresentationObject").Find("Shoot").transform);
                    button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("Shoot").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("Shoot").transform.position.y);
                    button = Instantiate(controlsMenuScript.playerOneNonMobilePrefabs.shootPrefabPLAlt, transform.Find("PresentationObject").Find("ShootAlt").transform);
                    button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("ShootAlt").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("ShootAlt").transform.position.y);
                }
                else if (gameplaySettings.langauageIndex == 1)
                {
                    GameObject button = Instantiate(controlsMenuScript.playerOneNonMobilePrefabs.shootPrefabEN, transform.Find("PresentationObject").Find("Shoot").transform);
                    button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("Shoot").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("Shoot").transform.position.y);
                    button = Instantiate(controlsMenuScript.playerOneNonMobilePrefabs.shootPrefabENAlt, transform.Find("PresentationObject").Find("ShootAlt").transform);
                    button.GetComponent<RectTransform>().position = new Vector2(transform.Find("PresentationObject").Find("ShootAlt").Find("Button").transform.position.x, transform.Find("PresentationObject").Find("ShootAlt").transform.position.y);
                }
            }
        }
        else if (stringNumber == 3)
            textToDisplay = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen3;
        else if (stringNumber == 4)
            textToDisplay = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen4;
        else if (stringNumber == 5)
            textToDisplay = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].tutorialScreen5;
        textGameObject.text = textToDisplay;
    }
}
