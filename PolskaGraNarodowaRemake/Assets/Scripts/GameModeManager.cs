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
    [SerializeField] internal Plane playerOnePlane;
    [SerializeField] internal Plane playerTwoPlane;
    internal FlightController flightController;
    //internal FadeOutTool fadeOutToolscript;
    public GameMode currentGameMode;
    
    private void OnEnable()
    {
        Application.targetFrameRate = 144;
    }
    private void Start()
    {
        playerOnePlane.LoadPlaneData(0);
        playerTwoPlane.LoadPlaneData(1);
        flightController = GetComponent<FlightController>();
    }
}
