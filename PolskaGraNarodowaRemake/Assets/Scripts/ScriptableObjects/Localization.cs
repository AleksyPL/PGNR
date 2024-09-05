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
    internal string controlsMenuShootAlt;
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
    internal string tutorialScreen1ProgressBar;
    internal string tutorialScreen2;
    internal string tutorialScreen2ProgressBar;
    internal string tutorialScreen3;
    internal string tutorialScreen4;
    internal string tutorialScreen5;
    internal string tutorialScreen5ProgressBar;
    internal string tutorialScreen6;
    internal string tutorialScreen7;
    internal string tutorialScreen7ProgressBar;
    internal string tutorialScreen8;
    internal string tutorialScreen9;
    internal string tutorialScreen9ProgressBar;
    internal string tutorialScreen10;

    internal void LoadData()
    {
        List<string> listOfAll = new();
        if(localizationTextFile != null)
            listOfAll.AddRange(localizationTextFile.text.Split(Environment.NewLine));
        if (listOfAll.Count != 0)
        {
            for(int i=0;i<listOfAll.Count;i++)
                listOfAll[i] = listOfAll[i].Replace("\r", "");
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
            controlsMenuShootAlt = listOfAll[47];
            controlsMenuPauseMenu = listOfAll[49];
            //options
            sfxVolumeSlider = listOfAll[51];
            musicVolumeSlider = listOfAll[53];
            quotesVolumeSlider = listOfAll[55];
            activeLanguage = listOfAll[57];
            //regular hud
            regularHudLevel = listOfAll[59];
            regularHudProgression0 = listOfAll[61];
            regularHudProgression1 = listOfAll[63];
            regularHudLandingMessage = listOfAll[65];
            regularHudPlaneHit = listOfAll[67];
            regularHudBottle = listOfAll[69];
            regularHudEarned0 = listOfAll[71];
            regularHudEarned1 = listOfAll[73];
            regularHudCongratulationsAfterLanding = listOfAll[75];
            //pause screen
            pauseScreenLevelTitle = listOfAll[77];
            pauseScreenBottlesTitle = listOfAll[79];
            pauseScreenPauseMainTitle = listOfAll[81];
            pauseScreenGameOverMainTitle = listOfAll[83];
            pauseScreenResumeGame = listOfAll[85];
            gameOverScreenTryAgain = listOfAll[87];
            warningTitle = listOfAll[89];
            activeBottleWarning = listOfAll[91];
            //color panel
            colorPanelPlayerWins = listOfAll[93];
            colorPanelPlayerLoses = listOfAll[95];
            colorPanelPlayerLosesSinglePlayer = listOfAll[97];
            //game modes
            classicModeMessage = listOfAll[99];
            endlessSingleMessage = listOfAll[101];
            endlessVersusMessage = listOfAll[103];
            //disclaimer
            disclaimerMessage = listOfAll[105];
            disclaimerTitle = listOfAll[107];
            //tutorial
            tutorialQuestion = listOfAll[109];
            tutorialTitle = listOfAll[111];
            tutorialTryAgain = listOfAll[113];
            tutorialScreen0 = listOfAll[115];
            tutorialScreen1 = listOfAll[117];
            tutorialScreen1ProgressBar = listOfAll[119];
            tutorialScreen2 = listOfAll[121];
            tutorialScreen2ProgressBar = listOfAll[123];
            tutorialScreen3 = listOfAll[125];
            tutorialScreen4 = listOfAll[127];
            tutorialScreen5 = listOfAll[129];
            tutorialScreen5ProgressBar = listOfAll[131];
            tutorialScreen6 = listOfAll[133];
            tutorialScreen7 = listOfAll[135];
            tutorialScreen7ProgressBar = listOfAll[137];
            tutorialScreen8 = listOfAll[139];
            tutorialScreen9 = listOfAll[141];
            tutorialScreen9ProgressBar = listOfAll[143];
            tutorialScreen10 = listOfAll[145];
        }
    }
}
