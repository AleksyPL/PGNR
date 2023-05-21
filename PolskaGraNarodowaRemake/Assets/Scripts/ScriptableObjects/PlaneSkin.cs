using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skin")]
public class PlaneSkin : ScriptableObject
{
    public Sprite planeStandard;
    public Sprite planeHoles;
    public Sprite planeWheels;
    public Sprite planeCrashed;
    public string[] skinName;
}
