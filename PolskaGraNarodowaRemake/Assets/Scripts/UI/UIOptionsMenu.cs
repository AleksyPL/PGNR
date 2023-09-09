using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIOptionsMenu : MonoBehaviour
{
    public GameplaySettings gameplaySettings;
    public GameObject volumeSFXSliderGameObject;
    public GameObject volumeMusicSliderGameObject;
    public GameObject volumeQuotesSliderGameObject;
    public GameObject languageSelectorGameObject;
    public GameObject activeLanguageGameObject;
    public GameObject backToMainMenuButton;
    private void OnEnable()
    {
        LoadValuesFromSettings();
    }
    private void Update()
    {
        UpdateSettingsUsingSliderValues();
        UpdateUIWithNewLanguage();
    }
    public void LoadValuesFromSettings()
    {
        volumeMusicSliderGameObject.GetComponent<Slider>().value = gameplaySettings.volumeMusic;
        volumeQuotesSliderGameObject.GetComponent<Slider>().value = gameplaySettings.volumeQuotes;
        volumeSFXSliderGameObject.GetComponent<Slider>().value = gameplaySettings.volumeSFX;
    }
    public void UpdateSettingsUsingSliderValues()
    {
        gameplaySettings.volumeMusic = volumeMusicSliderGameObject.GetComponent<Slider>().value;
        gameplaySettings.volumeQuotes = volumeQuotesSliderGameObject.GetComponent<Slider>().value;
        gameplaySettings.volumeSFX = volumeSFXSliderGameObject.GetComponent<Slider>().value;
    }
    private void UpdateUIWithNewLanguage()
    {
        List<string> names = new List<string>();
        foreach (string s in System.Enum.GetNames(typeof(Languages)))
            names.Add(s);
        activeLanguageGameObject.GetComponent<TMP_Text>().text = names[gameplaySettings.langauageIndex];
        volumeSFXSliderGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].sfxVolumeSlider;
        volumeMusicSliderGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].musicVolumeSlider;
        volumeQuotesSliderGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].quotesVolumeSlider;
        languageSelectorGameObject.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].activeLanguage;
        backToMainMenuButton.transform.Find("Text").GetComponent<TMP_Text>().text = gameplaySettings.localizationsStrings[gameplaySettings.langauageIndex].backToMainMenuButton;
    }
    public void NextLanguage()
    {
        gameplaySettings.langauageIndex++;
        if (gameplaySettings.langauageIndex > System.Enum.GetValues(typeof(Languages)).Length - 1)
            gameplaySettings.langauageIndex = 0;
    }
    public void PreviousLanguage()
    {
        gameplaySettings.langauageIndex--;
        if (gameplaySettings.langauageIndex < 0)
            gameplaySettings.langauageIndex = System.Enum.GetValues(typeof(Languages)).Length - 1;
    }
}
