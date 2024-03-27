using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class UIOptionsMenu : MonoBehaviour
{
    public GameObject eventSystemGameObject;
    private EventSystem eventSystem;
    public GameplaySettings gameplaySettings;
    public GameObject volumeSFXSliderGameObject;
    public GameObject volumeMusicSliderGameObject;
    public GameObject volumeQuotesSliderGameObject;
    public GameObject languageSelectorGameObject;
    public GameObject activeLanguageGameObject;
    public GameObject backToPauseScreenButton;
    public GameObject audioManagerGameObject;
    public Color notAvailableButton;
    private void OnEnable()
    {
        LoadValuesFromSettings();
        UpdateUIWithNewLanguage();
        eventSystem = eventSystemGameObject.GetComponent<EventSystem>();
        if (!UnityEngine.Device.Application.isMobilePlatform)
            eventSystem.SetSelectedGameObject(backToPauseScreenButton);
        volumeMusicSliderGameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate { audioManagerGameObject.GetComponent<AudioManager>().UpdateAllSoundsVolume(); });
        volumeSFXSliderGameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate { audioManagerGameObject.GetComponent<AudioManager>().UpdateAllSoundsVolume(); });
        volumeQuotesSliderGameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate { audioManagerGameObject.GetComponent<AudioManager>().UpdateAllSoundsVolume(); });
    }
    public void LoadValuesFromSettings()
    {
        volumeMusicSliderGameObject.GetComponent<Slider>().value = gameplaySettings.volumeMusic;
        volumeQuotesSliderGameObject.GetComponent<Slider>().value = gameplaySettings.volumeQuotes;
        volumeSFXSliderGameObject.GetComponent<Slider>().value = gameplaySettings.volumeSFX;
        audioManagerGameObject.GetComponent<AudioManager>().UpdateAllSoundsVolume();
    }
    public void UpdateSettingsUsingSliderValues()
    {
        gameplaySettings.volumeMusic = volumeMusicSliderGameObject.GetComponent<Slider>().value;
        gameplaySettings.volumeQuotes = volumeQuotesSliderGameObject.GetComponent<Slider>().value;
        gameplaySettings.volumeSFX = volumeSFXSliderGameObject.GetComponent<Slider>().value;
    }
    private void UpdateUIWithNewLanguage()
    {
        List<string> names = new();
        foreach (string s in System.Enum.GetNames(typeof(Languages)))
            names.Add(s);
        activeLanguageGameObject.GetComponent<TMP_Text>().text = names[gameplaySettings.langauageIndex];
        volumeSFXSliderGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].sfxVolumeSlider;
        volumeMusicSliderGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].musicVolumeSlider;
        volumeQuotesSliderGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].quotesVolumeSlider;
        languageSelectorGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].activeLanguage;
        if(SceneManager.GetActiveScene().name == "MainMenu")
            backToPauseScreenButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenu;
        else
            backToPauseScreenButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToPauseScreen;
    }
    public void NextLanguage()
    {
        gameplaySettings.langauageIndex++;
        if (gameplaySettings.langauageIndex > System.Enum.GetValues(typeof(Languages)).Length - 1)
            gameplaySettings.langauageIndex = 0;
        UpdateUIWithNewLanguage();
    }
    public void PreviousLanguage()
    {
        gameplaySettings.langauageIndex--;
        if (gameplaySettings.langauageIndex < 0)
            gameplaySettings.langauageIndex = System.Enum.GetValues(typeof(Languages)).Length - 1;
        UpdateUIWithNewLanguage();
    }
    internal void DisableLanguageButtons()
    {
        languageSelectorGameObject.transform.Find("LeftArrow").GetComponent<Button>().enabled = false;
        languageSelectorGameObject.transform.Find("LeftArrow").GetComponent<Image>().color = notAvailableButton;
        languageSelectorGameObject.transform.Find("RightArrow").GetComponent<Button>().enabled = false;
        languageSelectorGameObject.transform.Find("RightArrow").GetComponent<Image>().color = notAvailableButton;
    }
}
