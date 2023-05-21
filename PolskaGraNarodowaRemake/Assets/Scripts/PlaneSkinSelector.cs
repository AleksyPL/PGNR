using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneSkinSelector : MonoBehaviour
{
    internal MainMenuManager mainMenuManagerScript;
    public GameplaySettings gameplaySettings;
    [Header("Player One")]
    public GameObject playerOneMainGameObject;
    public GameObject playerOneIndicatorGameObject;
    public GameObject playerOnePlaneImageGameObject;
    public GameObject playerOneSkinNameGameObject;
    [Header("Player Two")]
    public GameObject playerTwoMainGameObject;
    public GameObject playerTwoIndicatorGameObject;
    public GameObject playerTwoPlaneImageGameObject;
    public GameObject playerTwoSkinNameGameObject;
    [Header("Control Buttons")]
    public GameObject backToMainMenuButtonGameObject;
    public GameObject startGameButtonGameObject;

    private void Start()
    {
        mainMenuManagerScript = GetComponent<MainMenuManager>();
    }
    internal void UpdateUIElements()
    {
        backToMainMenuButtonGameObject.transform.Find("Text").GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenuButton;
        startGameButtonGameObject.transform.Find("Text").GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].launchGameButton;
        playerOnePlaneImageGameObject.GetComponent<Image>().sprite = gameplaySettings.planeSkins[gameplaySettings.playersPlaneSkins[0]].planeStandard;
        playerOneSkinNameGameObject.GetComponent<Text>().text = gameplaySettings.planeSkins[gameplaySettings.playersPlaneSkins[0]].skinName[gameplaySettings.langauageIndex];
        if (mainMenuManagerScript.currentGameMode == GameMode.SinglePlayerClassic)
        {
            playerOneIndicatorGameObject.SetActive(false);
            playerTwoMainGameObject.SetActive(false);
        }
        else if (mainMenuManagerScript.currentGameMode == GameMode.VersusClassic)
        {
            playerTwoMainGameObject.SetActive(true);
            playerOneIndicatorGameObject.SetActive(true);
            playerOneIndicatorGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
            playerTwoIndicatorGameObject.GetComponent<Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
            playerTwoPlaneImageGameObject.GetComponent<Image>().sprite = gameplaySettings.planeSkins[gameplaySettings.playersPlaneSkins[1]].planeStandard;
            playerTwoSkinNameGameObject.GetComponent<Text>().text = gameplaySettings.planeSkins[gameplaySettings.playersPlaneSkins[1]].skinName[gameplaySettings.langauageIndex];
        }
    }
    public void NextPlaneSkin(int playerNumber)
    {
        gameplaySettings.playersPlaneSkins[playerNumber]++;
        if (gameplaySettings.playersPlaneSkins[playerNumber] > gameplaySettings.planeSkins.Length - 1)
            gameplaySettings.playersPlaneSkins[playerNumber] = 0;
        UpdateUIElements();
    }
    public void PreviousPlaneSkin(int playerNumber)
    {
        gameplaySettings.playersPlaneSkins[playerNumber]--;
        if (gameplaySettings.playersPlaneSkins[playerNumber] < 0)
            gameplaySettings.playersPlaneSkins[playerNumber] = gameplaySettings.planeSkins.Length - 1;
        UpdateUIElements();
    }
}
