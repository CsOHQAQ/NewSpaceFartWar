using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeEffect : MonoBehaviour
{
    public float liftTime;

    private float counter;

    private void OnEnable()
    {
        counter = liftTime;
    }

    private void Update()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            DestroyThis();
        }
    }

    private void DestroyThis()
    {
        ObjectPool.Recycle(gameObject);
    }
}
