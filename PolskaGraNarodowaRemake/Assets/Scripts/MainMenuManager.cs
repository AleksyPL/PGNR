using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    internal AudioManager audioScript;
    public GameplaySettings gameplaySettings;
    public GameObject audioManagerGameObject;
    public GameObject optionsMenuGameObject;
    public GameObject howToPlayPanelMenuGameObject;
    public GameObject menuButtonsMainGameObject;
    //buttons
    public GameObject startSinglePlayerModeMenuButton;
    public GameObject startMultiPlayerModeMenuButton;
    public GameObject optionsMenuButton;
    public GameObject howToPlayPanelMenuButton;
    public GameObject exitGameMenuButton;
    void Start()
    {
        Application.targetFrameRate = 144;
        audioScript = audioManagerGameObject.GetComponent<AudioManager>();
        audioScript.PlaySound("MainMenuTheme", audioScript.otherSounds);
        menuButtonsMainGameObject.SetActive(true);
        startSinglePlayerModeMenuButton.transform.Find("Text").GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton0;
        startMultiPlayerModeMenuButton.transform.Find("Text").GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton1;
        howToPlayPanelMenuButton.transform.Find("Text").GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton2;
        optionsMenuButton.transform.Find("Text").GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton3;
        exitGameMenuButton.transform.Find("Text").GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuButton4;
    }
    private void Update()
    {
        if(optionsMenuButton.activeSelf && Input.GetButtonDown("Cancel"))
            DisableOptionsMenu();
        else if (howToPlayPanelMenuButton.activeSelf && Input.GetButtonDown("Cancel"))
            DisableHowToPlayPanel();
    }
    public void StartGameSinglePlayer()
    {
        SceneManager.LoadScene("SinglePlayerMode");
    }
    public void StartGameMultiPlayer()
    {
        SceneManager.LoadScene("VersusMode");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void EnableOptionsMenu()
    {
        menuButtonsMainGameObject.SetActive(false);
        optionsMenuGameObject.SetActive(true);
    }
    public void DisableOptionsMenu()
    {
        menuButtonsMainGameObject.SetActive(true);
        optionsMenuGameObject.SetActive(false);
    }
    public void EnableHowToPlayPanel()
    {
        menuButtonsMainGameObject.SetActive(false);
        howToPlayPanelMenuGameObject.SetActive(true);
    }
    public void DisableHowToPlayPanel()
    {
        menuButtonsMainGameObject.SetActive(true);
        howToPlayPanelMenuGameObject.SetActive(false);
    }
}
