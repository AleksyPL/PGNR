using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPlane : MonoBehaviour
{
    public float lifeTime;
    private float lifeTimeCounter;

    void Update()
    {
        if (lifeTime > 0)
        {
            lifeTimeCounter += Time.deltaTime;
            if (lifeTimeCounter >= lifeTime)
                Destroy(transform.gameObject);
        }
        else
            Destroy(transform.gameObject);
    }
}
