using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControlsMenu : MonoBehaviour
{
    [System.Serializable]
    public class NonMobileControlsPrefabs
    {
        public GameObject upMovementPrefab;
        public GameObject downMovementPrefab;
        public GameObject shootPrefabPL;
        public GameObject shootPrefabEN;
        public GameObject pausePrefab;
    }
    public GameplaySettings gameplaySettings;
    public GameObject titleGameObject;
    public GameObject backToMainMenuButton;
    [Header("Mobile version")]
    public GameObject mobileMainGameObject;
    public GameObject mobileUpArrowGameObject;
    public GameObject mobileDownArrowGameObject;
    public GameObject mobileShootGameObject;
    public GameObject mobilePauseGameObject;
    [Header("Non Mobile version")]
    public GameObject nonMobileMainGameObject;
    public GameObject nonMobilePrefabParent;
    public GameObject playerOneIndicatorGameObject;
    public GameObject playerTwoIndicatorGameObject;
    public NonMobileControlsPrefabs playerOnePrefabs;
    public NonMobileControlsPrefabs playerTwoPrefabs;
    public GameObject nonMobileUpArrowGameObject;
    public GameObject nonMobileDownArrowGameObject;
    public GameObject nonMobileShootGameObject;
    public GameObject nonMobilePauseGameObject;
    void OnEnable()
    {
        UpdateUIWithLocalization();
        if (UnityEngine.Device.Application.isMobilePlatform)
            mobileMainGameObject.SetActive(true);
        else
        {
            nonMobileMainGameObject.SetActive(true);
            CleanPrefabParentGameObject();
            SpawnUIButtonImages();
        }
    }
    private void CleanPrefabParentGameObject()
    {
        while (nonMobilePrefabParent.transform.childCount > 0)
            DestroyImmediate(nonMobilePrefabParent.transform.GetChild(0).gameObject);
    }
    private void SpawnUIButtonImages()
    {
        //Up arrow
        GameObject button = Instantiate(playerOnePrefabs.upMovementPrefab, nonMobilePrefabParent.transform);
        button.GetComponent<RectTransform>().position = new Vector2(playerOneIndicatorGameObject.transform.position.x, nonMobileUpArrowGameObject.transform.position.y);
        button = Instantiate(playerTwoPrefabs.upMovementPrefab, nonMobilePrefabParent.transform);
        button.GetComponent<RectTransform>().position = new Vector2(playerTwoIndicatorGameObject.transform.position.x, nonMobileUpArrowGameObject.transform.position.y);
        //Down arrow
        button = Instantiate(playerOnePrefabs.downMovementPrefab, nonMobilePrefabParent.transform);
        button.GetComponent<RectTransform>().position = new Vector2(playerOneIndicatorGameObject.transform.position.x, nonMobileDownArrowGameObject.transform.position.y);
        button = Instantiate(playerTwoPrefabs.downMovementPrefab, nonMobilePrefabParent.transform);
        button.GetComponent<RectTransform>().position = new Vector2(playerTwoIndicatorGameObject.transform.position.x, nonMobileDownArrowGameObject.transform.position.y);
        //Shoot
        if(gameplaySettings.langauageIndex == 0)
        {
            button = Instantiate(playerOnePrefabs.shootPrefabPL, nonMobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(playerOneIndicatorGameObject.transform.position.x, nonMobileShootGameObject.transform.position.y);
            button = Instantiate(playerTwoPrefabs.shootPrefabPL, nonMobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(playerTwoIndicatorGameObject.transform.position.x, nonMobileShootGameObject.transform.position.y);
        }
        else if (gameplaySettings.langauageIndex == 1)
        {
            button = Instantiate(playerOnePrefabs.shootPrefabEN, nonMobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(playerOneIndicatorGameObject.transform.position.x, nonMobileShootGameObject.transform.position.y);
            button = Instantiate(playerTwoPrefabs.shootPrefabEN, nonMobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(playerTwoIndicatorGameObject.transform.position.x, nonMobileShootGameObject.transform.position.y);
        }
        //Pause
        button = Instantiate(playerOnePrefabs.pausePrefab, nonMobilePrefabParent.transform);
        button.GetComponent<RectTransform>().position = new Vector2(playerOneIndicatorGameObject.transform.position.x, nonMobilePauseGameObject.transform.position.y);
        button = Instantiate(playerTwoPrefabs.pausePrefab, nonMobilePrefabParent.transform);
        button.GetComponent<RectTransform>().position = new Vector2(playerTwoIndicatorGameObject.transform.position.x, nonMobilePauseGameObject.transform.position.y);
    }
    private void UpdateUIWithLocalization()
    {
        
        titleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuControls;
        if (!gameplaySettings.introductionScreens && SceneManager.GetActiveScene().name == "MainMenu")
            backToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].acceptanceMessage;
        else if (gameplaySettings.introductionScreens && SceneManager.GetActiveScene().name == "MainMenu")
            backToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        else
            backToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToPauseScreen;
        if (UnityEngine.Device.Application.isMobilePlatform)
        {
            mobileUpArrowGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuUp;
            mobileDownArrowGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuDown;
            mobileShootGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuShoot;
            mobilePauseGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuPauseMenu;
        }
        else
        {
            playerOneIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
            playerTwoIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
            nonMobileUpArrowGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuUp;
            nonMobileDownArrowGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuDown;
            nonMobileShootGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuShoot;
            nonMobilePauseGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuPauseMenu;
        }
    }
}
