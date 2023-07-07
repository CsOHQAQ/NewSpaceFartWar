using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public float bigFart;
    public float rotateFart;

    private Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.velocity = Vector3.zero; ;
            body.angularVelocity = 0;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            body.AddForce(transform.up * bigFart);
        }
        if (Input.GetKey(KeyCode.A))
        {
            TurnTo(false);
            Transform trans = transform.Find("Spaceman/FartPos");
            body.AddForceAtPosition(trans.up * rotateFart, trans.position);
        }
        if (Input.GetKey(KeyCode.D))
        {
            TurnTo(true);
            Transform trans = transform.Find("Spaceman/FartPos");
            body.AddForceAtPosition(trans.up * rotateFart, trans.position);
        }
    }

    private void TurnTo(bool right)
    {
        transform.Find("Spaceman").transform.localScale = new Vector3(right ? 1 : -1, 1, 1);
    }
}
