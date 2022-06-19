using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject audioManagerGameObject;
    internal AudioManager audioScript;
    void Start()
    {
        Application.targetFrameRate = 144;
        audioScript = audioManagerGameObject.GetComponent<AudioManager>();
        audioScript.PlaySound("MainMenuTheme", audioScript.otherSounds);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
