using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skin")]
public class SkinManager : ScriptableObject
{
    public Sprite planeWithoutWheels;
    public Sprite planeWithHoles;
    public Sprite planeWithWheels;
    public Sprite planeCrashed;
}
