using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(menuName = "ScriptableObjects/Localization")]
public class Localization : ScriptableObject
{
    public string localizationTextFile;
    //public TextAsset localizationTextFile;
    //main menu
    internal string mainMenuButton0;
    internal string mainMenuButton1;
    internal string mainMenuButton2;
    internal string mainMenuButton3;
    //game mode selection menu
    internal string gameModeSelectionMenuTitle;
    internal string gameModeSelectionMenuButton0;
    internal string gameModeSelectionMenuButton1;
    internal string gameModeSelectionMenuButton2;
    internal string gameModeSelectionMenuButton3;
    //skin selection menu
    internal string skinSelectionMenuTitle;
    //how to play
    internal string howToPlayTitle;
    internal string howToPlayStory;
    internal string howtoPlayControlsPlayerOne;
    internal string howtoPlayControlsPlayerTwo;
    internal string playerOneIndicator;
    internal string playerTwoIndicator;
    internal string backToMainMenuButton;
    //options
    internal string sfxVolumeSlider;
    internal string musicVolumeSlider;
    internal string quotesVolumeSlider;
    internal string activeLanguage;
    //regular hud
    internal string regularHudYear;
    internal string regularHudBottle;
    internal string regularHudEarned0;
    internal string regularHudEarned1;
    internal string regularHudProgression0;
    internal string regularHudProgression1;
    internal string regularHudLandingMessage;
    internal string regularHudPlaneHit;
    //pause screen
    internal string pauseScreenPauseMainTitle;
    internal string pauseScreenGameOverMainTitle;
    internal string pauseScreenYearTitle;
    internal string pauseScreenBottlesTitle;
    internal string pauseScreenButton0;
    internal string gameOverScreenButton0;
    internal string activeBottleWarning;
    internal string warningTitle;
    internal string warningYes;
    internal string warningNo;
    //color panel
    internal string colorPanelPlayerWins;
    internal string colorPanelPlayerLoses;
    internal string colorPanelPlayerLosesSinglePlayer;
    private void OnEnable()
    {
        LoadData();
    }
    private void LoadData()
    {
        List<string> listOfAll = new List<string>();
        if(localizationTextFile != null)
        {
            StreamReader inp_stm = new StreamReader(Application.streamingAssetsPath + "/" + localizationTextFile + ".txt");
            while (!inp_stm.EndOfStream)
            {
                string inp_ln = inp_stm.ReadLine();
                listOfAll.Add(inp_ln);
            }
            inp_stm.Close();
        }
        if (listOfAll.Count != 0)
        {
            mainMenuButton0 = listOfAll[1];
            mainMenuButton1 = listOfAll[3];
            mainMenuButton2 = listOfAll[5];
            mainMenuButton3 = listOfAll[7];
            gameModeSelectionMenuTitle = listOfAll[9];
            gameModeSelectionMenuButton0 = listOfAll[11];
            gameModeSelectionMenuButton1 = listOfAll[13];
            gameModeSelectionMenuButton2 = listOfAll[15];
            gameModeSelectionMenuButton3 = listOfAll[17];
            skinSelectionMenuTitle = listOfAll[19];
            howToPlayTitle = listOfAll[21];
            howToPlayStory = listOfAll[23];
            howtoPlayControlsPlayerOne = listOfAll[25];
            howtoPlayControlsPlayerTwo = listOfAll[27];
            playerOneIndicator = listOfAll[29];
            playerTwoIndicator = listOfAll[31];
            backToMainMenuButton = listOfAll[33];
            sfxVolumeSlider = listOfAll[35];
            musicVolumeSlider = listOfAll[37];
            quotesVolumeSlider = listOfAll[39];
            activeLanguage = listOfAll[41];
            regularHudYear = listOfAll[43];
            regularHudProgression0 = listOfAll[45];
            regularHudProgression1 = listOfAll[47];
            regularHudLandingMessage = listOfAll[49];
            regularHudPlaneHit = listOfAll[51];
            regularHudBottle = listOfAll[53];
            regularHudEarned0 = listOfAll[55];
            regularHudEarned1 = listOfAll[57];
            pauseScreenYearTitle = listOfAll[59];
            pauseScreenBottlesTitle = listOfAll[61];
            pauseScreenPauseMainTitle = listOfAll[63];
            pauseScreenGameOverMainTitle = listOfAll[65];
            pauseScreenButton0 = listOfAll[67];
            gameOverScreenButton0 = listOfAll[69];
            warningTitle = listOfAll[71];
            warningYes = listOfAll[73];
            warningNo = listOfAll[75];
            colorPanelPlayerWins = listOfAll[77];
            colorPanelPlayerLoses = listOfAll[79];
            colorPanelPlayerLosesSinglePlayer = listOfAll[81];
            activeBottleWarning = listOfAll[83];
        }
    }
}
