using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunPos : MonoBehaviour
{
    public float radius = 300f;
    public float aSpeed = 1;

    public void Update()
    {
        transform.position = new Vector3(Mathf.Cos(aSpeed * Time.time * Mathf.Deg2Rad), Mathf.Sin(aSpeed * Time.time * Mathf.Deg2Rad)) * radius;
    }
}
