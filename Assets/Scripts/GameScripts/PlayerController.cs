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
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Speed", true);
            body.AddForce(transform.up * bigFart);
        }
        else
        {
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Speed", false);
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            TurnTo(false);
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Rotate", true);
            Transform trans = transform.Find("Spaceman/FartPos");
            body.AddForceAtPosition(trans.up * rotateFart, trans.position);
        }
        else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            TurnTo(true);
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Rotate", true);
            Transform trans = transform.Find("Spaceman/FartPos");
            body.AddForceAtPosition(trans.up * rotateFart, trans.position);
        }
        else
        {
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Rotate", false);
        }
    }

    private void TurnTo(bool right)
    {
        transform.Find("Spaceman").localScale = new Vector3(right ? 1 : -1, 1, 1);
    }
}
