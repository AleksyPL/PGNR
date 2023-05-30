using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTool : MonoBehaviour
{
    public float timeBeforeDestroy;
    private float timeBeforeDestroyCounter;
    private void Update()
    {
        timeBeforeDestroyCounter += Time.deltaTime;
        if(timeBeforeDestroyCounter>=timeBeforeDestroy)
        {
            DestroyGameObject();
        }
    }
    public void DestroyGameObject()
    {
        Destroy(transform.gameObject);
    }
}
