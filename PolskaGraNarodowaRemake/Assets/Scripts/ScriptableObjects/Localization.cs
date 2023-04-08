using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Localization")]
public class Localization : ScriptableObject
{
    public TextAsset localizationFile;
    private void OnEnable()
    {
        LoadData();
    }
    private void LoadData()
    {

    }
}
