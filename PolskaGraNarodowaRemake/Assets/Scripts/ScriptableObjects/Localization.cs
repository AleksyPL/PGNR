using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(menuName = "ScriptableObjects/Localization")]
public class Localization : ScriptableObject
{
    public TextAsset localizationTextFile;
    //main menu
    internal string mainMenuButton0;
    internal string mainMenuButton1;
    internal string mainMenuButton2;
    internal string mainMenuButton3;
    internal string mainMenuButton4;
    //how to play
    internal string howToPlayTitle;
    internal string howToPlayStory;
    internal string howtoPlayControlsPlayerOne;
    internal string howtoPlayControlsPlayerTwo;
    internal string playerOneIndicator;
    internal string playerTwoIndicator;
    internal string backToMainMenuButton;
    internal string launchGameButton;
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
    internal string regularHudPlaneDestroyed;
    //pause screen
    internal string pauseScreenPauseMainTitle;
    internal string pauseScreenGameOverMainTitle;
    internal string pauseScreenYearTitle;
    internal string pauseScreenBottlesTitle;
    internal string pauseScreenButton0;
    internal string pauseScreenButton1;
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
            StreamReader inp_stm = new StreamReader(Application.dataPath + "/Localization Files/" + localizationTextFile.name + ".txt");
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
            mainMenuButton4 = listOfAll[9];
            howToPlayTitle = listOfAll[11];
            howToPlayStory = listOfAll[13];
            howtoPlayControlsPlayerOne = listOfAll[15];
            howtoPlayControlsPlayerTwo = listOfAll[17];
            playerOneIndicator = listOfAll[19];
            playerTwoIndicator = listOfAll[21];
            backToMainMenuButton = listOfAll[23];
            sfxVolumeSlider = listOfAll[25];
            musicVolumeSlider = listOfAll[27];
            quotesVolumeSlider = listOfAll[29];
            activeLanguage = listOfAll[31];
            regularHudYear = listOfAll[33];
            regularHudProgression0 = listOfAll[35];
            regularHudProgression1 = listOfAll[37];
            regularHudLandingMessage = listOfAll[39];
            regularHudPlaneHit = listOfAll[41];
            regularHudPlaneDestroyed = listOfAll[43];
            regularHudBottle = listOfAll[45];
            regularHudEarned0 = listOfAll[47];
            regularHudEarned1 = listOfAll[49];
            pauseScreenYearTitle = listOfAll[51];
            pauseScreenBottlesTitle = listOfAll[53];
            pauseScreenPauseMainTitle = listOfAll[55];
            pauseScreenGameOverMainTitle = listOfAll[57];
            pauseScreenButton0 = listOfAll[59];
            pauseScreenButton1 = listOfAll[61];
            gameOverScreenButton0 = listOfAll[63];
            warningTitle = listOfAll[65];
            warningYes = listOfAll[67];
            warningNo = listOfAll[69];
            colorPanelPlayerWins = listOfAll[71];
            colorPanelPlayerLoses = listOfAll[73];
            colorPanelPlayerLosesSinglePlayer = listOfAll[75];
            activeBottleWarning = listOfAll[77];
            launchGameButton = listOfAll[79];
        }
    }
}
