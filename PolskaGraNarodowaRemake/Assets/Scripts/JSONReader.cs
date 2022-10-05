using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using Newtonsoft.Json;
//public class Root
//{
//    public List<string> DrunkBottles { get; set; }
//    public List<string> Year { get; set; }
//    public List<string> Earned0 { get; set; }
//    public List<string> Earned1 { get; set; }
//    public List<string> Pause { get; set; }
//    public List<string> GameOver { get; set; }
//    public List<string> PauseButton0 { get; set; }
//    public List<string> PauseButton1 { get; set; }
//    public List<string> PauseButton2 { get; set; }
//    public List<string> GameOverButton0 { get; set; }
//    public List<string> GameOverButton1 { get; set; }
//    public List<string> Warning0 { get; set; }
//    public List<string> Warning1 { get; set; }
//    public List<string> Warning2 { get; set; }
//}
//public class Root
//{
//    public List<string> myStrings { get; set; }
//}

public class JSONReader : MonoBehaviour
{
    [System.Serializable]
    public class SingleArray
    {
        public string[] myStrings;
    }
    public TextAsset jsonFile;
    public List<SingleArray> localizationStrings;
    void Start()
    {
        //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonFile.text);
        //Debug.Log(myDeserializedClass);
        LoadDataFromJSON();
    }
    private void LoadDataFromJSON()
    {
        if(jsonFile != null)
        {
            //string json = Application.streamingAssetsPath + "PauseScreen.json";
            //string jsonContnet = File.ReadAllText(jsonFile);
            //LocalizationStrings stringsInJSON = JsonUtility.FromJson<LocalizationStrings>(Application.streamingAssetsPath + "MainMenu.json");
            Debug.Log(jsonFile.text);
            //Debug.Assert(localizationStrings);
            //localizationStrings = JsonUtility.FromJson<SingleArray>(Application.streamingAssetsPath + "MainMenu.json");
            //Debug.Log(localizationStrings.myStrings[0]);
            //Debug.Log(stringsInJSON);
            //foreach (LocalizationStrings strings in stringsInJSON.localizationStrings)
            //{
            //    Debug.Log(strings);
            //}
        }
    }
}
