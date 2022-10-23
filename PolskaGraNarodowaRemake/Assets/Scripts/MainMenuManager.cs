using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject audioManagerGameObject;
    internal AudioManager audioScript;
    public GameObject regularMenuButtons;
    public GameObject optionsMenuButtons;
    public GameObject howToPlayPanel;
    void Start()
    {
        Application.targetFrameRate = 144;
        audioScript = audioManagerGameObject.GetComponent<AudioManager>();
        audioScript.PlaySound("MainMenuTheme", audioScript.otherSounds);
        regularMenuButtons.SetActive(true);
    }
    private void Update()
    {
        if(optionsMenuButtons.activeSelf && Input.GetButtonDown("Cancel"))
            DisableOptionsMenu();
        else if (howToPlayPanel.activeSelf && Input.GetButtonDown("Cancel"))
            DisableHowToPlayPanel();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("SinglePlayerMode");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void EnableOptionsMenu()
    {
        regularMenuButtons.SetActive(false);
        optionsMenuButtons.SetActive(true);
    }
    public void DisableOptionsMenu()
    {
        regularMenuButtons.SetActive(true);
        optionsMenuButtons.SetActive(false);
    }
    public void EnableHowToPlayPanel()
    {
        regularMenuButtons.SetActive(false);
        howToPlayPanel.SetActive(true);
    }
    public void DisableHowToPlayPanel()
    {
        regularMenuButtons.SetActive(true);
        howToPlayPanel.SetActive(false);
    }
}
