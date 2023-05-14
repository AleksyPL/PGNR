using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameMode
{
    SinglePlayerClassic,
    VersusClassic
}

public class MainMenuManager : MonoBehaviour
{
    internal AudioManager audioScript;
    internal PlaneSkinSelector planeSkinSelectorScript;
    public GameMode currentGameMode;
    public GameplaySettings gameplaySettings;
    public GameObject audioManagerGameObject;
    public GameObject optionsMenuGameObject;
    public GameObject howToPlayPanelMenuGameObject;
    public GameObject menuButtonsMainGameObject;
    public GameObject skinSelectorMenuGameObject;
    //buttons
    public GameObject startSinglePlayerModeMenuButton;
    public GameObject startMultiPlayerModeMenuButton;
    public GameObject optionsMenuButton;
    public GameObject howToPlayPanelMenuButton;
    public GameObject exitGameMenuButton;
    void Start()
    {
        Application.targetFrameRate = 144;
        currentGameMode = GameMode.SinglePlayerClassic;
        planeSkinSelectorScript = GetComponent<PlaneSkinSelector>();
        audioScript = audioManagerGameObject.GetComponent<AudioManager>();
        audioScript.PlaySound("MainMenuTheme", audioScript.otherSounds);
        menuButtonsMainGameObject.SetActive(true);
        UpdateUIButtonsWithLocalization();
    }
    private void UpdateUIButtonsWithLocalization()
    {
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
        UpdateUIButtonsWithLocalization();
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
        UpdateUIButtonsWithLocalization();
    }
    public void EnableSkinSelectorMenu(int newGameMode)
    {
        if (newGameMode == 0)
            currentGameMode = GameMode.SinglePlayerClassic;
        else if (newGameMode == 1)
            currentGameMode = GameMode.VersusClassic;
        menuButtonsMainGameObject.SetActive(false);
        skinSelectorMenuGameObject.SetActive(true);
        planeSkinSelectorScript.UpdateUIElements();
    }
    public void DisableSkinSelectorMenu()
    {
        menuButtonsMainGameObject.SetActive(true);
        skinSelectorMenuGameObject.SetActive(false);
        UpdateUIButtonsWithLocalization();
        gameplaySettings.ResetPlayerSkins();
    }
    public void LaunchTheGame()
    {
        if (currentGameMode == GameMode.SinglePlayerClassic)
            StartGameSinglePlayer();
        else if (currentGameMode == GameMode.VersusClassic)
            StartGameMultiPlayer();
    }
}
