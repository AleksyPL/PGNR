using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTool : MonoBehaviour
{
    public void DestroyGameObject()
    {
        Destroy(transform.gameObject);
    }
}
