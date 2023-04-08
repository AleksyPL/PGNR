using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public enum GameMode
    {
        singleplayer,
        versus
    }
    internal enum Playthrough
    {
        inProgress,
        finished
    }
    [SerializeField] internal Plane playerOnePlane;
    [SerializeField] internal Plane playerTwoPlane;
    internal FlightController flightController;
    public GameMode currentGameMode;
    internal Playthrough currentPlaythrough;
    
    private void OnEnable()
    {
        Application.targetFrameRate = 144;
    }
    private void Start()
    {
        playerOnePlane.LoadPlaneData(0);
        playerTwoPlane.LoadPlaneData(1);
        flightController = GetComponent<FlightController>();
        currentPlaythrough = Playthrough.inProgress;
    }
    private void Update()
    {
        if(currentGameMode == GameMode.versus)
        {
            if(playerOnePlane.currentPlaneState == PlaneState.crashed && playerTwoPlane.currentPlaneState == PlaneState.crashed)
            {
                currentPlaythrough = Playthrough.finished;
            }
        }
    }
    internal ref Plane ReturnAPlaneObject(GameObject plane)
    {
        if (plane == playerOnePlane.planeGameObject)
            return ref playerOnePlane;
        else
            return ref playerTwoPlane;
    }
}
