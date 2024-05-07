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
    internal string mainMenuControls;
    internal string mainMenuOptions;
    internal string mainMenuQuitGame;
    internal string mainMenuPlot;
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
    internal string gameModeSelectionMenuMissingGameModes;
    //skin selection menu
    internal string skinSelectionMenuTitle;
    //plot menu
    internal string plotMenuPlot;
    //controls
    internal string controlsMenuUp;
    internal string controlsMenuDown;
    internal string controlsMenuShoot;
    internal string controlsMenuPauseMenu;
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
    //disclaimer
    internal string disclaimerTitle;
    internal string disclaimerMessage;
    //tutorial
    internal string tutorialQuestion;
    internal string tutorialTitle;
    internal string tutorialTryAgain;
    internal string tutorialScreen0;
    internal string tutorialScreen1;

    internal void LoadData()
    {
        List<string> listOfAll = new();
        if(localizationTextFile != null)
            listOfAll.AddRange(localizationTextFile.text.Split(Environment.NewLine));
        if (listOfAll.Count != 0)
        {
            //main menu
            mainMenuStartGame = listOfAll[1];
            mainMenuControls = listOfAll[3];
            mainMenuOptions = listOfAll[5];
            mainMenuPlot = listOfAll[7];
            mainMenuQuitGame = listOfAll[9];
            //general
            backToMainMenu = listOfAll[11];
            backToPauseScreen = listOfAll[13];
            warningYes = listOfAll[15];
            warningNo = listOfAll[17];
            playerOneIndicator = listOfAll[19];
            playerTwoIndicator = listOfAll[21];
            acceptanceMessage = listOfAll[23];
            //game mode selection menu
            gameModeSelectionMenuTitle = listOfAll[25];
            gameModeSelectionMenuSinglePlayerClassic = listOfAll[27];
            gameModeSelectionMenuSinglePlayerEndless = listOfAll[29];
            gameModeSelectionMenuMultiPlayerClassic = listOfAll[31];
            gameModeSelectionMenuMultiPlayerEndless = listOfAll[33];
            gameModeSelectionMenuMissingGameModes = listOfAll[35];
            //skin selection menu
            skinSelectionMenuTitle = listOfAll[37];
            //plot menu
            plotMenuPlot = listOfAll[39];
            //controls
            controlsMenuUp = listOfAll[41];
            controlsMenuDown = listOfAll[43];
            controlsMenuShoot = listOfAll[45];
            controlsMenuPauseMenu = listOfAll[47];
            //options
            sfxVolumeSlider = listOfAll[49];
            musicVolumeSlider = listOfAll[51];
            quotesVolumeSlider = listOfAll[53];
            activeLanguage = listOfAll[55];
            //regular hud
            regularHudLevel = listOfAll[57];
            regularHudProgression0 = listOfAll[59];
            regularHudProgression1 = listOfAll[61];
            regularHudLandingMessage = listOfAll[63];
            regularHudPlaneHit = listOfAll[65];
            regularHudBottle = listOfAll[67];
            regularHudEarned0 = listOfAll[69];
            regularHudEarned1 = listOfAll[71];
            regularHudCongratulationsAfterLanding = listOfAll[73];
            //pause screen
            pauseScreenLevelTitle = listOfAll[75];
            pauseScreenBottlesTitle = listOfAll[77];
            pauseScreenPauseMainTitle = listOfAll[79];
            pauseScreenGameOverMainTitle = listOfAll[81];
            pauseScreenResumeGame = listOfAll[83];
            gameOverScreenTryAgain = listOfAll[85];
            warningTitle = listOfAll[87];
            activeBottleWarning = listOfAll[89];
            //color panel
            colorPanelPlayerWins = listOfAll[91];
            colorPanelPlayerLoses = listOfAll[93];
            colorPanelPlayerLosesSinglePlayer = listOfAll[95];
            //game modes
            classicModeMessage = listOfAll[97];
            endlessSingleMessage = listOfAll[99];
            endlessVersusMessage = listOfAll[101];
            //disclaimer
            disclaimerMessage = listOfAll[103];
            disclaimerTitle = listOfAll[105];
            //tutorial
            tutorialQuestion = listOfAll[107];
            tutorialTitle = listOfAll[109];
            tutorialTryAgain = listOfAll[111];
            tutorialScreen0 = listOfAll[113];
            tutorialScreen1 = listOfAll[115];
        }
    }
}
