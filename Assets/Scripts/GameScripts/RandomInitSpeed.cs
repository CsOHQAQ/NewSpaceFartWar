using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInitSpeed : MonoBehaviour
{
    public float vel = 1f;
    public float aVel = 1f;

    private void Start()
    {
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        rig.angularVelocity = Random.Range(-aVel, aVel);
        rig.velocity = Random.insideUnitCircle * vel;
    }
}
