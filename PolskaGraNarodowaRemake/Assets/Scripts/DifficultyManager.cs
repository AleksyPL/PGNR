using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    internal PlaneBase baseScript;
    internal float difficultyMultiplier;
    internal float difficultyImpulsTimeMin;
    internal float difficultyImpulsTimeMax;
    private float difficultyImpulsTimeCurrent;
    private float difficultuImpulseCounter;
    internal float difficultyImpulseDirection;
    internal bool enableDifficultyImpulses;
    internal bool altitudeChangeForceOverrided;
    [SerializeField] internal float altitudeChangeForceOverridedMultiplier;
    [SerializeField] internal float difficultyImpulseForce;

    void Start()
    {
        baseScript = GetComponent<PlaneBase>();
        difficultyImpulseDirection = 1f;
        difficultyMultiplier = 0;
        difficultyImpulsTimeMin = baseScript.levelManagerScript.levelCounter * 0.1f;
        difficultyImpulsTimeMax = 2 * difficultyImpulsTimeMin;
        difficultyImpulsTimeCurrent = Random.Range(difficultyImpulsTimeMin, difficultyImpulsTimeMax);
        difficultuImpulseCounter = difficultyImpulsTimeCurrent;
        enableDifficultyImpulses = false;
        altitudeChangeForceOverrided = false;
    }

    void Update()
    {
        if (baseScript.currentPlaneState == PlaneBase.StateMachine.standard || baseScript.currentPlaneState == PlaneBase.StateMachine.wheelsOn)
        {
            ApplyDifficultyImpulse();
        }
    }
    internal void ApplyDifficultyImpulse()
    {
        if (enableDifficultyImpulses)
        {
            if (baseScript.inputScript.position.y != difficultyImpulseDirection && baseScript.inputScript.position.y !=0 && !altitudeChangeForceOverrided)
            {
                baseScript.flightControllScript.altitudeChangeForce *= altitudeChangeForceOverridedMultiplier;
                altitudeChangeForceOverrided = true;
            }    
            else if ((baseScript.inputScript.position.y == difficultyImpulseDirection && altitudeChangeForceOverrided) || (baseScript.inputScript.position.y == 0 && altitudeChangeForceOverrided))
            {
                baseScript.flightControllScript.altitudeChangeForce /= altitudeChangeForceOverridedMultiplier;
                altitudeChangeForceOverrided = false;
            }
            difficultuImpulseCounter -= Time.deltaTime;
            transform.position += new Vector3(0, difficultyImpulseDirection * difficultyImpulseForce * difficultyMultiplier * Time.deltaTime, 0);
            if (difficultuImpulseCounter <= 0)
            {
                difficultyImpulsTimeCurrent = Random.Range(difficultyImpulsTimeMin, difficultyImpulsTimeMax);
                difficultuImpulseCounter = difficultyImpulsTimeCurrent;
                difficultyImpulseDirection = Random.Range(-1, 1);
                if (difficultyImpulseDirection == 0)
                    difficultyImpulseDirection = 1;
            }
        }
    }
}
