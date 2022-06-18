using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    internal PlaneBase planeBaseScript;
    public GameObject planeGameObject;
    [SerializeField] private GameObject yearGameObject;
    [SerializeField] private GameObject levelProgressGameObject;
    [SerializeField] private GameObject scoreGameObject;
    [SerializeField] private GameObject bottlesGameObject;
    void Start()
    {
        planeBaseScript = planeGameObject.GetComponent<PlaneBase>();
    }
    void Update()
    {
        UpdateUI();
    }
    internal void UpdateUI()
    {
        //current year
        yearGameObject.GetComponent<Text>().text = ("Rok: " + (2009 + planeBaseScript.levelManagerScript.levelCounter)).ToString();
        //level progress
        if (planeBaseScript.currentPlaneState == PlaneBase.StateMachine.standard || planeBaseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
        {
            if (planeBaseScript.levelManagerScript.levelProgress < planeBaseScript.levelManagerScript.currentlevelDistance)
            {
                int levelProgress = (int)(planeBaseScript.levelManagerScript.levelProgress / planeBaseScript.levelManagerScript.currentlevelDistance * 100);
                levelProgressGameObject.GetComponent<Text>().text = ("Panie Prezydencie, przelecieliœmy ju¿: " + levelProgress + "% trasy. \n Jakoœ to bêdzie!").ToString();
            }
            else if (planeBaseScript.levelManagerScript.levelProgress >= planeBaseScript.levelManagerScript.currentlevelDistance)
                levelProgressGameObject.GetComponent<Text>().text = "Panie Prezydencie, l¹dujemy!";
        }
        else if (planeBaseScript.currentPlaneState == PlaneBase.StateMachine.damaged)
            levelProgressGameObject.GetComponent<Text>().text = "Panie Prezydencie, obawiam siê, ¿e siê rozpierdolimy";
        else if (planeBaseScript.currentPlaneState == PlaneBase.StateMachine.crashed)
            levelProgressGameObject.GetComponent<Text>().text = "No i chuj no i czeœæ";
        //bottles
        if (planeBaseScript.difficultyScript.difficultyMultiplier == 0)
            bottlesGameObject.GetComponent<Text>().text = "Wypite butelki: nic";
        else
            bottlesGameObject.GetComponent<Text>().text = ("Wypite butelki: " + planeBaseScript.difficultyScript.difficultyMultiplier).ToString();
        //score
        scoreGameObject.GetComponent<Text>().text = ("Zarobi³eœ: " + planeBaseScript.levelManagerScript.gameScore + " z³").ToString();
    }
}
