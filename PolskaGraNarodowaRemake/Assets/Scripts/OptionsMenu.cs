using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameplaySettings mySettings;
    public GameObject volumeSFXSliderGameObject;
    public GameObject volumeMusicSliderGameObject;
    public GameObject volumeQuotesSliderGameObject;
    private void OnEnable()
    {
        LoadValuesFromSettings();
    }
    public void LoadValuesFromSettings()
    {
        volumeMusicSliderGameObject.GetComponent<Slider>().value = mySettings.volumeMusic;
        volumeQuotesSliderGameObject.GetComponent<Slider>().value = mySettings.volumeQuotes;
        volumeSFXSliderGameObject.GetComponent<Slider>().value = mySettings.volumeSFX;
    }
    public void UpdateSettingsUsingSliderValues()
    {
        mySettings.volumeMusic = volumeMusicSliderGameObject.GetComponent<Slider>().value;
        mySettings.volumeQuotes = volumeQuotesSliderGameObject.GetComponent<Slider>().value;
        mySettings.volumeSFX = volumeSFXSliderGameObject.GetComponent<Slider>().value;
    }
}
