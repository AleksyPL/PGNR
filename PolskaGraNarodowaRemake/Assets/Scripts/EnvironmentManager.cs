using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [System.Serializable]
    public class Environment
    {
        public GameObject backgroundPrefab;
        public GameObject[] verticalObstaclesPrefabs;
        public GameObject fogPrefab;
        public Rain rainEnabled;
    };
    public enum Rain
    {
        on,
        off,
        random
    };
    internal FlightController flightControllerScript;
    public GameObject backgroundsMainGameObject;
    public Environment[] environmentsScenarios;
    internal int scenarioIndex;
    public GameObject cameraGameObjectPlayerOne;
    public GameObject cameraGameObjectPlayerTwo;
    internal void SpawnBackgroundImage()
    {
        flightControllerScript = GetComponent<FlightController>();
        ClearBackgroundImage();
        scenarioIndex = Random.Range(0, environmentsScenarios.Length);
        {
            GameObject newBackground = Instantiate(environmentsScenarios[scenarioIndex].backgroundPrefab);
            newBackground.transform.name = "BackgroundUp";
            newBackground.transform.parent = backgroundsMainGameObject.transform;
            newBackground.GetComponent<ParallaxManager>().BackgroundSetup(newBackground.GetComponent<ParallaxManager>().upperScreenY, cameraGameObjectPlayerOne);
        }
        if (flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusClassic || flightControllerScript.gameModeScript.currentGameMode == GameModeManager.GameMode.versusEndless)
        {
            GameObject newBackground = Instantiate(environmentsScenarios[scenarioIndex].backgroundPrefab);
            newBackground.transform.name = "BackgroundDown";
            newBackground.transform.parent = backgroundsMainGameObject.transform;
            newBackground.GetComponent<ParallaxManager>().BackgroundSetup(newBackground.GetComponent<ParallaxManager>().lowerScreenY, cameraGameObjectPlayerTwo);
        }
        if (environmentsScenarios[scenarioIndex].rainEnabled == Rain.on)
            TurnOnTheRain();
        else if (environmentsScenarios[scenarioIndex].rainEnabled == Rain.off)
            TurnOffTheRain();
        else if(environmentsScenarios[scenarioIndex].rainEnabled == Rain.random)
        {
            if(Random.Range(0, 2) == 0)
                TurnOffTheRain();
            else
                TurnOnTheRain();
        }
    }
    private void ClearBackgroundImage()
    {
        while (backgroundsMainGameObject.transform.childCount > 0)
            DestroyImmediate(backgroundsMainGameObject.transform.GetChild(0).gameObject);
    }
    private void TurnOnTheRain()
    {
        cameraGameObjectPlayerOne.transform.Find("RainEffect").gameObject.SetActive(true);
        if (cameraGameObjectPlayerTwo != null)
            cameraGameObjectPlayerTwo.transform.Find("RainEffect").gameObject.SetActive(true);
    }
    private void TurnOffTheRain()
    {
        cameraGameObjectPlayerOne.transform.Find("RainEffect").gameObject.SetActive(false);
        if (cameraGameObjectPlayerTwo != null)
            cameraGameObjectPlayerTwo.transform.Find("RainEffect").gameObject.SetActive(false);
    }
}
