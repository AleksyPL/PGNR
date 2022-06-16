using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    internal Vector3 position;
    internal bool spaceHold;
    internal bool spaceReleased;
    void Start()
    {
        position = Vector3.zero;
        spaceHold = false;
    }
    void Update()
    {
        position.y = Input.GetAxisRaw("Vertical");
        position.Normalize();
        spaceHold = Input.GetButton("Jump");
        spaceReleased = Input.GetButtonUp("Jump");
    }
}
