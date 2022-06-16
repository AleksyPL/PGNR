using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    internal float lifeTime;
    private float lifeTimeCounter;
    void Start()
    {
        lifeTime = 5f;
        lifeTimeCounter = 0;
    }
    void Update()
    {
        lifeTimeCounter += Time.deltaTime;
        if (lifeTimeCounter >= lifeTime)
            Destroy(gameObject);
    }
}
