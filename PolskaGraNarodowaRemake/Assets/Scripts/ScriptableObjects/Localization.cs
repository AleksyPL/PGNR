using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;

[CreateAssetMenu(menuName = "ScriptableObjects/Localization")]
public class Localization : ScriptableObject
{
    public TextAsset localizationTextFile;
    //main menu
    internal string mainMenuStartGame;
    internal string mainMenuHowToPlay;
    internal string mainMenuOptions;
    internal string mainMenuQuitGame;
    internal string mainMenuButtonPlot;
    //general
    internal string backToMainMenu;
    internal string backToPauseScreen;
    internal string warningYes;
    internal string warningNo;
    internal string playerOneIndicator;
    internal string playerTwoIndicator;
    internal string acceptanceMessage;
    //game mode selection menu
    internal string gameModeSelectionMenuTitle;
    internal string gameModeSelectionMenuSinglePlayerClassic;
    internal string gameModeSelectionMenuSinglePlayerEndless;
    internal string gameModeSelectionMenuMultiPlayerClassic;
    internal string gameModeSelectionMenuMultiPlayerEndless;
    //skin selection menu
    internal string skinSelectionMenuTitle;
    //plot menu
    internal string plotMenuPlot;
    //how to play
    //internal string howToPlayMenuTitle;
    internal string howtoPlayMenuControlsPlayerOne;
    internal string howtoPlayMenuControlsPlayerTwo;
    //options
    internal string sfxVolumeSlider;
    internal string musicVolumeSlider;
    internal string quotesVolumeSlider;
    internal string activeLanguage;
    //regular hud
    internal string regularHudLevel;
    internal string regularHudBottle;
    internal string regularHudEarned0;
    internal string regularHudEarned1;
    internal string regularHudProgression0;
    internal string regularHudProgression1;
    internal string regularHudLandingMessage;
    internal string regularHudPlaneHit;
    internal string regularHudCongratulationsAfterLanding;
    //pause screen
    internal string pauseScreenPauseMainTitle;
    internal string pauseScreenGameOverMainTitle;
    internal string pauseScreenLevelTitle;
    internal string pauseScreenBottlesTitle;
    internal string pauseScreenResumeGame;
    internal string gameOverScreenTryAgain;
    internal string activeBottleWarning;
    internal string warningTitle;
    //color panel
    internal string colorPanelPlayerWins;
    internal string colorPanelPlayerLoses;
    internal string colorPanelPlayerLosesSinglePlayer;
    //game modes
    internal string classicModeMessage;
    internal string endlessSingleMessage;
    internal string endlessVersusMessage;
    //power Ups
    internal string soberUpYouAreSoberMessage;
    internal string invertedSteeringRepairedMessage;
    //disclaimer
    internal string disclaimerMessage;

    internal void LoadData()
    {
        List<string> listOfAll = new();
        if(localizationTextFile != null)
            listOfAll.AddRange(localizationTextFile.text.Split(Environment.NewLine));
        if (listOfAll.Count != 0)
        {
            mainMenuStartGame = listOfAll[1];
            mainMenuHowToPlay = listOfAll[3];
            mainMenuOptions = listOfAll[5];
            mainMenuQuitGame = listOfAll[7];
            gameModeSelectionMenuTitle = listOfAll[9];
            gameModeSelectionMenuSinglePlayerClassic = listOfAll[11];
            gameModeSelectionMenuSinglePlayerEndless = listOfAll[13];
            gameModeSelectionMenuMultiPlayerClassic = listOfAll[15];
            gameModeSelectionMenuMultiPlayerEndless = listOfAll[17];
            skinSelectionMenuTitle = listOfAll[19];
            plotMenuPlot = listOfAll[23];
            //howToPlayMenuTitle = listOfAll[21];
            howtoPlayMenuControlsPlayerOne = listOfAll[25];
            howtoPlayMenuControlsPlayerTwo = listOfAll[27];
            playerOneIndicator = listOfAll[29];
            playerTwoIndicator = listOfAll[31];
            backToMainMenu = listOfAll[33];
            sfxVolumeSlider = listOfAll[35];
            musicVolumeSlider = listOfAll[37];
            quotesVolumeSlider = listOfAll[39];
            activeLanguage = listOfAll[41];
            regularHudLevel = listOfAll[43];
            regularHudProgression0 = listOfAll[45];
            regularHudProgression1 = listOfAll[47];
            regularHudLandingMessage = listOfAll[49];
            regularHudPlaneHit = listOfAll[51];
            regularHudBottle = listOfAll[53];
            regularHudEarned0 = listOfAll[55];
            regularHudEarned1 = listOfAll[57];
            pauseScreenLevelTitle = listOfAll[59];
            pauseScreenBottlesTitle = listOfAll[61];
            pauseScreenPauseMainTitle = listOfAll[63];
            pauseScreenGameOverMainTitle = listOfAll[65];
            pauseScreenResumeGame = listOfAll[67];
            gameOverScreenTryAgain = listOfAll[69];
            warningTitle = listOfAll[71];
            warningYes = listOfAll[73];
            warningNo = listOfAll[75];
            colorPanelPlayerWins = listOfAll[77];
            colorPanelPlayerLoses = listOfAll[79];
            colorPanelPlayerLosesSinglePlayer = listOfAll[81];
            activeBottleWarning = listOfAll[83];
            classicModeMessage = listOfAll[85];
            endlessSingleMessage = listOfAll[87];
            endlessVersusMessage = listOfAll[89];
            //soberUpYouAreSoberMessage = listOfAll[91];
            //invertedSteeringRepairedMessage = listOfAll[93];
            regularHudCongratulationsAfterLanding = listOfAll[95];
            mainMenuButtonPlot = listOfAll[97];
            backToPauseScreen = listOfAll[99];
            acceptanceMessage = listOfAll[101];
            disclaimerMessage = listOfAll[103];
        }
    }
}
