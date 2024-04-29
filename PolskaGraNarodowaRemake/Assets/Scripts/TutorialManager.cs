using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    internal FlightController flightControllerScript;
    internal enum TutorialPlayerState
    {
        Flying,
        Frozen,
        Reverting
    }
    internal TutorialPlayerState currentState;
    //Tutorial info screens
    public GameObject[] tutorialScreens;
    //Revert Effect
    private List<Vector3> playerPositions;
    internal float elapsedTime;
    [Range(0.1f, 0.99f)]
    public float positionRevertMinDuration;
    [Range(1, 5)]
    public float positionRevertMaxDuration;
    public float positionRevertSpeed;
    private float positonRevertTime;
    private float positionRevertLerpValue;
    //Checkpoint facts
    internal int checkpointNumber;
    internal bool checkpointFinished;
    internal bool checkpointGoalAchieved;
    void Start()
    {
        flightControllerScript = GetComponent<FlightController>();
        playerPositions = new List<Vector3>();
        currentState = TutorialPlayerState.Frozen;
        flightControllerScript.uiManagerScript.EnableTutorialScreen();
        checkpointNumber = 0;
        positionRevertLerpValue = 0;
        checkpointFinished = false;
        checkpointGoalAchieved = false;
    }
    void Update()
    {
        if (currentState == TutorialPlayerState.Flying)
            SavePlayerPosition();
        else if (currentState == TutorialPlayerState.Reverting)
            CalculateNewPlanePosition();
    }
    private void SavePlayerPosition()
    {
        playerPositions.Add(flightControllerScript.gameModeScript.playerOnePlane.planeGameObject.transform.position);
        elapsedTime += Time.deltaTime;
    }
    internal void CalculatePositionRevertDuration()
    {
        positonRevertTime = elapsedTime / positionRevertMaxDuration / positionRevertSpeed;
        positonRevertTime = Mathf.Clamp(positonRevertTime, positionRevertMinDuration, positionRevertMaxDuration);
        currentState = TutorialPlayerState.Reverting;
    }
    private void CalculateNewPlanePosition()
    {
        if (positionRevertLerpValue < 1)
        {
            positionRevertLerpValue += Time.deltaTime / positonRevertTime;
            int listIndex = (int)Mathf.Lerp(playerPositions.Count, 0, positionRevertLerpValue);
            flightControllerScript.RevertPlanePosition(flightControllerScript.gameModeScript.playerOnePlane, playerPositions[listIndex]);
        }
        else
            ClearPlayerPositionList();
    }
    private void ClearPlayerPositionList()
    {
        playerPositions.Clear();
        currentState = TutorialPlayerState.Frozen;
        elapsedTime = 0;
        positionRevertLerpValue = 0;
        checkpointFinished = false;
        flightControllerScript.uiManagerScript.EnableTutorialScreen();
    }
    internal void SpawnTutorialInfo(int arrayIndex)
    {
        if (tutorialScreens.Length >= arrayIndex)
            Instantiate(tutorialScreens[arrayIndex], flightControllerScript.uiManagerScript.tutorialMainGameObject.transform);
    }
    internal void OKButtonLogic()
    {
        if (!checkpointFinished & !checkpointGoalAchieved)
            currentState = TutorialPlayerState.Flying;
        else if (checkpointFinished && checkpointGoalAchieved)
        {
            checkpointNumber++;
            checkpointFinished = false;
            checkpointGoalAchieved = false;
            currentState = TutorialPlayerState.Flying;
            //final checkpoint
            if (checkpointNumber == 10)
                flightControllerScript.levelManagerScript.BackToMainMenu();
        }
        else if (checkpointFinished & !checkpointGoalAchieved)
            currentState = TutorialPlayerState.Reverting;
    }
}
