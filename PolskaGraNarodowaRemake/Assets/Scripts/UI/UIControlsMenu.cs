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
        public GameObject shootPrefabPLAlt;
        public GameObject shootPrefabEN;
        public GameObject shootPrefabENAlt;
        public GameObject pausePrefab;
    }
    [System.Serializable]
    public class MobileControlsPrefabs
    {
        public GameObject upMovementPrefab;
        public GameObject downMovementPrefab;
        public GameObject shootPrefab;
        public GameObject shootAltPrefab;
        public GameObject pausePrefab;
    }
    public GameplaySettings gameplaySettings;
    public GameObject titleGameObject;
    public GameObject backToMainMenuButton;
    [Header("Mobile version")]
    public GameObject mobileMainGameObject;
    public GameObject mobilePrefabParent;
    public GameObject mobileUpArrowGameObject;
    public GameObject mobileDownArrowGameObject;
    public GameObject mobileShootGameObject;
    //public GameObject mobileShootAltGameObject;
    public GameObject mobilePauseGameObject;
    public GameObject mobileColumn1GameObject;
    public GameObject mobileColumn2GameObject;
    public MobileControlsPrefabs playerOneMobilePrefabs;
    [Header("Non Mobile version")]
    public GameObject nonMobileMainGameObject;
    public GameObject nonMobilePrefabParent;
    public GameObject playerOneIndicatorGameObject;
    public GameObject playerTwoIndicatorGameObject;
    public GameObject nonMobileUpArrowGameObject;
    public GameObject nonMobileDownArrowGameObject;
    public GameObject nonMobileShootGameObject;
    public GameObject nonMobilePauseGameObject;
    public NonMobileControlsPrefabs playerOneNonMobilePrefabs;
    public NonMobileControlsPrefabs playerTwoNonMobilePrefabs;
    void OnEnable()
    {
        CleanPrefabParentGameObject();
        SpawnUIButtonImages();
        UpdateUIWithLocalization();
        if (UnityEngine.Device.Application.isMobilePlatform)
            mobileMainGameObject.SetActive(true);
        else
            nonMobileMainGameObject.SetActive(true);
    }
    private void CleanPrefabParentGameObject()
    {
        if(nonMobilePrefabParent.transform.childCount > 0)
        {
            foreach (Transform child in nonMobilePrefabParent.transform)
                GameObject.Destroy(child.gameObject);
        }
    }
    private void SpawnUIButtonImages()
    {
        if (UnityEngine.Device.Application.isMobilePlatform)
        {
            //Up arrow
            GameObject button = Instantiate(playerOneMobilePrefabs.upMovementPrefab, mobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(mobileColumn1GameObject.transform.position.x, mobileUpArrowGameObject.transform.position.y);
            //Down arrow
            button = Instantiate(playerOneMobilePrefabs.downMovementPrefab, mobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(mobileColumn1GameObject.transform.position.x, mobileDownArrowGameObject.transform.position.y);
            //Shoot
            button = Instantiate(playerOneMobilePrefabs.shootPrefab, mobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(mobileColumn2GameObject.transform.position.x, mobileShootGameObject.transform.position.y);
            //Pause
            button = Instantiate(playerOneMobilePrefabs.pausePrefab, mobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(mobileColumn2GameObject.transform.position.x, mobilePauseGameObject.transform.position.y);
        }
        else
        {
            //Up arrow
            GameObject button = Instantiate(playerOneNonMobilePrefabs.upMovementPrefab, nonMobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(playerOneIndicatorGameObject.transform.position.x, nonMobileUpArrowGameObject.transform.position.y);
            button = Instantiate(playerTwoNonMobilePrefabs.upMovementPrefab, nonMobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(playerTwoIndicatorGameObject.transform.position.x, nonMobileUpArrowGameObject.transform.position.y);
            //Down arrow
            button = Instantiate(playerOneNonMobilePrefabs.downMovementPrefab, nonMobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(playerOneIndicatorGameObject.transform.position.x, nonMobileDownArrowGameObject.transform.position.y);
            button = Instantiate(playerTwoNonMobilePrefabs.downMovementPrefab, nonMobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(playerTwoIndicatorGameObject.transform.position.x, nonMobileDownArrowGameObject.transform.position.y);
            //Shoot
            if (gameplaySettings.langauageIndex == 0)
            {
                button = Instantiate(playerOneNonMobilePrefabs.shootPrefabPL, nonMobilePrefabParent.transform);
                button.GetComponent<RectTransform>().position = new Vector2(playerOneIndicatorGameObject.transform.position.x, nonMobileShootGameObject.transform.position.y);
                button = Instantiate(playerTwoNonMobilePrefabs.shootPrefabPL, nonMobilePrefabParent.transform);
                button.GetComponent<RectTransform>().position = new Vector2(playerTwoIndicatorGameObject.transform.position.x, nonMobileShootGameObject.transform.position.y);
            }
            else if (gameplaySettings.langauageIndex == 1)
            {
                button = Instantiate(playerOneNonMobilePrefabs.shootPrefabEN, nonMobilePrefabParent.transform);
                button.GetComponent<RectTransform>().position = new Vector2(playerOneIndicatorGameObject.transform.position.x, nonMobileShootGameObject.transform.position.y);
                button = Instantiate(playerTwoNonMobilePrefabs.shootPrefabEN, nonMobilePrefabParent.transform);
                button.GetComponent<RectTransform>().position = new Vector2(playerTwoIndicatorGameObject.transform.position.x, nonMobileShootGameObject.transform.position.y);
            }
            //Pause
            button = Instantiate(playerOneNonMobilePrefabs.pausePrefab, nonMobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(playerOneIndicatorGameObject.transform.position.x, nonMobilePauseGameObject.transform.position.y);
            button = Instantiate(playerTwoNonMobilePrefabs.pausePrefab, nonMobilePrefabParent.transform);
            button.GetComponent<RectTransform>().position = new Vector2(playerTwoIndicatorGameObject.transform.position.x, nonMobilePauseGameObject.transform.position.y);
        }
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
            mobileUpArrowGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuUp;
            mobileDownArrowGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuDown;
            mobileShootGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuShoot + ". " + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuShootAlt;
            mobilePauseGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuPauseMenu;
        }
        else
        {
            playerOneIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
            playerTwoIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
            nonMobileUpArrowGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuUp;
            nonMobileDownArrowGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuDown;
            nonMobileShootGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuShoot + ". " + gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuShootAlt;
            nonMobilePauseGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].controlsMenuPauseMenu;
        }
    }
}
