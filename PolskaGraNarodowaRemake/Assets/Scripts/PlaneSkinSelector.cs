using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaneSkinSelector : MonoBehaviour
{
    internal MainMenuManager mainMenuManagerScript;
    public GameplaySettings gameplaySettings;
    public GameObject mainTitleGameObject;
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
        backToMainMenuButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        startGameButtonGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].mainMenuStartGame;
        mainTitleGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].skinSelectionMenuTitle;
        playerOnePlaneImageGameObject.GetComponent<Image>().sprite = gameplaySettings.planeSkins[gameplaySettings.playersPlaneSkins[0]].planeStandard;
        playerOneSkinNameGameObject.GetComponent<TMP_Text>().text = gameplaySettings.planeSkins[gameplaySettings.playersPlaneSkins[0]].skinName[gameplaySettings.langauageIndex];
        if (MainMenuManager.currentGameMode == GameMode.SinglePlayerClassic || MainMenuManager.currentGameMode == GameMode.SinglePlayerEndless)
        {
            playerOneMainGameObject.GetComponent<RectTransform>().position = new Vector3(playerOneMainGameObject.gameObject.transform.parent.GetComponent<RectTransform>().position.x, playerOneMainGameObject.GetComponent<RectTransform>().position.y, playerOneMainGameObject.GetComponent<RectTransform>().position.z);
            playerOneIndicatorGameObject.SetActive(false);
            playerTwoMainGameObject.SetActive(false);
        }
        else if (MainMenuManager.currentGameMode == GameMode.VersusClassic || MainMenuManager.currentGameMode == GameMode.VersusEndless)
        {
            playerTwoMainGameObject.SetActive(true);
            playerOneIndicatorGameObject.SetActive(true);
            playerOneMainGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-playerTwoMainGameObject.GetComponent<RectTransform>().anchoredPosition.x, playerOneMainGameObject.GetComponent<RectTransform>().anchoredPosition.y);
            playerOneIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerOneIndicator;
            playerTwoIndicatorGameObject.GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].playerTwoIndicator;
            playerTwoPlaneImageGameObject.GetComponent<Image>().sprite = gameplaySettings.planeSkins[gameplaySettings.playersPlaneSkins[1]].planeStandard;
            playerTwoSkinNameGameObject.GetComponent<TMP_Text>().text = gameplaySettings.planeSkins[gameplaySettings.playersPlaneSkins[1]].skinName[gameplaySettings.langauageIndex];
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
